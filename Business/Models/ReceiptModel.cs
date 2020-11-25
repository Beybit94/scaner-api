using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ReceiptModel
    {
        public int TaskId { get; set; }
        public string PlanNum { get; set; }
        public string NumberDoc { get; set; }
        public string Location { get; set; }

        public DateTime DateDoc = DateTime.Now;

        public DateTime DateBeginLoad = DateTime.Now;

        public DateTime DateEndLoad = DateTime.Now;

        public DateTime DateReceipt = DateTime.Now;

        public List<ReceiptGoodModel> ReceiptGoods = new List<ReceiptGoodModel>();
    }
    public class ReceiptGoodModel
    {
        public string Article { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public string GoodBarcode { get; set; }
    }
}
