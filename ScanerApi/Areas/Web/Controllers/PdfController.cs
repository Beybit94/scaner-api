using Business.Manager;
using Business.Models;
using Business.QueryModels.Data1c;
using Business.QueryModels.Good;
using Business.QueryModels.Task;
using PdfSharp;
using PdfSharp.Pdf;
using ScanerApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ScanerApi.Areas.Web.Controllers
{
    public class PdfController : Controller
    {
        private readonly TaskManager _taskManager;
        private readonly GoodManager _goodManager;

        public PdfController(TaskManager taskManager, GoodManager goodManager)
        {
            _taskManager = taskManager;
            _goodManager = goodManager;
        }

        public ActionResult Disqus()
        {
            return View();
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public FileResult Index(string PlanNum)
        {
            var task = _taskManager.GetTaskById(new TaskQueryModel { PlanNum = PlanNum });
            var goods = _goodManager.GetGoods(new GoodQueryModel { TaskId = task.Id });
            var differences = _taskManager.Differences(new TaskQueryModel { TaskId = task.Id, PlanNum = task.PlanNum });
            var test = differences.receipts.ToString();
            var report = new PdfViewModels { task = task, goods = goods, differences = differences };
            MemoryStream stream = new MemoryStream();
            string content = RenderRazorViewToString("Index", report);

            PdfDocument pdf = PdfGenerator.GeneratePdf(content, PageSize.A4);
            pdf.Save(stream, false);

            //return Json(new { success = true, data = content }, JsonRequestBehavior.AllowGet);
            return File(stream.ToArray(), "application/pdf");
        }

        public ActionResult Goods(GoodQueryModel model)
        {
            var data = _goodManager.GetGoodWithBarcode(model);
            return View("Goods", data);
        }

        /// <summary>
        /// Факт
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ScanerGoods(GoodQueryModel model)
        {
            var task = _taskManager.GetTaskById(new TaskQueryModel { PlanNum = model.PlanNum });
            model.TaskId = task.Id;
            var data = _goodManager.GetGoods(model);
            return View("ScanerGoods", data);
        }

        /// <summary>
        /// План
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult DocData(Data1cQueryModel model)
        {
            var data = _taskManager.DocDataModels(model);
            return View("DocData", data);
        }

    }
}