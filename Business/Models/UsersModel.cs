using Business.Validation;
using Business.Models.Base;
using System;
using Business.Validation.CustomAttributes;

namespace Business.Models
{
    public class UsersModel : Model
    {
        public int UserDivisionId { get; set; }
        public string UserFirstName { get; set; }
        public string UserSecondName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserGuid { get; set; }

        public string UserFullName => string.Concat(UserSecondName, " ", UserFirstName, " ", UserMiddleName);

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<UsersModel>().Validate(this);
            return result;
        }
    }
}
