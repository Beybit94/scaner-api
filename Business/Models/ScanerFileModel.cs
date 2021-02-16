using Business.Models.Base;
using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class ScanerFileModel : Model
    {
        public int TaskId { get; set; }
        public int? GoodId { get; set; }
        public string Path { get; set; }
        public int TypeId { get; set; }

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<ScanerFileModel>().Validate(this);
            return result;
        }
    }
}
