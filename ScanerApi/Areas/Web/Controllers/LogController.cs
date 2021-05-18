using Business.Manager;
using Business.QueryModels.Data1c;
using Business.QueryModels.Good;
using Business.QueryModels.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScanerApi.Areas.Web.Controllers
{
    public class LogController : Controller
    {
        private readonly TaskManager _taskManager;
        private readonly GoodManager _goodManager;

        public LogController(TaskManager taskManager, GoodManager goodManager)
        {
            _taskManager = taskManager;
            _goodManager = goodManager;
        }

        // GET: Web/Log
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Search(string planNum)
        {
            var task = _taskManager.GetTaskById(new TaskQueryModel { PlanNum = planNum });
            var logs = _taskManager.LogsByTask(new TaskQueryModel { TaskId = task.Id });
            ViewBag.Task = task;
            return PartialView("Search", logs);
        }

        public ActionResult ScanerGoods(int taskId)
        {
            var data = _goodManager.GetGoods(new GoodQueryModel { TaskId = taskId });
            return View("ScanerGoods", data);
        }

        public ActionResult DocData(string planNum)
        {
            var data = _taskManager.DocDataModels(new Data1cQueryModel { PlanNum = planNum });
            return View("DocData", data);
        }
    }
}