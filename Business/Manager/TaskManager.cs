using AutoMapper;
using Business.Models;
using Business.QueryModels.Task;
using Data.Queries.Data1c;
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

            query.PlanNum = task.PlanNum;

            var entity = _taskRepository.Differences(query);
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

        public List<ReceiptModel> PrepareDataTo1c(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var hTaskStatus = CacheDictionaryManager.GetDictionaryShort<hTaskStatus>().FirstOrDefault(d => d.Code == "End");
            var hProcessType = CacheDictionaryManager.GetDictionaryShort<hProcessType>().FirstOrDefault(d => d.Code == "SendTo1C");

            query.StatusId = hTaskStatus.Id;
            query.ProcessTypeId = hProcessType.Id;

            var tasks = _taskRepository.GetTasksByStatus(query);

            var data1cQuery = new Data1cQuery { };
            var receipts = new List<ReceiptModel>();
            foreach (var item in tasks)
            {
                data1cQuery.TaskId = item.Id;
                data1cQuery.PlanNum = item.PlanNum.Replace("\n", "").Replace("\r", "");
                var receipt = _data1CRepository.GetNumberDocs(data1cQuery);
                receipts.AddRange(_mapper.Map<List<ReceiptModel>>(receipt));
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
