using Business.Validation;
using Business.Validation.CustomAttributes;
using ScanerApi.Business.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.QueryModels.Users
{
    public class UsersQueryModel : QueryModel
    {
        [RequiredObject]
        public string Login { get; set; }

        [RequiredObject]
        public string Password { get; set; }

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<UsersQueryModel>().Validate(this);
            return result;
        }
    }
}
