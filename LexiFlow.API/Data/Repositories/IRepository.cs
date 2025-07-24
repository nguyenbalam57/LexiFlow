using System.Linq.Expressions;

namespace LexiFlow.API.Data.Repositories
{
    /// <summary>
    /// Generic repository interface defining common CRUD operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>IEnumerable of entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get entities matching the specified predicate
        /// </summary>
        /// <param name="predicate">Filter predicate</param>
        /// <returns>IEnumerable of filtered entities</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get a single entity by id
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity or null if not found</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Get a single entity matching the predicate
        /// </summary>
        /// <param name="predicate">Filter predicate</param>
        /// <returns>Entity or null if not found</returns>
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Add multiple entities
        /// </summary>
        /// <param name="entities">Entities to add</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Remove an entity
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        void Remove(T entity);

        /// <summary>
        /// Remove multiple entities
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Check if any entity matches the predicate
        /// </summary>
        /// <param name="predicate">Filter predicate</param>
        /// <returns>True if any entity matches, false otherwise</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Count entities matching the predicate
        /// </summary>
        /// <param name="predicate">Filter predicate</param>
        /// <returns>Count of matching entities</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get entities with pagination
        /// </summary>
        /// <param name="filter">Filter predicate</param>
        /// <param name="orderBy">Order by function</param>
        /// <param name="includeProperties">Navigation properties to include</param>
        /// <param name="pageIndex">Page index (0-based)</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged result</returns>
        Task<PagedResult<T>> GetPagedAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            int pageIndex = 0,
            int pageSize = 10);
    }

    /// <summary>
    /// Paged result containing items and pagination metadata
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageIndex > 0;
        public bool HasNextPage => PageIndex < TotalPages - 1;
    }
}