using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Business.Validation
{
    public class ModelValidationResult
    {
        public List<ValidationResult> Errors { get; private set; }

        public string Message
        {
            get
            {
                return string.Join(Environment.NewLine, Errors.Select(x => x.ErrorMessage));
            }
        }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
        }

        public ModelValidationResult(List<ValidationResult> errors = null)
        {
            Errors = errors ?? new List<ValidationResult>();
        }

        public void Add(ValidationResult error)
        {
            Errors.Add(error);
        }

        public void AddRange(List<ValidationResult> errors)
        {
            Errors.AddRange(errors);
        }
    }
}
