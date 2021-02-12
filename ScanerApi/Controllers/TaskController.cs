using Business.Manager;
using Business.QueryModels.Data1c;
using Business.QueryModels.Task;
using ScanerApi.Utils;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScanerApi.Controllers
{
    [RoutePrefix("api/Task")]
    public class TaskController : ApiController
    {
        private readonly TaskManager _taskManager;
        private readonly FileManager _fileManager;
        public TaskController(TaskManager taskManager, FileManager fileManager)
        {
            _taskManager = taskManager;
            _fileManager = fileManager;
        }

        [HttpPost]
        [ActionName("CreateTask")]
        [Route("CreateTask")]
        public IHttpActionResult CreateTask([FromBody] TaskQueryModel model)
        {
            try
            {
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
                return Ok(new { success = true, data = _taskManager.GetTaskById(model) });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("CloseTask")]
        [Route("CloseTask")]
        public IHttpActionResult CloseTask([FromBody] TaskQueryModel model)
        {
            try
            {
                _taskManager.CloseTask(model);
                return Ok(new { success = true });
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

        [HttpPost]
        [ActionName("UploadPhoto")]
        [Route("UploadPhoto")]
        public async Task<IHttpActionResult> UploadPhotoAsync()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new Exception(BadRequest().ToString());
            }

            try
            {
                var provider = new InMemoryMultipartFormDataStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                var query = provider.FormDataToTaskQuery();
                var path = "";
                Task fileResult = Task.Run(async () =>
                {
                    foreach (var file in provider.Files)
                    {
                        var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                        byte[] fileArray = await file.ReadAsByteArrayAsync();
                        path = _fileManager.UploadFile(fileArray, filename);
                    }
                });

                await fileResult.ContinueWith(t =>
                {
                    query.Path = path;
                    _taskManager.SaveAct(query);
                });
                fileResult.Wait();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }
    }
}
