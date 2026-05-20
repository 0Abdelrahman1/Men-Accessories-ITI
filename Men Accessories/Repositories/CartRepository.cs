using Men_Accessories.Contexts;
using Men_Accessories.Models;
using Microsoft.EntityFrameworkCore;

namespace Men_Accessories.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly MenAccessoriesContext _db;
        private readonly IProductRepository _productRepository;
        public CartRepository(MenAccessoriesContext db, IProductRepository productRepository)
        {
            _db = db;
            _productRepository = productRepository;
        }
        public void addToCart(int customerId, int productId, int quantity)
        {
            Cart? cart = getCartByCustomerId(customerId);
            Product? product = _productRepository.GetByKey(p => p.Id == productId);

            if (product != null)
            {
                if (cart == null)
                {
                    cart = new Cart { CustomerId = customerId };
                    _db.Carts.Add(cart);
                    _db.SaveChanges();
                }

                var existingCartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        UnitPrice = product.Price
                    });
                }

                _db.SaveChanges();
            }
        }

        public void clearCart(int customerId)
        {
            Cart? cart = getCartByCustomerId(customerId);
            if (cart is not null)
            {
                cart.CartItems.Clear();
                _db.SaveChanges();
            }
        }

        public Cart? getCartByCustomerId(int customerId)
        {
            return _db.Carts
              .Include(c => c.CartItems)
              .ThenInclude(i => i.Product) 
              .FirstOrDefault(c => c.CustomerId == customerId);
        }

        public void updateCart(Cart cart)
        {
            _db.Carts.Update(cart);
            _db.SaveChanges();
        }

        public void UpdateQuantity(int customerId, int productId, int newQuantity)
        {
            var cart = getCartByCustomerId(customerId);
            if (cart != null)
            {
                var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    if (newQuantity > 0)
                    {
                        item.Quantity = newQuantity;
                        _db.SaveChanges();
                    }
                }
            }
        }

        public void RemoveFromCart(int customerId, int productId)
        {
            var cart = getCartByCustomerId(customerId);
            if (cart != null)
            {
                var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
                if (item != null)
                {
                    cart.CartItems.Remove(item);
                    _db.SaveChanges();
                }
            }
        }
    }
}
