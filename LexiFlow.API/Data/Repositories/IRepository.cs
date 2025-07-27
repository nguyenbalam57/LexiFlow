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
        /// <returns>IQueryable of entities for further filtering</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Get all entities as an enumerable collection
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
        /// Update multiple entities
        /// </summary>
        /// <param name="entities">Entities to update</param>
        void UpdateRange(IEnumerable<T> entities);

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
        /// <returns>True if at least one entity matches, false otherwise</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Count entities matching the predicate
        /// </summary>
        /// <param name="predicate">Filter predicate</param>
        /// <returns>Number of entities matching the predicate</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get a paged result of entities
        /// </summary>
        /// <param name="filter">Filter predicate</param>
        /// <param name="orderBy">Order by function</param>
        /// <param name="includeProperties">Navigation properties to include</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged result of entities</returns>
        Task<PagedResult<T>> GetPagedAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            int page = 1,
            int pageSize = 10);
    }

    /// <summary>
    /// Paged result of entities
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Total number of items
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Items in the current page
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Whether there is a previous page
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Whether there is a next page
        /// </summary>
        public bool HasNextPage => Page < TotalPages;
    }
}