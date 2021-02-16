using Business.Manager;
using System;
using System.Web.Http;
using static Business.Models.Dictionary.StandartDictionaries;

namespace ScanerApi.Controllers
{
    [RoutePrefix("api/Dictionary")]
    public class DictionaryController : ApiController
    {
        [HttpPost]
        [ActionName("GetDamagePercent")]
        [Route("GetDamagePercent")]
        public IHttpActionResult GetDamagePercent()
        {
            try
            {
                return Ok(new { success = true, data = CacheDictionaryManager.GetDictionaryShort<hDamagePercent>() });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }
    }
}
