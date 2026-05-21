using Men_Accessories.Contexts;
using Men_Accessories.Models;
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

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
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
            var orderQuery = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();

            Order? order;

            if (User.IsInRole("Admin"))
            {
                order = orderQuery.FirstOrDefault(o => o.Id == id);
            }
            else
            {
                int customerId = GetCustomerId();
                order = orderQuery.FirstOrDefault(o => o.Id == id && o.CustomerId == customerId);
            }

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