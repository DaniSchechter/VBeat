using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.Validations
{
    public class FutureDateAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;
            if (DateTime.Now.CompareTo(date) <= 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Only Future Date is Valid");
            }
        }
    }
}
