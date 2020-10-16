using Business.Validation;
using ScanerApi.Business.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.QueryModels.Data1c
{
    public class Data1cQueryModel : QueryModel
    {
        public int UserId { get; set; }
        public string PlanNum { get; set; }
        public int TaskId { get; set; }
        public string Planguid { get; set; }
        public DateTime? DateDoc { get; set; }
        public string NumberDoc { get; set; }
        public string TypeDoc { get; set; }
        public string Article { get; set; }
        public string Barcode { get; set; }
        public decimal? Quantity { get; set; }

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<Data1cQueryModel>().Validate(this);
            return result;
        }
    }
}
