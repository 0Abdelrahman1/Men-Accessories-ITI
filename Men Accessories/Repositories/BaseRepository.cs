using Men_Accessories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Men_Accessories.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly MenAccessoriesContext _context;
        public BaseRepository(MenAccessoriesContext context)
        {
            _context = context;
        }

        #region Synchronous Methods
        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T? GetByKey<TKey>(TKey id, Func<T, TKey> keySelector)
        {
            return _context.Set<T>().FirstOrDefault(s => keySelector(s)!.Equals(id));
        }

        public List<T> GetByAttribute<TAttribute>(TAttribute value, Func<T, TAttribute> attributeSelector)
        {
            return _context.Set<T>().Where(s => attributeSelector(s)!.Equals(value)).ToList();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void Delete<TKey>(TKey id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }
        #endregion

        #region Asynchronous Methods
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByKeyAsync<TKey>(TKey id, Func<T, TKey> keySelector)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(s => keySelector(s)!.Equals(id));
        }

        public async Task<List<T>> GetByAttributeAsync<TAttribute>(TAttribute value, Func<T, TAttribute> attributeSelector)
        {
            return await _context.Set<T>().Where(s => attributeSelector(s)!.Equals(value)).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync<TKey>(TKey id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}

