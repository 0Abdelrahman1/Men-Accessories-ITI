using Men_Accessories.Contexts;
using Men_Accessories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Men_Accessories.Controllers
{
    [Authorize] 
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;
        private readonly MenAccessoriesContext _context;

        public OrderController(ICartService cartService, MenAccessoriesContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        public IActionResult Success(string sessionId)
        {
            int customerId = GetCustomerId();

            _cartService.Checkout(customerId, sessionId);

            return View();
        }
        public IActionResult MyOrders()
        {
            int customerId = GetCustomerId();

            var orders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }

        public IActionResult Details(int id)
        {
            int customerId = GetCustomerId();

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id && o.CustomerId == customerId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        private int GetCustomerId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == userId);

            return customer != null ? customer.Id : 0;
        }
    }
}