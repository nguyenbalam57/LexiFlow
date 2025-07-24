// LexiFlow.Core/Interfaces/IDbTransaction.cs
using System;
using System.Threading.Tasks;

namespace LexiFlow.Core.Interfaces
{
    /// <summary>
    /// Interface for database transactions
    /// </summary>
    public interface IDbTransaction : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Commits the transaction
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        Task RollbackAsync();
    }
}