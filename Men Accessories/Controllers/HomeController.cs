using Men_Accessories.Contexts;
using Men_Accessories.Models;
using Men_Accessories.Repositories;
using Men_Accessories.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Men_Accessories.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly MenAccessoriesContext _context;

        public HomeController(IProductService productService, IBaseRepository<Category> categoryRepository,MenAccessoriesContext menAccessoriesContext )
        {
            _categoryRepository = categoryRepository;
            _context  = menAccessoriesContext;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            ViewBag.Categories = _categoryRepository.GetAll();
            List<int> userFavorites = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == userId);

                if (customer != null && customer.FavoriteProductIds != null)
                {
                    userFavorites = customer.FavoriteProductIds;
                }
            }
            ViewBag.FavoriteProductIds = userFavorites;
            return View(products);
        }
        // AJAX endpoint for filtering/sorting
        [HttpGet]
        public IActionResult FilterAndSort(string categoryIds = "", string sortBy = "default", string keyword = "", bool inStock = false, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue)
        {
            var products = _productService.GetAllProducts();

            // SEARCH by keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p =>
                    p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // FILTER by categories
            if (!string.IsNullOrEmpty(categoryIds))
            {
                var ids = categoryIds.Split(',')
                    .Where(id => int.TryParse(id, out _))
                    .Select(int.Parse)
                    .ToList();

                if (ids.Count > 0)
                    products = products.Where(p => ids.Contains(p.CategoryId)).ToList();
            }

            // FILTER by stock
            if (inStock)
                products = products.Where(p => p.StockQuantity > 0).ToList();

            // FILTER by price range
            products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();

            // SORT
            products = sortBy switch
            {
                "name_asc" => products.OrderBy(p => p.Name).ToList(),
                "name_desc" => products.OrderByDescending(p => p.Name).ToList(),
                "price_asc" => products.OrderBy(p => p.Price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.Price).ToList(),
                "newest" => products.OrderByDescending(p => p.CreatedAt).ToList(),
                "oldest" => products.OrderBy(p => p.CreatedAt).ToList(),
                _ => products
            };

            List<int> userFavorites = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == userId);
                if (customer != null && customer.FavoriteProductIds != null)
                    userFavorites = customer.FavoriteProductIds;
            }
            ViewBag.FavoriteProductIds = userFavorites;

            return PartialView("_ProductsGrid", products);
        }

        [HttpGet]
        public IActionResult GetPriceRange()
        {
            var products = _productService.GetAllProducts();
            return Json(new
            {
                min = products.Any() ? products.Min(p => p.Price) : 0,
                max = products.Any() ? products.Max(p => p.Price) : 1000
            });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
