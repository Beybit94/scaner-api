using Business.Manager;
using Business.QueryModels.Task;
using PdfSharp;
using PdfSharp.Pdf;
using System.IO;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ScanerApi.Areas.Web.Controllers
{
    public class PdfController : Controller
    {
        private readonly TaskManager _taskManager;
        public PdfController(TaskManager taskManager)
        {
            _taskManager = taskManager;
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

        public FileResult Index(TaskQueryModel model)
        {
            var goods = _taskManager.Differences(model);
            MemoryStream stream = new MemoryStream();
            string content = RenderRazorViewToString("Index", goods);
            
            PdfDocument pdf = PdfGenerator.GeneratePdf(content, PageSize.A4);
            pdf.Save(stream, false);

            //return Json(new { success = true, data = content }, JsonRequestBehavior.AllowGet);
            return File(stream.ToArray(), "application/pdf");
        }
    }
}