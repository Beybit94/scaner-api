using Business.Validation;
using ScanerApi.Business.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.QueryModels.Good
{
    public class GoodQueryModel : QueryModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int? BoxId { get; set; }
        public string BarCode { get; set; }
        public string PlanNum { get; set; }
        public string GoodArticle { get; set; }
        public int CountQty { get; set; }

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<GoodQueryModel>().Validate(this);
            return result;
        }
    }
}
