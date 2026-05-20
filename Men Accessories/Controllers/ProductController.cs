using Men_Accessories.Contexts;
using Men_Accessories.Models;
using Men_Accessories.Repositories;
using Men_Accessories.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Men_Accessories.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly MenAccessoriesContext _context;

        public ProductController(IProductRepository productRepository, MenAccessoriesContext menAccessoriesContext)
        {
            _productRepository = productRepository;
            _context = menAccessoriesContext;
        }    
       
        public IActionResult Index(int page = 1)
        {
            int pageSize = 12;

            var result = _productRepository.GetPaginatedProducts(page, pageSize);

            var vm = new ProductPaginationViewModel
            {
                Products = result.products,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)result.totalCount / pageSize)
            };

            return View(vm);
        }
       
        public IActionResult Details(int id)
        {
            var product = _productRepository.GetByKey(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewBag.CategoriesList = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Add(product);
                return RedirectToAction("Index");
            }
            var categories = _context.Categories.ToList();
            ViewBag.CategoriesList = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetByKey(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = _context.Categories.ToList();
            ViewBag.CategoriesList = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);
                return RedirectToAction("Index");
            }
            var categories = _context.Categories.ToList();
            ViewBag.CategoriesList = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetByKey(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ToggleFavorite(int customerId, int productId)
        {
            _productRepository.ToggleFavorite(customerId, productId);
            return RedirectToAction("Details", new { id = productId });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Favorites(int customerId)
        {
            var favoriteProducts = _productRepository.GetCustomerFavorites(customerId);
            return View(favoriteProducts);
        }
    }
}
