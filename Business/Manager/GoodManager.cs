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

        public List<GoodsModel> Get(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            var entity = _goodRepository.Get(query);
            return _mapper.Map<List<GoodsModel>>(entity);
        }

        public void UnloadGood(GoodQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<GoodQuery>(queryModel);

            _goodRepository.UnloadGood(query);
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
