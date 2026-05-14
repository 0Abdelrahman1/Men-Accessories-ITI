using Men_Accessories.Models;
using Men_Accessories.Repositories;
using Men_Accessories.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Men_Accessories.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBaseRepository<Category> _categoryRepository;

        public HomeController(IProductService productService, IBaseRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            ViewBag.Categories = _categoryRepository.GetAll();
            return View(products);
        }
        // AJAX endpoint for filtering/sorting
        [HttpGet]
        public IActionResult FilterAndSort(string categoryIds = "", string sortBy = "default")
        {
            var products = _productService.GetAllProducts();

            // FILTER by multiple categories
            if (!string.IsNullOrEmpty(categoryIds))
            {
                var ids = categoryIds.Split(',')
                    .Where(id => int.TryParse(id, out _))
                    .Select(int.Parse)
                    .ToList();

                if (ids.Count > 0)
                {
                    products = products.Where(p => ids.Contains(p.CategoryId)).ToList();
                }
            }

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

            return PartialView("_ProductsGrid", products);
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
