using AutoMapper;
using Business.Models;
using Business.QueryModels.Task;
using Data.Queries.Task;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Manager
{
    public class TaskManager
    {
        private readonly TaskRepository _taskRepository;
        private IMapper _mapper;

        public TaskManager(TaskRepository taskRepository, IMapper mappper)
        {
            _taskRepository = taskRepository;
            _mapper = mappper;
        }

        public int GetPlanNum(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var entity = _taskRepository.GetPlanNum(query);
            return entity;
        }

        public void UnloadTask(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            _taskRepository.UnloadTask(query);
        }

        public TasksModel GetActiveTask(TaskQueryModel queryModel)
        {
            if(queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var entity = _taskRepository.GetActiveTask(query);
            return _mapper.Map<TasksModel>(entity);
        }

        public TasksModel GetTaskById(TaskQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));
            var query = _mapper.Map<TaskQuery>(queryModel);

            var entity = _taskRepository.GetTaskById(query);
            return _mapper.Map<TasksModel>(entity); ;
        }
    }
}
