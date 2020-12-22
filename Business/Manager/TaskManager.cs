﻿using AutoMapper;
using Business.Models;
using Business.QueryModels.Task;
using Data.Queries.Data1c;
using Data.Queries.Good;
using Data.Queries.Task;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public List<DifferencesModel> Differences(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));

            var query = _mapper.Map<TaskQuery>(queryModel);
            var task = _taskRepository.GetTaskById(query);

            var result = new List<DifferencesModel>();
            var goodQuery = new GoodQuery { TaskId = task.Id };
            var data1cQuery = new Data1cQuery { PlanNum = task.PlanNum };

            var goods = _goodRepository.GetGoods(goodQuery);
            var docdatas = _data1CRepository.DocDataByPlanNum(data1cQuery);

            //Товары в коробе
            foreach (var good in goods.Where(m=>!string.IsNullOrEmpty(m.GoodArticle))
                                      .Where(m => m.BoxId.HasValue && m.BoxId.Value != 0))
            {
                var box = goods.FirstOrDefault(m => m.Id == good.BoxId);
                var _docDatas = docdatas.Where(m => m.Article == good.GoodArticle &&
                                                          m.PlanNum == data1cQuery.PlanNum &&
                                                          m.Barcode == box.BarCode);

                //Пропускаем где по одному Article несколько записей
                var length = _docDatas.Count();
                if (length > 1) continue;

                //Если пусто выбираем любой РОТ
                var _docData = _docDatas.FirstOrDefault();
                if (_docData == null) _docData = docdatas.FirstOrDefault();

                var diff = new DifferencesModel
                {
                    NumberDoc = _docData.NumberDoc,
                    GoodId = good.GoodId,
                    GoodName = good.GoodName,
                    GoodArticle = good.GoodArticle,
                    CountQty = good.CountQty,
                    Quantity = _docData.Quantity,
                };
                result.Add(diff);
            }

            //Товары вне короба
            foreach (var good in goods.Where(m => !string.IsNullOrEmpty(m.GoodArticle))
                                      .Where(m => !m.BoxId.HasValue || m.BoxId == 0))
            {
                var _docDatas = docdatas.Where(m => m.Article == good.GoodArticle &&
                                                          m.PlanNum == data1cQuery.PlanNum &&
                                                          m.Barcode == "0");

                //Пропускаем где по одному Article несколько записей
                var length = _docDatas.Count();
                if (length > 1) continue;

                //Если пусто выбираем любой РОТ
                var _docData = _docDatas.FirstOrDefault();
                if (_docData == null) _docData = docdatas.FirstOrDefault();

                var diff = new DifferencesModel
                {
                    NumberDoc = _docData.NumberDoc,
                    GoodId = good.GoodId,
                    GoodName = good.GoodName,
                    GoodArticle = good.GoodArticle,
                    CountQty = good.CountQty,
                    Quantity = _docData.Quantity,
                };
                result.Add(diff);
            }

            //Товар из 1с
            foreach (var good in docdatas.Where(m => !result.Any(r => r.GoodArticle == m.Article))
                                        .GroupBy(m => m.Article))
            {
                if (good.Count() > 1) continue;

                var _docData = good.FirstOrDefault();
                goodQuery.GoodArticle = _docData.Article;
                var _good = _goodRepository.GetGoodsByFilter(goodQuery).FirstOrDefault();
                var diff = new DifferencesModel
                {
                    NumberDoc = _docData.NumberDoc,
                    GoodId = _good.GoodId,
                    GoodName = _good.GoodName,
                    GoodArticle = _good.GoodArticle,
                    CountQty = 0,
                    Quantity = _docData.Quantity,
                };
                result.Add(diff);
            }

            //Несколько товаров по Article
            foreach (var good in docdatas.GroupBy(m => m.Article).Where(m => m.Count() > 1))
            {
                var article = good.FirstOrDefault().Article;
                var _good = goods.FirstOrDefault(m => m.GoodArticle == article);

                var _docDatas = good.Select(m => new { m.PlanNum, m.NumberDoc, m.DateDoc, m.LocationGuid, m.Article, m.Quantity });

                var quantity = _good != null ? _good.CountQty : 0;
                foreach (var _docData in _docDatas)
                {
                    goodQuery.GoodArticle = _docData.Article;
                    var __good = _goodRepository.GetGoodsByFilter(goodQuery).FirstOrDefault();

                    var diff = new DifferencesModel
                    {
                        NumberDoc = _docData.NumberDoc,
                        GoodId = __good.GoodId,
                        GoodName = __good.GoodName,
                        GoodArticle = __good.GoodArticle,
                        Quantity = _docData.Quantity,
                    };

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

            return result.Where(m => m.CountQty != m.Quantity).ToList();
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

            var goodQuery = new GoodQuery { };
            var data1cQuery = new Data1cQuery { };
            var receipts = new List<ReceiptModel>();

            var boxReceipts = new List<ReceiptModel>();
            var notBoxReceipts = new List<ReceiptModel>();
            var docReceipts = new List<ReceiptModel>();
            var rotReceipts = new List<ReceiptModel>();

            foreach (var item in tasks)
            {
                var _receipts = new List<ReceiptModel>();

                goodQuery.TaskId = item.Id;
                goodQuery.PlanNum = item.PlanNum.Replace("\n", "").Replace("\r", "");
                var goods = _goodRepository.GetGoods(goodQuery);

                data1cQuery.PlanNum = item.PlanNum.Replace("\n", "").Replace("\r", "");
                var docdatas = _data1CRepository.DocDataByPlanNum(data1cQuery);

                //Товары в коробе
                foreach (var good in goods.Where(m => !string.IsNullOrEmpty(m.GoodArticle))
                                          .Where(m => m.BoxId.HasValue && m.BoxId.Value != 0))
                {
                    var box = goods.FirstOrDefault(m => m.Id == good.BoxId);
                    var _docDatas = docdatas.Where(m => m.Article == good.GoodArticle &&
                                                              m.PlanNum == data1cQuery.PlanNum &&
                                                              m.Barcode == box.BarCode);

                    //Пропускаем где по одному Article несколько записей
                    var length = _docDatas.Count();
                    if (length > 1) continue;

                    //Если пусто выбираем любой РОТ
                    var _docData = _docDatas.FirstOrDefault();
                    if (_docData == null) _docData = docdatas.FirstOrDefault();

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
                    boxReceipts.Add(_receipt);
                }
                _receipts.AddRange(boxReceipts);

                //Товары вне короба
                foreach (var good in goods.Where(m => !string.IsNullOrEmpty(m.GoodArticle))
                                          .Where(m => !m.BoxId.HasValue || m.BoxId == 0))
                {
                    var _docDatas = docdatas.Where(m => m.Article == good.GoodArticle &&
                                                              m.PlanNum == data1cQuery.PlanNum &&
                                                              m.Barcode == "0");

                    //Пропускаем где по одному Article несколько записей
                    var length = _docDatas.Count();
                    if (length > 1) continue;

                    //Если пусто выбираем любой РОТ
                    var _docData = _docDatas.FirstOrDefault();
                    if (_docData == null) _docData = docdatas.FirstOrDefault();

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
                    };
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

                    var _docDatas = good.Select(m => new { m.PlanNum, m.NumberDoc, m.DateDoc, m.LocationGuid, m.Article, m.Quantity });

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
                        var quantity = _good.CountQty;
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

                            if (quantity <= 0)
                            {
                                _receipt.Quantity = 0;
                            }
                            else
                            {
                                _receipt.Quantity = quantity >= _docData.Quantity ? _docData.Quantity : quantity;
                                _receipt.GoodBarcode = quantity >= _docData.Quantity ? _good.BarCode : "";
                                _receipt.Barcode = box != null ? box.BarCode : "";

                                quantity = quantity - _docData.Quantity;
                            }

                            rotReceipts.Add(_receipt);
                        }
                    }
                }
                _receipts.AddRange(rotReceipts);

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
