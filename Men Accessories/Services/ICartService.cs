using Men_Accessories.Models;
using Men_Accessories.ViewModels.CartRelated;

namespace Men_Accessories.Services
{
    public interface ICartService
    {
        void addToCart(int customerId, int productId, int quantity);
        void updateCart(CartModelView cart);
        void clearCart(int customerId);
        CartModelView? getCartByCustomerId(int customerId);
        void Checkout(int customerId, string stripeSessionId);
        void RemoveFromCart(int customerId, int productId);
    }
}
