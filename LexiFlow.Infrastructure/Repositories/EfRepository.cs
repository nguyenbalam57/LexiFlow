using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Infrastructure.Repositories
{
    /// <summary>
    /// Entity Framework implementation of the IRepository interface
    /// </summary>
    /// <typeparam name="T">Entity type derived from BaseEntity</typeparam>
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger _logger;

        public EfRepository(AppDbContext dbContext, ILogger<EfRepository<T>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbSet
                    .Where(e => !e.IsDeleted)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync for {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbSet
                    .Where(e => !e.IsDeleted)
                    .Where(predicate)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAsync for {EntityType} with predicate", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            Expression<Func<T, bool>> predicate,
            int page,
            int pageSize,
            string sortBy = "",
            bool sortAscending = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Ensure valid pagination parameters
                page = page < 1 ? 1 : page;
                pageSize = pageSize < 1 ? 10 : (pageSize > 100 ? 100 : pageSize);

                // Start with base query
                var query = _dbSet
                    .Where(e => !e.IsDeleted)
                    .Where(predicate);

                // Get total count
                var totalCount = await query.CountAsync(cancellationToken);

                // Apply sorting
                IQueryable<T> sortedQuery = query;

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    // Use reflection to get the property for sorting
                    var property = typeof(T).GetProperty(sortBy);

                    if (property != null)
                    {
                        // Create a parameter expression for the lambda
                        var parameter = Expression.Parameter(typeof(T), "x");

                        // Create a property access expression
                        var propertyAccess = Expression.MakeMemberAccess(parameter, property);

                        // Create a lambda expression
                        var lambda = Expression.Lambda(propertyAccess, parameter);

                        // Call the appropriate OrderBy method
                        var methodName = sortAscending ? "OrderBy" : "OrderByDescending";
                        var methodCallExpression = Expression.Call(
                            typeof(Queryable),
                            methodName,
                            new Type[] { typeof(T), property.PropertyType },
                            query.Expression,
                            Expression.Quote(lambda));

                        sortedQuery = query.Provider.CreateQuery<T>(methodCallExpression);
                    }
                    else
                    {
                        // Fall back to sorting by ID if property not found
                        sortedQuery = sortAscending
                            ? query.OrderBy(e => e.Id)
                            : query.OrderByDescending(e => e.Id);
                    }
                }
                else
                {
                    // Default sort by ID if no sort field specified
                    sortedQuery = sortAscending
                        ? query.OrderBy(e => e.Id)
                        : query.OrderByDescending(e => e.Id);
                }

                // Apply pagination
                var items = await sortedQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                return (items, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPagedAsync for {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbSet
                    .Where(e => !e.IsDeleted && e.Id == id)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByIdAsync for {EntityType} with ID {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbSet
                    .Where(e => !e.IsDeleted)
                    .Where(predicate)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FirstOrDefaultAsync for {EntityType} with predicate", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbSet
                    .Where(e => !e.IsDeleted)
                    .Where(predicate)
                    .FirstAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FirstAsync for {EntityType} with predicate", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbSet
                    .Where(e => !e.IsDeleted)
                    .AnyAsync(predicate, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnyAsync for {EntityType} with predicate", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbSet
                    .Where(e => !e.IsDeleted)
                    .CountAsync(predicate, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CountAsync for {EntityType} with predicate", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T> AddAsync(T entity, int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                entity.SetCreationInfo(userId);

                await _dbSet.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Added {EntityType} with ID {Id}", typeof(T).Name, entity.Id);

                return entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error in AddAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error in AddAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddAsync for {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityList = entities.ToList();

                foreach (var entity in entityList)
                {
                    entity.SetCreationInfo(userId);
                }

                await _dbSet.AddRangeAsync(entityList, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Added {Count} {EntityType} entities", entityList.Count, typeof(T).Name);

                return entityList;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error in AddRangeAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error in AddRangeAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddRangeAsync for {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<T> UpdateAsync(T entity, int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                entity.SetModificationInfo(userId);

                _dbContext.Entry(entity).State = EntityState.Modified;

                // Ensure RowVersion is not modified
                _dbContext.Entry(entity).Property(x => x.RowVersion).OriginalValue = entity.RowVersion;

                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Updated {EntityType} with ID {Id}", typeof(T).Name, entity.Id);

                return entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error in UpdateAsync for {EntityType} with ID {Id}", typeof(T).Name, entity.Id);

                // Handle concurrency conflict
                var entry = ex.Entries.Single();
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                if (databaseValues == null)
                {
                    _logger.LogError("Entity was deleted by another user");
                    throw new DbUpdateConcurrencyException("The record you attempted to edit was deleted by another user.", ex);
                }

                var databaseEntity = (T)databaseValues.ToObject();

                // Get information about who modified the entity
                var modifiedBy = databaseEntity.ModifiedBy ?? 0;
                var modifiedAt = databaseEntity.ModifiedAt ?? DateTime.UtcNow;

                _logger.LogError("Entity was modified by user {ModifiedBy} at {ModifiedAt}", modifiedBy, modifiedAt);

                throw new DbUpdateConcurrencyException(
                    $"The record you attempted to edit was modified by another user at {modifiedAt}. " +
                    "Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error in UpdateAsync for {EntityType} with ID {Id}", typeof(T).Name, entity.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateAsync for {EntityType} with ID {Id}", typeof(T).Name, entity.Id);
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityList = entities.ToList();

                foreach (var entity in entityList)
                {
                    entity.SetModificationInfo(userId);
                    _dbContext.Entry(entity).State = EntityState.Modified;

                    // Ensure RowVersion is not modified
                    _dbContext.Entry(entity).Property(x => x.RowVersion).OriginalValue = entity.RowVersion;
                }

                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Updated {Count} {EntityType} entities", entityList.Count, typeof(T).Name);

                return entityList;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error in UpdateRangeAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error in UpdateRangeAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateRangeAsync for {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(int id, int userId, bool softDelete = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);

                if (entity == null)
                {
                    _logger.LogWarning("Delete failed: {EntityType} with ID {Id} not found", typeof(T).Name, id);
                    return false;
                }

                return await DeleteAsync(entity, userId, softDelete, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync for {EntityType} with ID {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(T entity, int userId, bool softDelete = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (softDelete)
                {
                    entity.SetDeletionInfo(userId);
                    _dbContext.Entry(entity).State = EntityState.Modified;
                }
                else
                {
                    _dbSet.Remove(entity);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    softDelete ? "Soft deleted {EntityType} with ID {Id}" : "Hard deleted {EntityType} with ID {Id}",
                    typeof(T).Name, entity.Id);

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error in DeleteAsync for {EntityType} with ID {Id}", typeof(T).Name, entity.Id);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error in DeleteAsync for {EntityType} with ID {Id}", typeof(T).Name, entity.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync for {EntityType} with ID {Id}", typeof(T).Name, entity.Id);
                throw;
            }
        }

        public virtual async Task<bool> DeleteRangeAsync(IEnumerable<T> entities, int userId, bool softDelete = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityList = entities.ToList();

                if (softDelete)
                {
                    foreach (var entity in entityList)
                    {
                        entity.SetDeletionInfo(userId);
                        _dbContext.Entry(entity).State = EntityState.Modified;
                    }
                }
                else
                {
                    _dbSet.RemoveRange(entityList);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    softDelete ? "Soft deleted {Count} {EntityType} entities" : "Hard deleted {Count} {EntityType} entities",
                    entityList.Count, typeof(T).Name);

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error in DeleteRangeAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error in DeleteRangeAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteRangeAsync for {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return _dbSet.Where(e => !e.IsDeleted);
        }

        public virtual void Attach(T entity)
        {
            _dbSet.Attach(entity);
        }

        public virtual void Detach(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error in SaveChangesAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error in SaveChangesAsync for {EntityType}", typeof(T).Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveChangesAsync for {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public virtual async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return new EfDbTransaction(await _dbContext.Database.BeginTransactionAsync(cancellationToken));
        }
    }

    /// <summary>
    /// Entity Framework implementation of the IDbTransaction interface
    /// </summary>
    public class EfDbTransaction : IDbTransaction
    {
        private readonly Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;
        private bool _disposed;

        public EfDbTransaction(Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                }

                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
        }
    }
}