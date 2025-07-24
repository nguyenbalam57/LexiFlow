// LexiFlow.Infrastructure/EfTransaction.cs
using System.Threading.Tasks;
using LexiFlow.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace LexiFlow.Infrastructure
{
    /// <summary>
    /// Implementation of IDbTransaction using Entity Framework
    /// </summary>
    public class EfTransaction : IDbTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public EfTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _transaction.DisposeAsync();
        }
    }
}