using System.Linq.Expressions;

namespace ContosoUniversity.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetQueryable(); // Add method to get queryable for pagination
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(int id);
        
        /// <summary>
        /// Gets an entity by ID with related entities eagerly loaded.
        /// Supports Include() and ThenInclude() to avoid N+1 query problems.
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <param name="includes">Navigation property expressions to include</param>
        /// <returns>Entity with specified includes, or null if not found</returns>
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
    }
}
