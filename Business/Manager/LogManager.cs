using AutoMapper;
using Business.Models;
using Business.QueryModels.Logs;
using Data.Model;
using Data.Queries.Logs;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Manager
{
    public class LogManager
    {
        private readonly LogRepository _logRepository;
        private IMapper _mapper;

        public LogManager(LogRepository logRepository, IMapper mappper)
        {
            _logRepository = logRepository;
            _mapper = mappper;
        }

        public List<LogsModel> LogsByTask(LogsListQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<LogsListQuery>(queryModel);

            var entity = _logRepository.List(query);
            return _mapper.Map<List<LogsModel>>(entity);
        }

        public void Insert(LogsModel model)
        {
            var entity = _mapper.Map<Logs>(model);
            _logRepository.Insert(entity);
        }
    }
}
