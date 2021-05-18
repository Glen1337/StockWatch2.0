using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace DegenApp.Attributes
{
    public class SecurityTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (SecurityTypes.securityTypes.Contains(value?.ToString().ToLower()) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Invalid Security Type for Holding: { value?.ToString()}";

            return new ValidationResult(msg);
        }
    }
}
