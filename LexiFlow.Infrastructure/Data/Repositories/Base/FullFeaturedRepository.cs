using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data.Repositories.Base
{
    /// <summary>
    /// Repository implementation for entities that support both soft delete and activation
    /// </summary>
    /// <typeparam name="T">Entity type that implements both ISoftDeletable and IActivatable</typeparam>
    public class FullFeaturedRepository<T> : BaseRepository<T>
        where T : class, ISoftDeletable, IActivatable
    {
        public FullFeaturedRepository(LexiFlowContext context) : base(context)
        {
        }

        public override IQueryable<T> GetAll()
        {
            return base.GetAll().Where(e => !e.IsDeleted && e.IsActive);
        }

        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetAll().ToListAsync();
        }

        public override async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public override async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public override async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().AnyAsync(predicate);
        }

        public override async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAll().CountAsync(predicate);
        }

        public override void Remove(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            Update(entity);
        }

        public override void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllIncludingInactiveAndDeletedAsync()
        {
            return await base.GetAllAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllIncludingInactiveAsync()
        {
            return await base.GetAll().Where(e => !e.IsDeleted).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllIncludingDeletedAsync()
        {
            return await base.GetAll().Where(e => e.IsActive).ToListAsync();
        }

        public virtual async Task<PagedResult<T>> GetAllIncludingInactiveAndDeletedPagedAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            int page = 1,
            int pageSize = 10)
        {
            return await base.GetPagedAsync(filter, orderBy, includeProperties, page, pageSize);
        }
    }
}
