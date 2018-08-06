using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VBeat.Models.Validations
{
    public class PositiveNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int number = (int)value; 
            if (number>0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Only Positive Numbers Allowed");
            }
        }
    }
}
