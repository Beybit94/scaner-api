using Business.Models.Base;
using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Dictionary
{
    public class BaseDictionary : BaseDictionaryShort
    {
        public string Code { get; set; }
    }

    public class BaseDictionaryShort : Model
    {
        public string Name { get; set; }
        public override ModelValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}
