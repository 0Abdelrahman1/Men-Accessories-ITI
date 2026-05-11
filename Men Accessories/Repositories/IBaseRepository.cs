using System.Linq.Expressions;

namespace Men_Accessories.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        // Sync methods
        public List<T> GetAll();
        public T? GetByKey<TKey>(TKey id, Func<T, TKey> keySelector);
        public List<T> GetByAttribute<TAttribute>(TAttribute value, Func<T, TAttribute> attributeSelector);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete<TKey>(TKey id);

        // Async methods
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByKeyAsync<TKey>(TKey id, Func<T, TKey> keySelector);
        public Task<List<T>> GetByAttributeAsync<TAttribute>(TAttribute value, Func<T, TAttribute> attributeSelector);
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync<TKey>(TKey id);
    }
}
