﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Receipt
    {
        public int TaskId { get; set; }
        public string PlanNum { get; set; }
        public DateTime? DateDoc { get; set; }

        public string NumberDoc { get; set; }

        public List<ReceiptGood> ReceiptGoods = new List<ReceiptGood>();
    }
    public class ReceiptGood
    {
        public string Article { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
    }
}
