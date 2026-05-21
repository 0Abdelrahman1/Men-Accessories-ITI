using Men_Accessories.Contexts;
using Men_Accessories.Models;
using Men_Accessories.Repositories;
using Men_Accessories.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Men_Accessories.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly MenAccessoriesContext _context;

        public ProductController(
            IProductRepository productRepository, 
            MenAccessoriesContext menAccessoriesContext)
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

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.Rates)
                .FirstOrDefault(p => p.Id == id);
                
            if (product == null)
                return NotFound();
                
            return View(product);
        }

        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewBag.CategoriesList = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, string[]? productImageUrls)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Add product
                    _productRepository.Add(product);

                    // Add additional images if provided
                    if (productImageUrls != null && productImageUrls.Length > 0)
                    {
                        foreach (var imageUrl in productImageUrls)
                        {
                            if (!string.IsNullOrWhiteSpace(imageUrl))
                            {
                                var productImage = new ProductImage
                                {
                                    ProductId = product.Id,
                                    ImageUrl = imageUrl.Trim()
                                };
                                _context.ProductImages.Add(productImage);
                            }
                        }
                    }
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating product: {ex.Message}");
                }
            }

            var categories = _context.Categories.ToList();
            ViewBag.CategoriesList = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);
                
            if (product == null)
                return NotFound();

            var categories = _context.Categories.ToList();
            ViewBag.CategoriesList = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product, string[]? productImageUrls)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Get existing product
                    var existingProduct = _context.Products.Find(id);
                    if (existingProduct == null)
                        return NotFound();

                    // Update product properties
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.ImageUrl = product.ImageUrl;
                    existingProduct.Price = product.Price;
                    existingProduct.StockQuantity = product.StockQuantity;
                    existingProduct.Discount = product.Discount;
                    existingProduct.IsFeatured = product.IsFeatured;
                    existingProduct.CategoryId = product.CategoryId;

                    _productRepository.Update(existingProduct);

                    // Add new additional images if provided
                    if (productImageUrls != null && productImageUrls.Length > 0)
                    {
                        var currentImageCount = _context.ProductImages.Count(pi => pi.ProductId == id);
                        
                        foreach (var imageUrl in productImageUrls)
                        {
                            if (!string.IsNullOrWhiteSpace(imageUrl) && currentImageCount < 5)
                            {
                                var productImage = new ProductImage
                                {
                                    ProductId = id,
                                    ImageUrl = imageUrl.Trim()
                                };
                                _context.ProductImages.Add(productImage);
                                currentImageCount++;
                            }
                        }
                    }
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating product: {ex.Message}");
                }
            }

            var categories = _context.Categories.ToList();
            ViewBag.CategoriesList = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProductImage(int productImageId)
        {
            var productImage = _context.ProductImages.Find(productImageId);
            if (productImage == null)
                return NotFound();

            try
            {
                int productId = productImage.ProductId;
                _context.ProductImages.Remove(productImage);
                _context.SaveChanges();

                return RedirectToAction("Edit", new { id = productId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error deleting image: {ex.Message}");
                return RedirectToAction("Edit", new { id = productImage.ProductId });
            }
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetByKey(p => p.Id == id);
            if (product == null)
                return NotFound();
                
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction(nameof(Index));
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

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RateProduct(string email, int productId, int stars, string? comment)
        {
            _productRepository.AddRating(email, productId, stars, comment);
            return RedirectToAction("Details", new { id = productId });
        }
    }
}
