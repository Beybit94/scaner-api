using Business.Models.Base;
using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class Scaner_1cDocDataModel : Model
    {
        public string PlanNum { get; set; }
        public DateTime DateDoc { get; set; }
        public string GUID_PlanWMSNumber { get; set; }
        public string NumberDoc { get; set; }
        public string TypeDoc { get; set; }
        public string Article { get; set; }
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public string LocationGuid { get; set; }
        public string GoodArticle { get; set; }
        public int CountQty { get; set; }

        public override ModelValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}
