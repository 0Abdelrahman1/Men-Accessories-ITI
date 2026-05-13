using Men_Accessories.Models;
using Men_Accessories.ViewModels.Cart;

namespace Men_Accessories.ExtensionMethods
{
    public static class CartExtensionMethods
    {
        public static decimal CalculateTotalAmount(this Cart cart)
        {
            return cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);
        }

        public static CartModelView ToCartModelView(this Cart cart)
        {
            return new CartModelView
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                Customer = cart.Customer,
                CartItems = cart.CartItems
            };
        }
    }
}
