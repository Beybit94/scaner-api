using Business.Manager;
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
        public TaskController(TaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        [HttpPost]
        [ActionName("GetPlanNum")]
        [Route("GetPlanNum")]
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
                var taskId = _taskManager.GetTaskId(model);

                var taskQueyModel = new TaskQueryModel { TaskId = taskId };
                return Ok(new { success = true, data = _taskManager.GetActiveTask(taskQueyModel) });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }
    }
}
