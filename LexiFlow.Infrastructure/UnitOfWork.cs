// LexiFlow.Infrastructure/UnitOfWork.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Infrastructure
{
    /// <summary>
    /// Implementation of unit of work pattern using Entity Framework
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private IDbTransaction _currentTransaction;
        private bool _disposed = false;

        public UnitOfWork(
            AppDbContext dbContext,
            IRepositoryFactory repositoryFactory,
            ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Implement specific repository properties
        private IUserRepository _users;
        public IUserRepository Users => _users ??= _repositoryFactory.GetRepository<IUserRepository>();

        private IRoleRepository _roles;
        public IRoleRepository Roles => _roles ??= _repositoryFactory.GetRepository<IRoleRepository>();

        private IVocabularyRepository _vocabularyItems;
        public IVocabularyRepository VocabularyItems => _vocabularyItems ??= _repositoryFactory.GetRepository<IVocabularyRepository>();

        private ICategoryRepository _categories;
        public ICategoryRepository Categories => _categories ??= _repositoryFactory.GetRepository<ICategoryRepository>();

        private IUserActivityRepository _userActivities;
        public IUserActivityRepository UserActivities => _userActivities ??= _repositoryFactory.GetRepository<IUserActivityRepository>();

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return (IRepository<T>)repository;
            }

            var newRepo = _repositoryFactory.GetRepository<T>();
            _repositories[typeof(T)] = newRepo;
            return newRepo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Saving changes to database");
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to database");
                throw;
            }
        }

        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                _logger.LogWarning("Transaction already in progress");
                return _currentTransaction;
            }

            _logger.LogDebug("Beginning new transaction");
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                if (_currentTransaction == null)
                {
                    _logger.LogWarning("No transaction to commit");
                    return;
                }

                _logger.LogDebug("Committing transaction");
                await _currentTransaction.CommitAsync();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_currentTransaction == null)
                {
                    _logger.LogWarning("No transaction to rollback");
                    return;
                }

                _logger.LogDebug("Rolling back transaction");
                await _currentTransaction.RollbackAsync();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
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
                    _currentTransaction?.Dispose();
                    _dbContext.Dispose();
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
            if (!_disposed)
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                }

                await _dbContext.DisposeAsync();
                _disposed = true;
            }
        }
    }
}