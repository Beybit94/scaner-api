﻿using System;
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

        public string GoodName { get; set; }
        public int CountQty { get; set; }
        public string Article { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public string GoodBarcode { get; set; }

        public bool IsDefect = false;
        public DateTime DefectDate { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string DefectPercentage { get; set; }
        public string DefectLink { get; set; }
    }
}
