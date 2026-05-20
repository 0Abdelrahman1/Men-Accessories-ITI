using Men_Accessories.Models;

namespace Men_Accessories.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        List<Product> GetByCategory(int categoryId);
        List<Product> GetByPriceRange(decimal minPrice, decimal maxPrice);
        List<Product> Search(string keyword);
        void ToggleFavorite(int customerId, int productId);
        bool IsCustomerFavorite(int customerId, int productId);
        List<Product> GetCustomerFavorites(int customerId);

        IQueryable<Product> GetAllQueryable();
        (List<Product> products, int totalCount) GetPaginatedProducts(int pageIndex, int pageSize);
        void AddRating(string email, int productId, int stars, string? comment);
    }
}
