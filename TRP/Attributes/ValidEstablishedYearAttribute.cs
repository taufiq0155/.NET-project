using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TRP.Attributes
{
    public class ValidEstablishedYearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int year)
            {
                int currentYear = DateTime.Now.Year;
                if (year < 1990 || year > currentYear)
                {
                    return new ValidationResult($"Established year must be between 1990 and {currentYear}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}