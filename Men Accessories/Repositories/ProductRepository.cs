using Men_Accessories.Contexts;
using Men_Accessories.Models;
using Microsoft.EntityFrameworkCore;

namespace Men_Accessories.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {

        public ProductRepository(MenAccessoriesContext context) : base(context) { }

        public List<Product> GetByCategory(int categoryId)
        {
            return _context.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).ToList();
        }

        public List<Product> GetByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _context.Products.Include(p => p.Category).Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
        }

        public List<Product> Search(string keyword)
        {
            return _context.Products.Include(p => p.Category)
                .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
                .ToList();
        }


        public void ToggleFavorite(int customerId, int productId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer != null)
            {
                if (customer.FavoriteProductIds.Contains(productId))
                {
                    customer.FavoriteProductIds.Remove(productId);
                }
                else
                {
                    customer.FavoriteProductIds.Add(productId);
                }
                _context.SaveChanges();
            }
        }

        public List<Product> GetCustomerFavorites(int customerId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer != null && customer.FavoriteProductIds.Any())
            {
                return _context.Products.Where(p => customer.FavoriteProductIds.Contains(p.Id)).ToList();
            }
            return new List<Product>();
        }

        public bool IsCustomerFavorite(int customerId, int productId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            return customer != null && customer.FavoriteProductIds.Contains(productId);
        }

        public IQueryable<Product> GetAllQueryable()
        {
            return _context.Products.Include(p => p.Category);
        }
        public (List<Product> products, int totalCount) GetPaginatedProducts(int pageIndex, int pageSize)
        {
            var query = GetAllQueryable();

            int totalCount = query.Count();

            var products = query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (products, totalCount);
        }
    }
}
