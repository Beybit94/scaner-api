using AutoMapper;
using Business.Models;
using Business.QueryModels.Data1c;
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

            var InProcess = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "In process");
            var Start = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "Start");
            var End = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "End");

            query.Start = Start.Id;
            query.InProcess = InProcess.Id;
            query.End = End.Id;

            _taskRepository.UnloadTask(query);
        }

        public TasksModel GetActiveTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var hEndStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "Start");
            var hInProcessStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "In process");

            query.Start = hEndStatus.Id;
            query.InProcess = hInProcessStatus.Id;

            var entity = _taskRepository.GetActiveTask(query);
            return _mapper.Map<TasksModel>(entity);
        }

        public TasksModel GetTaskById(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var entity = _taskRepository.GetTaskById(query);
            return _mapper.Map<TasksModel>(entity);
        }

        public void EndTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var StartStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "Start");
            var EndStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "End");
            var InProcessStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "In process");

            var task = _taskRepository.GetTaskById(query);
            if (task.StatusId == StartStatus.Id)
            {
                var hFileType = CacheDictionaryManager.GetDictionaryShort<hFileType>().FirstOrDefault(d => d.Code == "Act_Photo");
                var files = _taskRepository.FilesByTask(query);

                if (!files.Any(m => m.TypeId == hFileType.Id))
                {
                    var goods = _goodRepository.GetGoodsByTask(new GoodQuery { TaskId = task.Id });
                    if (goods.Any(m => m.DefectId.HasValue)) throw new Exception("Фото акта не прикреплен");

                    var difference = Differences(new TaskQueryModel { TaskId = task.Id, PlanNum = task.PlanNum });
                    if (difference.receipts.Any(m => m.CountQty != m.Quantity)) throw new Exception("Фото акта не прикреплен");
                }

                query.StatusId = InProcessStatus.Id;
            }
            else if (task.StatusId == InProcessStatus.Id)
            {
                query.StatusId = EndStatus.Id;
                query.EndDateTime = DateTime.Now;
            }

            _taskRepository.SetStaus(query);
        }

        public void CloseTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var Status = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "Deleted");
            query.StatusId = Status.Id;
            _taskRepository.SetStaus(query);
            _taskRepository.CloseTask(query);
        }

        public DifferencesModel Differences(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));

            var query = _mapper.Map<TaskQuery>(queryModel);

            var StartStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "Start");
            var task = _taskRepository.GetTaskById(new TaskQuery { TaskId = query.TaskId });

            var goods = _goodRepository.GetGoods(new GoodQuery { TaskId = query.TaskId });
            var docdatas = _data1CRepository.DocDataByPlanNum(new Data1cQuery { PlanNum = query.PlanNum });

            var diff = new DifferencesModel();

            //Отсканированные товары/короб
            foreach (var good in goods.Where(m => m.GoodId != 0))
            {
                var receipt = new ReceiptModel
                {
                    GoodName = good.GoodName,
                    Article = good.GoodArticle,
                    CountQty = good.CountQty,
                    GoodBarcode = good.BarCode
                };

                GoodsModel box = null;
                if (good.BoxId.HasValue && good.BoxId != 0)
                {
                    box = _mapper.Map<GoodsModel>(goods.FirstOrDefault(m => m.Id == good.BoxId));
                    receipt.Barcode = box.BarCode;
                }
                else
                {
                    receipt.Barcode = "0";
                }

                var _docDatas = docdatas.Where(m => m.Article == good.GoodArticle &&
                                                    m.Barcode == (box != null ? box.BarCode : "0"));

                //Пропускаем где по одному Article несколько записей
                var length = _docDatas.Count();
                if (length > 1) continue;

                //Если пусто выбираем любой РОТ
                var docData = length > 0 ? _docDatas.FirstOrDefault() : docdatas.FirstOrDefault();

                if (docData == null)
                {
                    receipt.NumberDoc = "";
                    receipt.DateDoc = DateTime.Now;
                    receipt.Location = "";
                    receipt.Quantity = 0;
                }
                else
                {
                    receipt.NumberDoc = docData.NumberDoc;
                    receipt.DateDoc = docData.DateDoc ?? DateTime.Now;
                    receipt.Location = docData.LocationGuid;
                    receipt.Quantity = length <= 0 ? 0 : docData.Quantity;
                }

                //if (receipt.CountQty == receipt.Quantity) continue;

                if (good.DefectId.HasValue)
                {
                    receipt.IsDefect = true;
                    receipt.DefectDate = good.Created;
                    receipt.Description = good.Defect.Description;
                    receipt.SerialNumber = good.Defect.SerialNumber;
                    receipt.DefectPercentage = good.Defect.Damage.ToString();
                }
                diff.receipts.Add(receipt);                    
            }
           //Товар из 1с
            foreach (var good in docdatas.GroupBy(m => m.Article))
            {
                var docData = good.FirstOrDefault();
                //По одному Article несколько записей
                if (good.Count() > 1)
                {
                    var findGood = goods.FirstOrDefault(m => m.GoodArticle == docData.Article);
                    if (findGood == null)
                    {
                        findGood = _goodRepository.GetGoodsByArticle(new GoodQuery { GoodArticle = docData.Article });
                    }

                    //var quantity = findGood.CountQty; // old value
                    var quantity = goods.Where(x => x.GoodArticle == findGood.GoodArticle).GroupBy(x => new { x.GoodArticle })
                      .Select(g => g.Sum(x => x.CountQty)).Sum();

                    foreach (var item in good.Select(m => new { m.PlanNum, m.NumberDoc, m.DateDoc, m.LocationGuid, m.Article, m.Quantity, m.Barcode }))
                    {
                        var receipt = new ReceiptModel
                        {
                            NumberDoc = item.NumberDoc,
                            DateDoc = item.DateDoc ?? DateTime.Now,
                            Location = item.LocationGuid,
                            Quantity = item.Quantity,
                            Barcode = item.Barcode,
                            Article = item.Article,
                            GoodBarcode = findGood.BarCode,
                            GoodName = findGood.GoodName
                        };

                        var goodBoxExists = goods.Any(x => x.GoodArticle == findGood.GoodArticle && x.BoxId != 0 && x.BoxName != item.Barcode);
                        var isEmptyCountQty = false;
                        if (!goodBoxExists && item.Barcode != "0" && !string.IsNullOrEmpty(item.Barcode))
                        {
                            isEmptyCountQty = true;
                        }

                        if (quantity <= 0 || isEmptyCountQty)
                        {
                            receipt.CountQty = 0;

                            if (isEmptyCountQty)
                            {
                                receipt.CountQty = quantity >= item.Quantity ? item.Quantity : quantity;
                                quantity = quantity - item.Quantity;
                            }
                        }
                        else
                        {
                            var goodSumCountQty = goods.Where(x => x.GoodArticle == item.Article && x.BoxName == item.Barcode).Sum(x => x.CountQty);
                            var goodSumCountQty1 = goods.Where(x => x.GoodArticle == item.Article && x.BoxName != item.Barcode).Sum(x => x.CountQty);
                            var selectNotBoxedGoods = goods.Where(x => x.GoodArticle == item.Article && x.BoxName != item.Barcode);

                            receipt.CountQty = quantity >= item.Quantity ? item.Quantity : quantity;
                            quantity = quantity - item.Quantity;
                        }

                        diff.receipts.Add(receipt);
                    }
                }
                else
                {
                    if (diff.receipts.Any(m => m.Article == docData.Article &&
                                             m.Barcode == docData.Barcode &&
                                             m.NumberDoc == docData.NumberDoc)) continue;

                    var receipt = new ReceiptModel
                    {
                        NumberDoc = docData.NumberDoc,
                        DateDoc = docData.DateDoc ?? DateTime.Now,
                        Location = docData.LocationGuid,
                        Quantity = docData.Quantity,
                        Barcode = docData.Barcode,
                        Article = docData.Article,
                        GoodBarcode = "",
                        CountQty = 0,
                    };

                    var findGood = _goodRepository.GetGoodsByArticle(new GoodQuery { GoodArticle = docData.Article });
                    receipt.GoodName = findGood != null ? findGood.GoodName : "";
                    //if (receipt.CountQty == receipt.Quantity) continue;
                    diff.receipts.Add(receipt);
                }
            }

            if (task.StatusId == StartStatus.Id)
            {
                diff.boxes = _mapper.Map<List<GoodsModel>>(goods.Where(m => m.GoodId == 0 && m.DefectId != null).ToList());
                foreach (var item in docdatas.Where(m => m.Barcode != "0").GroupBy(m => m.Barcode))
                {
                    if (goods.Any(m => m.BarCode == item.FirstOrDefault().Barcode)) continue;
                    diff.boxes.Add(new GoodsModel { BarCode = item.FirstOrDefault().Barcode });
                }
                diff.receipts = diff.receipts.Where(m => m.Barcode == "0" || diff.boxes.Any(b => b.BarCode == m.Barcode))
                                             .Where(m => m.CountQty != m.Quantity).ToHashSet();
                diff.receipts.Distinct();
            }
            else
            {
                diff.boxes = _mapper.Map<List<GoodsModel>>(goods.Where(m => m.GoodId == 0 && m.DefectId == null).ToList());
                foreach (var item in docdatas.Where(m => m.Barcode != "0").GroupBy(m => m.Barcode))
                {
                    if (goods.Any(m => m.BarCode == item.FirstOrDefault().Barcode)) continue;
                    diff.boxes.Add(new GoodsModel { BarCode = item.FirstOrDefault().Barcode });                  
                }
                diff.receipts = diff.receipts.Where(m => m.Barcode == "0" || diff.boxes.Any(b => b.BarCode == m.Barcode)) 
                                             .Where(m => m.CountQty != m.Quantity).ToHashSet();

               diff.receipts.Distinct();
            }

            return diff;
        }

        public void SaveAct(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var hFileType = CacheDictionaryManager.GetDictionaryShort<hFileType>().FirstOrDefault(d => d.Code == "Act_Photo");
            query.TypeId = hFileType.Id;
            query.TypeName = hFileType.Name;

            _taskRepository.SaveAct(query);
        }

        public List<ReceiptModel> PrepareDataTo1c(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var hTaskStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "End");
            var hProcessType = CacheDictionaryManager.GetDictionaryShort<hProcessType>().FirstOrDefault(d => d.Code == "SendTo1C");

            query.End = hTaskStatus.Id;
            query.ProcessTypeId = hProcessType.Id;

            var tasks = _taskRepository.GetTasksByStatus(query);
            var hFileType = CacheDictionaryManager.GetDictionaryShort<hFileType>().FirstOrDefault(d => d.Code == "Defect_Photo");

            var receipts = new List<ReceiptModel>();

            var boxReceipts = new List<ReceiptModel>();
            var notBoxReceipts = new List<ReceiptModel>();
            var docReceipts = new List<ReceiptModel>();
            var rotReceipts = new List<ReceiptModel>();

            foreach (var item in tasks)
            {
                var _receipts = new List<ReceiptModel>();

                var files = _taskRepository.FilesByTask(new TaskQuery { TaskId = item.Id });
                var goods = _goodRepository.GetGoods(new GoodQuery { TaskId = item.Id, PlanNum = item.PlanNum });
                var docdatas = _data1CRepository.DocDataByPlanNum(new Data1cQuery { PlanNum = item.PlanNum.Trim() });

                //Товары в коробе
                foreach (var good in goods.Where(m => !string.IsNullOrEmpty(m.GoodArticle))
                                          .Where(m => m.BoxId.HasValue && m.BoxId.Value != 0))
                {
                    var box = goods.FirstOrDefault(m => m.Id == good.BoxId);
                    var _docDatas = docdatas.Where(m => m.Article == good.GoodArticle &&
                                                              m.PlanNum == item.PlanNum &&
                                                              m.Barcode == box.BarCode);
                    //Пропускаем где по одному Article несколько записей
                    var length = _docDatas.Count();
                    if (length > 1) continue;

                    //Если пусто выбираем любой РОТ
                    var _docData = _docDatas.FirstOrDefault();
                    if (_docData == null) _docData = docdatas.FirstOrDefault();
                    if (_docData == null) continue;

                    var _receipt = new ReceiptModel
                    {
                        TaskId = item.Id,
                        PlanNum = _docData.PlanNum,
                        NumberDoc = _docData.NumberDoc,
                        DateDoc = _docData.DateDoc.Value,
                        Location = _docData.LocationGuid,
                        DateBeginLoad = item.CreateDateTime,
                        DateEndLoad = item.EndDateTime ?? DateTime.Now,
                        Article = good.GoodArticle,
                        Quantity = good.CountQty,
                        GoodBarcode = good.BarCode,
                        Barcode = box.BarCode,
                    };

                    if (good.DefectId.HasValue)
                    {
                        _receipt.IsDefect = true;
                        _receipt.DefectDate = good.Created;
                        _receipt.Description = good.Defect.Description;
                        _receipt.SerialNumber = good.Defect.SerialNumber;
                        _receipt.DefectPercentage = good.Defect.Damage.ToString();
                        _receipt.DefectLink = string.Join(",", files.Where(m => m.GoodId == good.Id && m.TypeId == hFileType.Id)
                                                                    .Select(m=>m.Path));
                    }
                    boxReceipts.Add(_receipt);
                }
                _receipts.AddRange(boxReceipts);


                //Товары вне короба
                foreach (var good in goods.Where(m => !string.IsNullOrEmpty(m.GoodArticle))
                                          .Where(m => !m.BoxId.HasValue || m.BoxId == 0))
                {
                    var _docDatas = docdatas.Where(m => m.Article == good.GoodArticle &&
                                                              m.PlanNum == item.PlanNum &&
                                                              m.Barcode == "0");

                    //Пропускаем где по одному Article несколько записей
                    var length = _docDatas.Count();
                    if (length > 1) continue;

                    //Если пусто выбираем любой РОТ
                    var _docData = _docDatas.FirstOrDefault();
                    if (_docData == null) _docData = docdatas.FirstOrDefault();
                    if (_docData == null) continue;

                    var _receipt = new ReceiptModel
                    {
                        TaskId = item.Id,
                        PlanNum = _docData.PlanNum,
                        NumberDoc = _docData.NumberDoc,
                        DateDoc = _docData.DateDoc.Value,
                        Location = _docData.LocationGuid,
                        DateBeginLoad = item.CreateDateTime,
                        DateEndLoad = item.EndDateTime ?? DateTime.Now,
                        Article = good.GoodArticle,
                        Quantity = good.CountQty,
                        GoodBarcode = good.BarCode,
                        Barcode = "0" // added new value
                    };

                    if (good.DefectId.HasValue)
                    {
                        _receipt.IsDefect = true;
                        _receipt.DefectDate = good.Created;
                        _receipt.Description = good.Defect.Description;
                        _receipt.SerialNumber = good.Defect.SerialNumber;
                        _receipt.DefectPercentage = good.Defect.Damage.ToString();
                        _receipt.DefectLink = string.Join(",", files.Where(m => m.GoodId == good.Id && m.TypeId == hFileType.Id)
                                                                    .Select(m => m.Path));
                    }

                    notBoxReceipts.Add(_receipt);
                }
                _receipts.AddRange(notBoxReceipts);

                //Товар из 1с
                foreach (var good in docdatas.Where(m => !_receipts.Any(r => r.Article == m.Article))
                                            .GroupBy(m => m.Article))
                {

                    if (good.Count() > 1) continue;

                    var _docData = good.FirstOrDefault();
                    var _receipt = new ReceiptModel
                    {
                        TaskId = item.Id,
                        PlanNum = _docData.PlanNum,
                        NumberDoc = _docData.NumberDoc,
                        DateDoc = _docData.DateDoc.Value,
                        Location = _docData.LocationGuid,
                        DateBeginLoad = item.CreateDateTime,
                        DateEndLoad = item.EndDateTime ?? DateTime.Now,
                        Article = _docData.Article,
                        Quantity = 0,
                    };
                    docReceipts.Add(_receipt);
                }
                _receipts.AddRange(docReceipts);

                //Несколько товаров по Article
                foreach (var good in docdatas.GroupBy(m => m.Article).Where(m => m.Count() > 1))
                {
                    var article = good.FirstOrDefault().Article;
                    var _good = goods.FirstOrDefault(m => m.GoodArticle == article);

                    var _docDatas = good.Select(m => new { m.PlanNum, m.NumberDoc, m.DateDoc, m.LocationGuid, m.Article, m.Quantity, m.Barcode });

                    if (_good == null)
                    {
                        foreach (var _docData in _docDatas)
                        {
                            var _receipt = new ReceiptModel
                            {
                                TaskId = item.Id,
                                PlanNum = _docData.PlanNum,
                                NumberDoc = _docData.NumberDoc,
                                DateDoc = _docData.DateDoc.Value,
                                Location = _docData.LocationGuid,
                                DateBeginLoad = item.CreateDateTime,
                                DateEndLoad = item.EndDateTime ?? DateTime.Now,
                                Article = _docData.Article,
                                Quantity = 0,
                            };
                            rotReceipts.Add(_receipt);
                        }
                    }
                    else
                    {
                        var box = goods.FirstOrDefault(m => m.Id == _good.BoxId);

                        //  var quantity = _good.CountQty; // -- old value
                        var quantity = goods.Where(x => x.GoodArticle == _good.GoodArticle).GroupBy(x => new { x.GoodArticle })
                        .Select(g => g.Sum(x => x.CountQty)).FirstOrDefault();

                        foreach (var _docData in _docDatas)
                        {
                            var _receipt = new ReceiptModel
                            {
                                TaskId = item.Id,
                                PlanNum = _docData.PlanNum,
                                NumberDoc = _docData.NumberDoc,
                                DateDoc = _docData.DateDoc.Value,
                                Location = _docData.LocationGuid,
                                DateBeginLoad = item.CreateDateTime,
                                DateEndLoad = item.EndDateTime ?? DateTime.Now,
                                Article = _docData.Article,
                            };

                            if (_good.DefectId.HasValue)
                            {
                                _receipt.IsDefect = true;
                                _receipt.DefectDate = _good.Created;
                                _receipt.Description = _good.Defect.Description;
                                _receipt.SerialNumber = _good.Defect.SerialNumber;
                                _receipt.DefectPercentage = _good.Defect.Damage.ToString();
                                _receipt.DefectLink = string.Join(",", files.Where(m => m.GoodId == _good.Id && m.TypeId == hFileType.Id)
                                                                    .Select(m => m.Path));
                            }

                            if (quantity <= 0)
                            {
                                _receipt.Quantity = 0;
                            }
                            else
                            {
                                _receipt.Quantity = quantity >= _docData.Quantity ? _docData.Quantity : quantity;
                                _receipt.GoodBarcode = quantity >= _docData.Quantity ? _good.BarCode : "";
                                _receipt.Barcode = (_docData.Barcode != null ) ? _docData.Barcode : "0";
                                //_receipt.Barcode = box != null ? box.BarCode : ""; // old.value

                                quantity = quantity - _docData.Quantity;
                            }

                            rotReceipts.Add(_receipt);
                            //-------------------------------------------

                        }
                    }
                }
                _receipts.AddRange(rotReceipts);

                receipts.AddRange(_receipts);
            }

            //var rec1 = boxReceipts.Where(x => x.Article == "461514");
            //var rec2 = notBoxReceipts.Where(x => x.Article == "461514");
            //var rec3 = docReceipts.Where(x => x.Article == "461514");
            //var rec4 = rotReceipts.Where(x => x.Article == "461514");
            
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

        public List<Scaner_1cDocDataModel> DocDataModels(Data1cQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));

            var query = _mapper.Map<Data1cQuery>(queryModel);
            var entity = _data1CRepository.DocDataByPlanNum(query);

            return _mapper.Map<List<Scaner_1cDocDataModel>>(entity);
        }

        public List<LogsModel> LogsByTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var entity = _taskRepository.LogsByTask(query);
            return _mapper.Map<List<LogsModel>>(entity);
        }

        public string GetAddressLocationByTask(int taskId)
        {
            var result = _taskRepository.GetAddressLocationByTask(taskId);
            return result;
        }
    }
}
