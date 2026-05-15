using Men_Accessories.Contexts;
using Men_Accessories.ExtensionMethods;
using Men_Accessories.Models;

namespace Men_Accessories.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MenAccessoriesContext _db;
        public OrderRepository(MenAccessoriesContext db)
        {
            _db = db;
        }
        public void CreateOrder(Order order)
        {
            _db.Orders.Add(order);
            foreach (var orderItem in order.OrderItems)
            {
                var product = _context.Products.Find(orderItem.ProductId);

                if (product != null)
                {
                    product.StockQuantity -= orderItem.Quantity;

                    if (product.StockQuantity < 0)
                    {
                        product.StockQuantity = 0;
                    }

                    _context.Products.Update(product);
                } 
            }
            _db.SaveChanges();
        }

        public void CreateOrderFromCart(Cart cart, string stripeSessionId)
        {
            Order order = new Order()
            {
                CustomerId = cart.CustomerId,
                CreatedAt = DateTime.Now,
                Customer = cart.Customer,
                TotalAmount = cart.CalculateTotalAmount(), 
                PaymentStatus = "Paid", 
                OrderStatus = "Processing",
                StripeSessionId = stripeSessionId,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                }).ToList(),
            };
            CreateOrder(order);
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            return _db.Orders.Where(o => o.CustomerId == customerId).ToList();
        }


    }
}
