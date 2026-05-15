using Men_Accessories.Models;

namespace Men_Accessories.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Product GetById(int id);
        List<Product> GetByCategory(int categoryId);
        List<Product> GetByPriceRange(decimal minPrice, decimal maxPrice);
        List<Product> Search(string keyword);
        void ToggleFavorite(int customerId, int productId);
        bool IsCustomerFavorite(int customerId, int productId);
        List<Product> GetCustomerFavorites(int customerId);
    }
}
