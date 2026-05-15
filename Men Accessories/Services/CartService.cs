using Men_Accessories.ExtensionMethods;
using Men_Accessories.Models;
using Men_Accessories.Repositories;
using Men_Accessories.ViewModels.CartRelated;

namespace Men_Accessories.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        public CartService(ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }
        public void addToCart(int customerId, int productId, int quantity)
        {
            _cartRepository.addToCart(customerId, productId, quantity);
        }

        public void Checkout(int customerId, string stripeSessionId)
        {
            Cart? cart = _cartRepository.getCartByCustomerId(customerId);
            if (cart is not null && cart.CartItems.Count > 0)
            {
                _orderRepository.CreateOrderFromCart(cart, stripeSessionId);
                _cartRepository.clearCart(customerId);
            }
        }
        public void UpdateQuantity(int customerId, int productId, int newQuantity)
        {
            _cartRepository.UpdateQuantity(customerId, productId, newQuantity);
        }
        public void clearCart(int customerId)
        {
            _cartRepository.clearCart(customerId);
        }

        public CartModelView? getCartByCustomerId(int customerId)
        {
            return _cartRepository.getCartByCustomerId(customerId)?.ToCartModelView();
        }

        public void updateCart(CartModelView cart)
        {
            _cartRepository.updateCart(cart.ToCart());
        }

        public void RemoveFromCart(int customerId, int productId)
        {
            _cartRepository.RemoveFromCart(customerId, productId);
        }
    }
}
