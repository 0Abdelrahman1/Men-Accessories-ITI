using Men_Accessories.Contexts;
using Men_Accessories.Models;
using Microsoft.EntityFrameworkCore;

namespace Men_Accessories.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MenAccessoriesContext _context;

        public ProductRepository(MenAccessoriesContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            return _context.Products.Include(p => p.Category).ToList();
        }

        public Product? GetByKey<TKey>(TKey id, Func<Product, TKey> keySelector)
        {
            return _context.Products.Include(p => p.Category).FirstOrDefault(p => keySelector(p).Equals(id));
        }

        public List<Product> GetByAttribute<TAttribute>(TAttribute value, Func<Product, TAttribute> attributeSelector)
        {
            return _context.Products.Include(p => p.Category).Where(p => attributeSelector(p).Equals(value)).ToList();
        }

        public void Add(Product entity)
        {
            _context.Products.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Product entity)
        {
            _context.Products.Update(entity);
            _context.SaveChanges();
        }

        public void Delete<TKey>(TKey id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetByKeyAsync<TKey>(TKey id, Func<Product, TKey> keySelector)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => keySelector(p).Equals(id));
        }

        public async Task<List<Product>> GetByAttributeAsync<TAttribute>(TAttribute value, Func<Product, TAttribute> attributeSelector)
        {
            return await _context.Products.Include(p => p.Category).Where(p => attributeSelector(p).Equals(value)).ToListAsync();
        }

        public async Task AddAsync(Product entity)
        {
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync<TKey>(TKey id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

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
    }
}
