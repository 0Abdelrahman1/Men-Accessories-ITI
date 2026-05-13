using Men_Accessories.Models;

namespace Men_Accessories.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetOrdersByCustomerId(int customerId);
        void CreateOrder(Order order);
        void CreateOrderFromCart(Cart cart);
    }
}
