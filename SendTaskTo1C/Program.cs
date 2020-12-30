using Autofac;
using Business.Manager;
using Business.QueryModels.Task;
using Newtonsoft.Json;
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
            //query.PlanNum = "0000121838_1";
            var items = taskManager.PrepareDataTo1c(query);

            //var difference = taskManager.Differences(query);
            //query.Request = JsonConvert.SerializeObject(difference);

            if (!items.Any()) return;

            using (var acceptSend = new WebReference.WebСервис_Приемка_АРЕНА())
            {
                List<WebReference.Receipt> data = new List<WebReference.Receipt>();

                foreach (var item in items.GroupBy(m=>m.NumberDoc))
                {
                    var res = new WebReference.Receipt();
                    res.GUID_Division = "A34D95B8-3BFF-11DF-9B61-001B78E2224A";
                    res.DateBeginLoad = item.FirstOrDefault().DateBeginLoad;
                    res.DateDoc = item.FirstOrDefault().DateDoc;
                    res.DateEndLoad = item.FirstOrDefault().DateEndLoad;
                    res.DateReceipt = item.FirstOrDefault().DateReceipt;
                    res.GUID_Location = item.FirstOrDefault().Location;
                    res.GUID_WEB = item.FirstOrDefault().TaskId.ToString();
                    res.Rowpictures = "0";
                    res.NumberDoc = item.FirstOrDefault().NumberDoc;
                    res.TypeDoc = "РасходныйОрдерНаТовары";

                    var goods = new List<WebReference.ReceiptGood>();
                    foreach (var g in item)
                    {
                        var good = new WebReference.ReceiptGood();
                        good.Article = g.Article;
                        good.Quantity = g.Quantity;
                        good.Barcode = g.Barcode;
                        good.GoodBarcode = g.GoodBarcode;
                        goods.Add(good);
                    }

                    res.ReceiptGood = goods.ToArray();
                    data.Add(res);
                }

                List<string> errors = new List<string>();
                query.Request = JsonConvert.SerializeObject(data.ToArray());
                var resultSend = acceptSend.LoadReceipts_new(data.ToArray());
                foreach (var r in resultSend)
                {
                    foreach (var m in r.Messages)
                    {
                        errors.Add("CODE: " + m.Code + "; Message: " + m.Message1);
                    }
                }
                query.TaskId = items.FirstOrDefault().TaskId;
                query.Message = JsonConvert.SerializeObject(resultSend);
                query.Request = JsonConvert.SerializeObject(data.ToArray());
                taskManager.Set1cStatus(query);

                try
                {
                    
                }
                catch(Exception ex)
                {
                    throw ex;
                }

               
            }
        }
    }
}
