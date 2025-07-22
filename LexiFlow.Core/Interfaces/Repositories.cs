using LexiFlow.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int? take = null);
        Task<(IEnumerable<T>, int)> GetPagedAsync(Expression<Func<T, bool>> predicate, int page, int pageSize, string orderByProperty, bool isAscending);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity, int userId);
        Task UpdateAsync(T entity, int userId);
        Task DeleteAsync(int id, int userId);
    }

    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<bool> IsInRoleAsync(int userId, string roleName);
    }

    /// <summary>
    /// Role repository interface
    /// </summary>
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetByUserIdAsync(int userId);
    }

    /// <summary>
    /// Vocabulary repository interface
    /// </summary>
    public interface IVocabularyRepository : IGenericRepository<Vocabulary>
    {
        Task<IEnumerable<Vocabulary>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Vocabulary>> SearchAsync(string keyword, string language = "all");
    }

    /// <summary>
    /// Category repository interface
    /// </summary>
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetByParentIdAsync(int? parentId);
    }

    /// <summary>
    /// User activity repository interface
    /// </summary>
    public interface IUserActivityRepository : IGenericRepository<UserActivity>
    {
        Task<IEnumerable<UserActivity>> GetByUserIdAsync(int userId, int take = 100);
        Task<IEnumerable<UserActivity>> GetByModuleAsync(string module, int take = 100);
        Task<IEnumerable<UserActivity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}