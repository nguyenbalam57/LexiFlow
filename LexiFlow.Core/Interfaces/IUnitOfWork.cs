using LexiFlow.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface quản lý các repository và transaction
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Repository quản lý người dùng
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Repository quản lý vai trò
        /// </summary>
        IRoleRepository Roles { get; }

        /// <summary>
        /// Repository quản lý từ vựng
        /// </summary>
        IVocabularyRepository VocabularyItems { get; }

        /// <summary>
        /// Repository quản lý danh mục
        /// </summary>
        ICategoryRepository Categories { get; }

        /// <summary>
        /// Repository quản lý hoạt động người dùng
        /// </summary>
        IUserActivityRepository UserActivities { get; }

        /// <summary>
        /// Gets a repository for the specified entity type
        /// </summary>
        IRepository<T> GetRepository<T>() where T : BaseEntity;

        /// <summary>
        /// Lưu các thay đổi vào cơ sở dữ liệu
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Bắt đầu transaction mới
        /// </summary>
        Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commit transaction hiện tại
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rollback transaction hiện tại
        /// </summary>
        Task RollbackTransactionAsync();
    }
}