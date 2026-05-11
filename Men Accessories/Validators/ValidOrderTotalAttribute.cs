using Men_Accessories.Models;
using System.ComponentModel.DataAnnotations;

namespace Men_Accessories.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidOrderTotalAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var order = validationContext.ObjectInstance as Order;
            if (order?.OrderItems == null || value is not decimal total)
                return ValidationResult.Success;

            var calculatedTotal = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
            if (total != calculatedTotal)
                return new ValidationResult(
                    $"Total amount must equal sum of order items: {calculatedTotal}");

            return ValidationResult.Success;
        }
    }
}
