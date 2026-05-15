using Men_Accessories.Models;

namespace Men_Accessories.Repositories
{
    public interface ICartRepository
    {
        void addToCart(int customerId, int productId, int quantity);
        void updateCart(Cart cart);
        void clearCart(int customerId);
        Cart? getCartByCustomerId(int customerId);

        void RemoveFromCart(int customerId, int productId);

        void UpdateQuantity(int customerId, int productId, int newQuantity);
    }
}
