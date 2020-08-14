using System;
using System.ComponentModel.DataAnnotations;

namespace Business.Validation.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredObjectAttribute : ValidationAttribute
    {
        public RequiredObjectAttribute() : base(() => "")
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var stringValue = value as string;
            if (stringValue != null)
            {
                return stringValue.Trim().Length != 0;
            }

            return true;
        }
    }
}
