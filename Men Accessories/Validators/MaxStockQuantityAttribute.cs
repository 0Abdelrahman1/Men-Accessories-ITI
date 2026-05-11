using Men_Accessories.Models;
using System.ComponentModel.DataAnnotations;

namespace Men_Accessories.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxStockQuantityAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var cartItem = validationContext.ObjectInstance as CartItem;
            if (cartItem?.Product == null || value is not int quantity)
                return ValidationResult.Success;

            if (quantity > cartItem.Product.StockQuantity)
                return new ValidationResult(
                    $"Only {cartItem.Product.StockQuantity} units available in stock");

            return ValidationResult.Success;
        }
    }
}
