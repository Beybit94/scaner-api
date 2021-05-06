using Business.Manager;
using Business.QueryModels.Users;
using System;
using System.Web.Http;

namespace ScanerApi.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly UsersManager _usersManager;
        public UserController(UsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        [HttpGet]
        [ActionName("Test")]
        [Route("Test")]
        public IHttpActionResult Test()
        {
            var model = new UsersQueryModel() { Login = "AAbatova", Password = "Qwerty123" };
            var _data = _usersManager.Find(model);
            return Ok(new { success = true, data = _data });
        }

        [HttpPost]
        [ActionName("FindUser")]
        [Route("FindUser")]
        public IHttpActionResult FindUser([FromBody] UsersQueryModel model)
        {
            try
            {
                return Ok(new { success = true, data = _usersManager.Find(model) });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message, success = false });
            }
        }
    }
}
