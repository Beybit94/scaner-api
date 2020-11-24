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
        public DateTime? DateDoc { get; set; }

        public string NumberDoc { get; set; }

        public List<ReceiptGoodModel> ReceiptGoods = new List<ReceiptGoodModel>();
    }
    public class ReceiptGoodModel
    {
        public string Article { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
    }
}
