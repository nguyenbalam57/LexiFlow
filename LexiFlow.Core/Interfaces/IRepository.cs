using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LexiFlow.Core.Entities;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Generic repository interface for database operations
    /// </summary>
    /// <typeparam name="T">Entity type derived from BaseEntity</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets entities matching the specified predicate
        /// </summary>
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets paged entities matching the specified predicate
        /// </summary>
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            Expression<Func<T, bool>> predicate,
            int page,
            int pageSize,
            string sortBy = "",
            bool sortAscending = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single entity by ID
        /// </summary>
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single entity matching the specified predicate
        /// </summary>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first entity that matches the predicate
        /// </summary>
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any entity matches the specified predicate
        /// </summary>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts entities matching the specified predicate
        /// </summary>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task<T> AddAsync(T entity, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple entities
        /// </summary>
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task<T> UpdateAsync(T entity, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates multiple entities
        /// </summary>
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity by ID
        /// </summary>
        Task<bool> DeleteAsync(int id, int userId, bool softDelete = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        Task<bool> DeleteAsync(T entity, int userId, bool softDelete = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes multiple entities
        /// </summary>
        Task<bool> DeleteRangeAsync(IEnumerable<T> entities, int userId, bool softDelete = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a queryable collection for advanced querying
        /// </summary>
        IQueryable<T> AsQueryable();

        /// <summary>
        /// Attaches a detached entity to the context
        /// </summary>
        void Attach(T entity);

        /// <summary>
        /// Detaches an entity from the context
        /// </summary>
        void Detach(T entity);

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins a new transaction
        /// </summary>
        Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a database transaction
    /// </summary>
    public interface IDbTransaction : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Commits the transaction
        /// </summary>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}