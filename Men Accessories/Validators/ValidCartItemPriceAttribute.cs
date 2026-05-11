using Men_Accessories.Models;
using System.ComponentModel.DataAnnotations;

namespace Men_Accessories.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidCartItemPriceAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var cartItem = validationContext.ObjectInstance as CartItem;
            if (cartItem?.Product == null || value is not decimal price)
                return ValidationResult.Success;

            if (price != cartItem.Product.Price)
                return new ValidationResult(
                    $"Unit price must match product price: {cartItem.Product.Price}");

            return ValidationResult.Success;
        }
    }
}
