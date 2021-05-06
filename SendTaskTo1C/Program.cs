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

        static async Task Main(string[] args)
        {

            Container = AutofacConfig.Register();
            var taskManager = Container.Resolve<TaskManager>();

            var query = new TaskQueryModel();
            //query.PlanNum = "0000121838_1";
            var items = taskManager.PrepareDataTo1c(query);

            //query.TaskId = 140;
            //query.PlanNum = "0000110175_4";
            //var difference = taskManager.Differences(query);

            if (!items.Any()) return;

            using (var acceptSend = new WebReference.WebСервис_Приемка_АРЕНА())
            {
                var data = items.GroupBy(m => m.NumberDoc).Select(m => new WebReference.Receipt
                {
                    GUID_Location = m.FirstOrDefault().Location,
                    GUID_WEB = m.FirstOrDefault().TaskId.ToString(),
                    GUID_Division = "A34D95B8-3BFF-11DF-9B61-001B78E2224A",
                    DateDoc = m.FirstOrDefault().DateDoc,
                    DateBeginLoad = m.FirstOrDefault().DateBeginLoad,
                    DateEndLoad = m.FirstOrDefault().DateEndLoad,
                    DateReceipt = m.FirstOrDefault().DateReceipt,
                    Rowpictures = "0",
                    TypeDoc = "РасходныйОрдерНаТовары",
                    NumberDoc = m.FirstOrDefault().NumberDoc
                }).ToArray();

                foreach (var item in data)
                {
                    item.ReceiptGood = items.Where(m => m.NumberDoc == item.NumberDoc).Select(g =>
                    new WebReference.ReceiptGood
                    {
                        Article = g.Article,
                        Quantity = g.Quantity,
                        Barcode = g.Barcode,
                        GoodBarcode = g.GoodBarcode,
                        //Defect = g.IsDefect,
                        //DefectDate = g.DefectDate,
                        //DefectDescription = g.Description,
                        //SerialNumber = g.SerialNumber,
                        //DefectPercentage = g.DefectPercentage,
                        //DefectLink = g.DefectLink
                    }).ToArray();
                }

                List<string> errors = new List<string>();
                query.Request = JsonConvert.SerializeObject(data);
                var arr = data.ToArray();
                try
                {
                    var resultSend = acceptSend.LoadReceipts_new(data);
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

                    if (errors.Count > 0)
                    {
                        var msgErr = "";
                        foreach (var err in errors)
                        {
                            msgErr = msgErr + err + "<br/>";
                        }
                        SendMsgToEmail("Сканер. Возвращаемые ошибки с 1С", msgErr);
                    }
                    
                }
                catch (Exception ex)
                {
                    SendMsgToEmail("Сканер. Ошибка при отправке данных в 1С", ex.Message + " " + ex.StackTrace);
                    throw ex;
                }


            }
        }

        /// <summary>
        /// Отправка уведомлении на почту
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        private static void SendMsgToEmail(string subject, string message)
        {
            try
            {
                var mailServerSenderAddress = System.Configuration.ConfigurationManager.AppSettings["MailServerSenderAddress"];
                System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress(mailServerSenderAddress, mailServerSenderAddress);
                
                var mailServerToAddress = System.Configuration.ConfigurationManager.AppSettings["MailServerToAddress"];
                System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(mailServerToAddress);
                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(from, to);
                m.Subject = subject;
                m.Body = message;
                m.IsBodyHtml = true;
                var smtpHost = System.Configuration.ConfigurationManager.AppSettings["MailServerHost"].ToString();
                var smtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MailServerPort"].ToString());

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpHost, smtpPort);

                var mailServerUser = System.Configuration.ConfigurationManager.AppSettings["MailServerUser"].ToString();
                var mailServerPassword = System.Configuration.ConfigurationManager.AppSettings["MailServerPassword"].ToString();
                smtp.Credentials = new System.Net.NetworkCredential(mailServerUser, mailServerPassword);
                // smtp.EnableSsl = false;
                smtp.Send(m); 
            }
            catch (Exception ex) { 

            }


           
        }

    }
}
