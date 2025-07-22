using System;
using System.Threading.Tasks;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface quản lý các repository và transaction
    /// </summary>
    public interface IUnitOfWork : IDisposable
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
        /// Lưu các thay đổi vào cơ sở dữ liệu
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Bắt đầu transaction mới
        /// </summary>
        Task BeginTransactionAsync();

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