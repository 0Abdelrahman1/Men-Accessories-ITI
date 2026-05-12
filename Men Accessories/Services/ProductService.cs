using Men_Accessories.Models;
using Men_Accessories.Repositories;

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

        public Product? GetProductById(int id)
        {
            return _productRepository.GetByKey(id, p => p.Id);
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
    }
}
