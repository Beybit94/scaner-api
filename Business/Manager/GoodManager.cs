using AutoMapper;
using Business.Models;
using Business.QueryModels.Good;
using Data.Queries.Good;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Manager
{
    public class GoodManager
    {
        private readonly GoodRepository _goodRepository;
        private IMapper _mapper;

        public GoodManager(GoodRepository goodRepository, IMapper mappper)
        {
            _goodRepository = goodRepository;
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

            var entity = _goodRepository.GetGoodsByTask(query);
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

            //Проверка товара на количество
            var goods = _goodRepository.ExistGood(query);
            if (goods.Count() > 1) throw new Exception("Несколько товаров по штрихкоду");
            if (goods.Count() == 0) throw new Exception("Товар не найден");

            var saveQuery = query;
            saveQuery.GoodId = goods.FirstOrDefault().GoodId;
            saveQuery.GoodName = goods.FirstOrDefault().GoodName;
            saveQuery.GoodArticle = goods.FirstOrDefault().GoodArticle;
            saveQuery.CountQty = 1;
            _goodRepository.Save(saveQuery);
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
    }
}
