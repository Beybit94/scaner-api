using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Validation
{
    public class ModelValidator<T> where T : class
    {
        public ModelValidationResult Validate(T model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            var isValid = Validator.TryValidateObject(model, context, results, true);

            return new ModelValidationResult(results);
        }

    }
}
