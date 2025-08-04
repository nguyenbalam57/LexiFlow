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
    /// Repository implementation for entities that support activation/deactivation
    /// </summary>
    /// <typeparam name="T">Entity type that implements IActivatable</typeparam>
    public class ActivatableRepository<T> : BaseRepository<T> where T : class, IActivatable
    {
        public ActivatableRepository(LexiFlowContext context) : base(context)
        {
        }

        public override IQueryable<T> GetAll()
        {
            return base.GetAll().Where(e => e.IsActive);
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

        public virtual async Task<IEnumerable<T>> GetAllIncludingInactiveAsync()
        {
            return await base.GetAllAsync();
        }

        public virtual async Task<PagedResult<T>> GetAllIncludingInactivePagedAsync(
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
