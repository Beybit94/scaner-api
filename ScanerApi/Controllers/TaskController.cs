﻿using Business.Manager;
using Business.QueryModels.Good;
using Business.QueryModels.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScanerApi.Controllers
{
    [RoutePrefix("api/Task")]
    public class TaskController : ApiController
    {
        private readonly TaskManager _taskManager;
        private readonly GoodManager _goodManager;
        public TaskController(TaskManager taskManager, GoodManager goodManager)
        {
            _taskManager = taskManager;
            _goodManager = goodManager;
        }

        [HttpPost]
        [ActionName("CreateTask")]
        [Route("CreateTask")]
        public IHttpActionResult CreateTask([FromBody] TaskQueryModel model)
        {
            try
            {
                var res = _taskManager.GetPlanNum(model);
                if (res == 0) throw new Exception("Документ с таким номером не найден");

                _taskManager.UnloadTask(model);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("GetActiveTask")]
        [Route("GetActiveTask")]
        public IHttpActionResult GetActiveTask([FromBody] TaskQueryModel model)
        {
            try
            {
                return Ok(new { success = true, data = _taskManager.GetActiveTask(model) });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("EndTask")]
        [Route("EndTask")]
        public IHttpActionResult EndTask([FromBody] TaskQueryModel model)
        {
            try
            {
                _taskManager.EndTask(model);
                return Ok(new { success = true } );
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("Differences")]
        [Route("Differences")]
        public IHttpActionResult Differences([FromBody] TaskQueryModel model)
        {
            try
            {
                return Ok(new { success = true, data = _taskManager.Differences(model) });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }
    }
}
