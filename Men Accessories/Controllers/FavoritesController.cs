using Men_Accessories.Contexts;
using Men_Accessories.Models;
using Men_Accessories.Repositories;
using Men_Accessories.Services; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Men_Accessories.Controllers
{
    [Authorize] 
    public class FavoritesController : Controller
    {
        private readonly IProductRepository _productRepository; 
        private readonly MenAccessoriesContext _context;

        public FavoritesController(IProductRepository productRepository, MenAccessoriesContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == userId);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            var favorites = _productRepository.GetCustomerFavorites(customer.Id);

            return View(favorites);
        }

        [HttpPost]
        public IActionResult Toggle(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Json(new { success = false, message = "Please login first" });

            var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == userId);
            if (customer == null)
                return Json(new { success = false, message = "Customer not found" });

            _productRepository.ToggleFavorite(customer.Id, productId);

            bool isFavorite = _productRepository.IsCustomerFavorite(customer.Id, productId);

            return Json(new { success = true, isFavorite = isFavorite });
        }
    }
}