using Business.Manager;
using Business.QueryModels.Data1c;
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
        private readonly Data1cManager _data1CManager;

        public PdfController(Data1cManager data1CManager)
        {
            _data1CManager = data1CManager;
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

        public FileResult Index(Data1cQueryModel model)
        {
            var goods = _data1CManager.Differences(model);
            MemoryStream stream = new MemoryStream();
            string content = RenderRazorViewToString("Index", goods);
            
            PdfDocument pdf = PdfGenerator.GeneratePdf(content, PageSize.A4);
            pdf.Save(stream, false);

            //return Json(new { success = true, data = content }, JsonRequestBehavior.AllowGet);
            return File(stream.ToArray(), "application/pdf");
        }
    }
}