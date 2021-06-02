using AutoMapper;
using Business.Models;
using Business.QueryModels.Good;
using Data.Queries.Good;
using Data.Queries.Task;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static Business.Models.Dictionary.StandartDictionaries;

namespace Business.Manager
{
    public class GoodManager
    {
        private readonly GoodRepository _goodRepository;
        private readonly TaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GoodManager(GoodRepository goodRepository, TaskRepository taskRepository, IMapper mappper)
        {
            _goodRepository = goodRepository;
            _taskRepository = taskRepository;
            _mapper = mappper;
        }

        public List<GoodsModel> GetGoodWithBarcode(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            var entity = _goodRepository.GetGoodWithBarcode(query);
            return _mapper.Map<List<GoodsModel>>(entity);
        }


        public List<GoodsModel> GetGoods(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            var entity = _goodRepository.GetGoods(query);
            return _mapper.Map<List<GoodsModel>>(entity);
        }

        public List<GoodsModel> GetGoodsByTask(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            var StartStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "Start");
            var task = _taskRepository.GetTaskById(new TaskQuery { TaskId = query.TaskId });

            var entity = task.StatusId == StartStatus.Id ? _goodRepository.GetGoodsByTask(query)
                                                         : _goodRepository.GetBoxesByTask(query);
            return _mapper.Map<List<GoodsModel>>(entity);
        }

        public List<GoodsModel> GetGoodsByBox(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            var entity = _goodRepository.GetGoodsByBox(query);
            return _mapper.Map<List<GoodsModel>>(entity);
        }

        public List<GoodsModel> GetGoodsByFilter(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            var entity = _goodRepository.GetGoodsByFilter(query);
            return _mapper.Map<List<GoodsModel>>(entity);
        }

        public void Save(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            query.GoodArticle = !string.IsNullOrEmpty(query.BarCode) && !string.IsNullOrWhiteSpace(query.BarCode) ?
                                "" : query.GoodArticle;
            //Проверка товара на количество
            var goods = _goodRepository.ExistGood(query);
            if (goods.Count() > 1) throw new Exception("Несколько товаров по штрихкоду");
            if (goods.Count() == 0)
            {
                if (!string.IsNullOrEmpty(query.BarCode) && query.BarCode.StartsWith("C"))
                {
                    throw new Exception("Короб по текущей задаче не найден");
                }
             
             throw new Exception("Товар не найден");
            }
                
            //if (goods.FirstOrDefault().GoodId == 0 && query.BoxId > 0) throw new Exception("Запрет короб внутри короба");

            query.GoodId = goods.FirstOrDefault().GoodId;
            query.GoodName = goods.FirstOrDefault().GoodName;
            query.GoodArticle = goods.FirstOrDefault().GoodArticle;
            query.CountQty = 1;

            _goodRepository.Save(query);
        }

        public void Update(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            _goodRepository.Update(query);
        }

        public void Delete(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            _goodRepository.Delete(query);
        }

        public void Defect(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            _goodRepository.SaveDefect(query);

            if (!string.IsNullOrEmpty(query.Path))
            {
                var taskQuery = new TaskQuery { TaskId = query.TaskId, Path = query.Path, GoodId = query.Id };
                var hFileType = CacheDictionaryManager.GetDictionaryShort<hFileType>().FirstOrDefault(d => d.Code == "Defect_Photo");
                taskQuery.TypeId = hFileType.Id;
                taskQuery.TypeName = hFileType.Name;
                _taskRepository.SaveAct(taskQuery);
            }
        }
    }
}
