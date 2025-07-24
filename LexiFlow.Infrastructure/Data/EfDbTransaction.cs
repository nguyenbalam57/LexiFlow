using LexiFlow.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LexiFlow.Infrastructure.Data
{
    /// <summary>
    /// Triển khai IDbTransaction sử dụng Entity Framework
    /// </summary>
    public class EfDbTransaction : IDbTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public EfDbTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        /// <summary>
        /// Commit transaction
        /// </summary>
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.CommitAsync(cancellationToken);
        }

        /// <summary>
        /// Rollback transaction
        /// </summary>
        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

        /// <summary>
        /// Giải phóng tài nguyên
        /// </summary>
        public void Dispose()
        {
            _transaction.Dispose();
        }

        /// <summary>
        /// Giải phóng tài nguyên bất đồng bộ
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await _transaction.DisposeAsync();
        }
    }
}