using Business.Manager;
using Business.QueryModels.Good;
using ScanerApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScanerApi.Controllers
{
    [RoutePrefix("api/Good")]
    public class GoodController : ApiController
    {
        private readonly GoodManager _goodManager;
        private readonly FileManager _fileManager;
        public GoodController(GoodManager goodManager, FileManager fileManager)
        {
            _goodManager = goodManager;
            _fileManager = fileManager;
        }


        [HttpPost]
        [ActionName("GetGoodsByTask")]
        [Route("GetGoodsByTask")]
        public IHttpActionResult GetGoodsByTask([FromBody] GoodQueryModel model)
        {
            try
            {
                return Ok(new { success = true, data = _goodManager.GetGoodsByTask(model) });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("GetGoodsByBox")]
        [Route("GetGoodsByBox")]
        public IHttpActionResult GetGoodsByBox([FromBody] GoodQueryModel model)
        {
            try
            {
                return Ok(new { success = true, data = _goodManager.GetGoodsByBox(model) });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("GetGoodsByFilter")]
        [Route("GetGoodsByFilter")]
        public IHttpActionResult GetGoodsByFilter([FromBody] GoodQueryModel model)
        {
            try
            {
                return Ok(new { success = true, data = _goodManager.GetGoodsByFilter(model) });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("Create")]
        [Route("Create")]
        public IHttpActionResult Create([FromBody] GoodQueryModel model)
        {
            try
            {
                _goodManager.Save(model);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("Update")]
        [Route("Update")]
        public IHttpActionResult Update([FromBody] GoodQueryModel model)
        {
            try
            {
                _goodManager.Update(model);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [Route("Delete")]
        public IHttpActionResult Delete([FromBody] GoodQueryModel model)
        {
            try
            {
                _goodManager.Delete(model);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }

        [HttpPost]
        [ActionName("Defect")]
        [Route("Defect")]
        public async Task<IHttpActionResult> Defect()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new Exception(BadRequest().ToString());
            }

            try
            {
                var provider = new InMemoryMultipartFormDataStreamProvider();

                await Request.Content.ReadAsMultipartAsync(provider);

                var path = "";
                foreach (var file in provider.Files)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    byte[] fileArray = await file.ReadAsByteArrayAsync();
                    string.Concat(path, "," + _fileManager.UploadFile(fileArray, filename));
                }

                var query = provider.FormDataToGoodQuery();
                query.Path = path;
                _goodManager.Defect(query);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }
    }
}