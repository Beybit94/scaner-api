using Autofac;
using Business.Manager;
using Business.QueryModels.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendTaskTo1C
{
    class Program
    {
        public static IContainer Container { get; set; }
        static void Main(string[] args)
        {
            Container = AutofacConfig.Register();
            var taskManager = Container.Resolve<TaskManager>();

            var query = new TaskQueryModel();
            var items = taskManager.GetDataTo1c(query);

            using (var acceptSend = new WebReference.WebСервис_Приемка_АРЕНА())
            {
                List<WebReference.Receipt> data = new List<WebReference.Receipt>();
                List<WebReference.ReceiptGood> goods = new List<WebReference.ReceiptGood>();
                List<WebReference.Receipt> datanew = new List<WebReference.Receipt>();

                foreach (var item in items.GroupBy(m=>m.NumberDoc))
                {
                    query.PlanNum = item.FirstOrDefault().PlanNum;
                    var docData = taskManager.GetPlanNum(query);
                    var DateDoc = item.FirstOrDefault().DateDoc.HasValue ? item.FirstOrDefault().DateDoc.Value : docData.DateDoc.Value;
                    var NumberDoc = string.IsNullOrEmpty(item.FirstOrDefault().NumberDoc) ? docData.NumberDoc : item.FirstOrDefault().NumberDoc;

                    var res = new WebReference.Receipt();
                    res.GUID_Division = "A34D95B8-3BFF-11DF-9B61-001B78E2224A";
                    res.DateBeginLoad = DateTime.Now;
                    res.DateDoc = DateDoc;
                    res.DateEndLoad = DateTime.Now;
                    res.DateReceipt = DateTime.Now;
                    res.GUID_Location = "10c95cb1-29c2-11e0-8806-001b78e2224a";
                    res.GUID_WEB = item.FirstOrDefault().TaskId.ToString();
                    res.Rowpictures = "0";
                    res.NumberDoc = NumberDoc;
                    res.TypeDoc = "РасходныйОрдерНаТовары";

                    foreach(var g in item)
                    {
                        var good = new WebReference.ReceiptGood();
                        good.Article = g.GoodArticle;
                        good.Quantity = g.CountQty;
                        good.Barcode = g.Barcode;
                        goods.Add(good);
                    }

                    res.ReceiptGood = goods.ToArray();
                    data.Add(res);
                }

                List<string> errors = new List<string>();
                var resultSend = acceptSend.LoadReceipts_new(data.ToArray());
                foreach (var r in resultSend)
                {
                    foreach (var m in r.Messages)
                    {
                        errors.Add("CODE: " + m.Code + "; Message: " + m.Message1);
                    }
                }

                query.TaskId = items.FirstOrDefault().TaskId;
                taskManager.SetDataTo1c(query);
            }
        }
    }
}
