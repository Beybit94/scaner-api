using AutoMapper;
using Business.Models;
using Business.QueryModels.Task;
using Data.Queries.Data1c;
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
        private readonly Data1cRepository _data1CRepository;
        private IMapper _mapper;

        public TaskManager(TaskRepository taskRepository, Data1cRepository data1CRepository, IMapper mappper)
        {
            _taskRepository = taskRepository;
            _data1CRepository = data1CRepository;
            _mapper = mappper;
        }

        public void UnloadTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var res = _taskRepository.GetPlanNum(query);
            if (res == 0) throw new Exception("Документ с таким номером не найден");

            var hTaskStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "Start");
            query.StatusId = hTaskStatus.Id;
            _taskRepository.UnloadTask(query);
        }

        public TasksModel GetActiveTask(TaskQueryModel queryModel)
        {
            if(queryModel == null) throw new ArgumentNullException(nameof(queryModel));
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
            var query = _mapper.Map<Data1cQuery>(queryModel);

            var entity = _data1CRepository.Differences(query);
            return _mapper.Map<List<DifferencesModel>>(entity);
        }

        public void SaveAct(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var hFileType = CacheDictionaryManager.GetDictionaryShort<hFileType>().FirstOrDefault(d => d.Code == "Act_Photo");
            query.TypeId = hFileType.Id;

            _taskRepository.SaveAct(query);
        }
    }
}
