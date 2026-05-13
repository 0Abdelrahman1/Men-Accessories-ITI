using Men_Accessories.Models;

namespace Men_Accessories.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        List<Product> GetByCategory(int categoryId);
        List<Product> GetByPriceRange(decimal minPrice, decimal maxPrice);
        List<Product> Search(string keyword);
    }
}
