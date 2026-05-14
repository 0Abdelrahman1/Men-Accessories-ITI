using Men_Accessories.Models;
using Men_Accessories.Repositories;
using Microsoft.Identity.Client;

namespace Men_Accessories.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _productRepository.GetByCategory(categoryId);
        }

        public List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _productRepository.GetByPriceRange(minPrice, maxPrice);
        }

        public List<Product> SearchProducts(string keyword)
        {
            return _productRepository.Search(keyword);
        }

        public void AddProduct(Product product)
        {
            _productRepository.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.Update(product);
        }

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);
        }

        public void ToggleFavorite(int customerId, int productId)
        {
            _productRepository.ToggleFavorite(customerId, productId);
        }

        public List<Product> GetCustomerFavorites(int customerId)
        {
            return _productRepository.GetCustomerFavorites(customerId);
        }

        public bool IsFavorite(int customerId, int productId)
        {
            return _productRepository.IsCustomerFavorite(customerId, productId);
        }
    }
}
