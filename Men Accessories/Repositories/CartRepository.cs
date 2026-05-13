using Men_Accessories.Contexts;
using Men_Accessories.Models;

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
            Product? product = _productRepository.GetByKey(productId, p => p.Id);
            if (product is not null && cart is not null)
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                });
                _db.SaveChanges();
            }
        }

        public void clearCart(int customerId)
        {
            Cart? cart = getCartByCustomerId(customerId);
            if(cart is not null)
            {
                cart.CartItems.Clear();
                _db.SaveChanges();
            }
        }

        public Cart? getCartByCustomerId(int customerId)
        {
            return _db.Carts.FirstOrDefault(c => c.CustomerId == customerId);
        }

       public void updateCart(Cart cart)
        {
            _db.Carts.Update(cart);
            _db.SaveChanges();
        }
    }
}
