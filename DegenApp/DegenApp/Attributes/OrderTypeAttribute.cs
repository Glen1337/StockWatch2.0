using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DegenApp.Attributes
{
    public class OrderTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (OrderTypes.orderTypes.Contains(value?.ToString().ToLower()) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Invalid Security Type for Holding: { value?.ToString()}";

            return new ValidationResult(msg);
        }
    }
}
