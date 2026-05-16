using System.Linq.Expressions;

namespace Men_Accessories.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        // Sync methods
        public List<T> GetAll();
        public T? GetByKey(Expression<Func<T, bool>> predicate);
        public List<T> GetByAttribute(Expression<Func<T, bool>> predicate);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete<TKey>(TKey id);

        // Async methods
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByKeyAsync(Expression<Func<T, bool>> predicate);
        public Task<List<T>> GetByAttributeAsync(Expression<Func<T, bool>> predicate);
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task DeleteAsync<TKey>(TKey id);
    }
}
