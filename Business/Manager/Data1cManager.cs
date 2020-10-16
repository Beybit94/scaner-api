using AutoMapper;
using Business.Models;
using Business.QueryModels.Data1c;
using Data.Queries.Data1c;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Manager
{
    public class Data1cManager
    {
        private readonly Data1cRepository _data1CRepository;
        private IMapper _mapper;

        public Data1cManager(Data1cRepository data1CRepository, IMapper mappper)
        {
            _data1CRepository = data1CRepository;
            _mapper = mappper;
        }

        public List<DifferencesModel> Differences(Data1cQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<Data1cQuery>(queryModel);

            var entity = _data1CRepository.Differences(query);
            return _mapper.Map<List<DifferencesModel>>(entity);
        }

        public void SaveDataFrom1c(Data1cQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<Data1cQuery>(queryModel);

            _data1CRepository.SaveDataFrom1c(query);
        }
    }
}
