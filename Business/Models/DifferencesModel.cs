using Business.Models.Base;
using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class DifferencesModel:Model
    {
        public string NumberDoc { get; set; }
        public int GoodId { get; set; }
        public string GoodArticle { get; set; }
        public string GoodName { get; set; }
        public int Quantity { get; set; }
        public int CountQty { get; set; }
        public int ExcessQty { get; set; }

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<DifferencesModel>().Validate(this);
            return result;
        }
    }
}
