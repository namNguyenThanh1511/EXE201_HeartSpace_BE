using System.Linq.Expressions;

namespace HeartSpace.Domain.Repositories
{
    public interface IRepositoryBase<T>
    {
        // Sync methods
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Async methods
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
    }
}
