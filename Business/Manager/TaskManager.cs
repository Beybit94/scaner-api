using AutoMapper;
using Business.Models;
using Business.QueryModels.Task;
using Data.Queries.Data1c;
using Data.Queries.Good;
using Data.Queries.Task;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using static Business.Models.Dictionary.StandartDictionaries;

namespace Business.Manager
{
    public class TaskManager
    {
        private readonly TaskRepository _taskRepository;
        private readonly GoodRepository _goodRepository;
        private readonly Data1cRepository _data1CRepository;
        private IMapper _mapper;

        public TaskManager(TaskRepository taskRepository, GoodRepository goodRepository, Data1cRepository data1CRepository, IMapper mappper)
        {
            _taskRepository = taskRepository;
            _goodRepository = goodRepository;
            _data1CRepository = data1CRepository;
            _mapper = mappper;
        }

        public void UnloadTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            _taskRepository.GetPlanNum(query);

            var hTaskStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "Start");
            query.StatusId = hTaskStatus.Id;
            _taskRepository.UnloadTask(query);
        }

        public TasksModel GetActiveTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var hTaskStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "Start");
            query.StatusId = hTaskStatus.Id;

            var entity = _taskRepository.GetActiveTask(query);
            return _mapper.Map<TasksModel>(entity);
        }

        public void EndTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var hTaskStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "End");
            query.StatusId = hTaskStatus.Id;

            _taskRepository.EndTask(query);
        }

        public void CloseTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            _taskRepository.CloseTask(query);
        }

        public List<ReceiptModel> Differences(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));

            var query = _mapper.Map<TaskQuery>(queryModel);

            var result = new List<ReceiptModel>();
            var goodQuery = new GoodQuery { TaskId = query.TaskId };
            var data1cQuery = new Data1cQuery { PlanNum = query.PlanNum };

            var goods = _goodRepository.GetGoods(goodQuery);
            var docdatas = _data1CRepository.DocDataByPlanNum(data1cQuery);

            //Товары в коробе
            foreach (var good in goods.Where(m => !string.IsNullOrEmpty(m.GoodArticle))
                                      .Where(m => m.BoxId.HasValue && m.BoxId.Value != 0))
            {
                var diff = new ReceiptModel
                {
                    GoodName = good.GoodName,
                    Article = good.GoodArticle,
                    CountQty = good.CountQty,
                    GoodBarcode = good.BarCode
                };

                var box = goods.FirstOrDefault(m => m.Id == good.BoxId);
                diff.Barcode = box.BarCode;

                var _docDatas = docdatas.Where(m => m.Article == good.GoodArticle &&
                                                          m.PlanNum == data1cQuery.PlanNum &&
                                                          m.Barcode == box.BarCode);

                //Пропускаем где по одному Article несколько записей
                var length = _docDatas.Count();
                if (length > 1) continue;

                //Если пусто выбираем любой РОТ
                var _docData = _docDatas.FirstOrDefault();
                if (_docData == null) _docData = docdatas.FirstOrDefault();

                diff.NumberDoc = _docData.NumberDoc;
                diff.DateDoc = _docData.DateDoc ?? DateTime.Now;
                diff.Location = _docData.LocationGuid;
                diff.Quantity = length <= 0 ? 0 : _docData.Quantity;

                result.Add(diff);
            }

            //Товары вне короба
            foreach (var good in goods.Where(m => !string.IsNullOrEmpty(m.GoodArticle))
                                      .Where(m => !m.BoxId.HasValue || m.BoxId == 0))
            {
                var diff = new ReceiptModel
                {
                    GoodName = good.GoodName,
                    Article = good.GoodArticle,
                    CountQty = good.CountQty,
                    GoodBarcode = good.BarCode,
                    Barcode = "0"
                };

                var _docDatas = docdatas.Where(m => m.Article == good.GoodArticle &&
                                                          m.PlanNum == data1cQuery.PlanNum &&
                                                          m.Barcode == "0");

                //Пропускаем где по одному Article несколько записей
                var length = _docDatas.Count();
                if (length > 1) continue;

                //Если пусто выбираем любой РОТ
                var _docData = _docDatas.FirstOrDefault();
                if (_docData == null) _docData = docdatas.FirstOrDefault();

                diff.NumberDoc = _docData.NumberDoc;
                diff.DateDoc = _docData.DateDoc ?? DateTime.Now;
                diff.Location = _docData.LocationGuid;
                diff.Quantity = length <= 0 ? 0 : _docData.Quantity;

                result.Add(diff);
            }

            //Товар из 1с
            foreach (var good in docdatas.Where(m => !result.Any(r => r.Article == m.Article && r.Barcode == m.Barcode))
                                        .GroupBy(m => m.Article))
            {
                if (good.Count() > 1) continue;

                var _docData = good.FirstOrDefault();
                var diff = new ReceiptModel
                {
                    NumberDoc = _docData.NumberDoc,
                    DateDoc = _docData.DateDoc ?? DateTime.Now,
                    Location = _docData.LocationGuid,
                    CountQty = 0,
                    Quantity = _docData.Quantity,
                    Barcode = _docData.Barcode
                };

                goodQuery.GoodArticle = _docData.Article;
                var _good = _goodRepository.GetGoodsByFilter(goodQuery).FirstOrDefault();

                diff.GoodName = _good != null ? _good.GoodName : "";
                diff.Article = _good != null ? _good.GoodArticle : "";

                result.Add(diff);
            }

            //Несколько товаров по Article
            foreach (var good in docdatas.GroupBy(m => m.Article).Where(m => m.Count() > 1))
            {
                var article = good.FirstOrDefault().Article;
                var _good = goods.FirstOrDefault(m => m.GoodArticle == article);

                var _docDatas = good.Select(m => new { m.PlanNum, m.NumberDoc, m.DateDoc, m.LocationGuid, m.Article, m.Quantity, m.Barcode });

                var quantity = _good != null ? _good.CountQty : 0;
                foreach (var _docData in _docDatas)
                {
                    var diff = new ReceiptModel
                    {
                        NumberDoc = _docData.NumberDoc,
                        DateDoc = _docData.DateDoc ?? DateTime.Now,
                        Location = _docData.LocationGuid,
                        Quantity = _docData.Quantity,
                        Barcode = _docData.Barcode
                    };

                    diff.GoodName = _good != null ? _good.GoodName : "";
                    diff.Article = _good != null ? _good.GoodArticle : "";

                    if (quantity <= 0)
                    {
                        diff.CountQty = 0;
                    }
                    else
                    {
                        diff.CountQty = quantity >= _docData.Quantity ? _docData.Quantity : quantity;
                        quantity = quantity - _docData.Quantity;
                    }
                    result.Add(diff);
                }
            }

            return result.ToList();
        }

        public void SaveAct(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var hFileType = CacheDictionaryManager.GetDictionaryShort<hFileType>().FirstOrDefault(d => d.Code == "Act_Photo");
            query.TypeId = hFileType.Id;

            _taskRepository.SaveAct(query);
        }

        public List<ReceiptModel> PrepareDataTo1c(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var hTaskStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "End");
            var hProcessType = CacheDictionaryManager.GetDictionaryShort<hProcessType>().FirstOrDefault(d => d.Code == "SendTo1C");

            query.StatusId = hTaskStatus.Id;
            query.ProcessTypeId = hProcessType.Id;
            var tasks = _taskRepository.GetTasksByStatus(query);

            var receipts = new List<ReceiptModel>();
            foreach (var item in tasks)
            {
                queryModel.TaskId = item.Id;
                queryModel.PlanNum = item.PlanNum.Replace("\n", "").Replace("\r", "");
                var _receipts = Differences(queryModel);

                receipts.AddRange(_receipts);
            }

            return receipts;
        }

        public void Set1cStatus(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<Data1cQuery>(queryModel);

            var hProcessType = CacheDictionaryManager.GetDictionaryShort<hProcessType>().FirstOrDefault(d => d.Code == "SendTo1C");
            query.ProcessTypeId = hProcessType.Id;

            _data1CRepository.SetDataTo1c(query);
        }
    }
}
