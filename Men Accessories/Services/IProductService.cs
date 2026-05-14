using Men_Accessories.Models;

namespace Men_Accessories.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
        List<Product> GetProductsByCategory(int categoryId);
        List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        List<Product> SearchProducts(string keyword);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        void ToggleFavorite(int customerId, int productId);
        List<Product> GetCustomerFavorites(int customerId);
        bool IsFavorite(int customerId, int productId);
    }
}
