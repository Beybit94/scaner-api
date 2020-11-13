using Business.Models.Base;
using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class GoodsModel:Model
    {
        public int GoodId { get; set; }
        public int Count { get; set; }
        public string GoodName { get; set; }
        public string GoodArticle { get; set; }
        public string BarCode { get; set; }

        public int? BoxId { get; set; }

        public bool IsBox
        {
            get
            {
                return string.IsNullOrEmpty(GoodArticle) || GoodArticle.Equals("0") || GoodId == 0;
            }
        }

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<GoodsModel>().Validate(this);
            return result;
        }
    }
}
