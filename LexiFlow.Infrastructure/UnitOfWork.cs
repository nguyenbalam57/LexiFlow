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
    /// Interface for unit of work pattern
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Gets a repository for the specified entity type
        /// </summary>
        IRepository<T> GetRepository<T>() where T : BaseEntity;

        /// <summary>
        /// Gets the repository for User entities
        /// </summary>
        IRepository<User> Users { get; }

        /// <summary>
        /// Gets the repository for Role entities
        /// </summary>
        IRepository<Role> Roles { get; }

        /// <summary>
        /// Gets the repository for Category entities
        /// </summary>
        IRepository<Category> Categories { get; }

        /// <summary>
        /// Gets the repository for VocabularyItem entities
        /// </summary>
        IRepository<VocabularyItem> VocabularyItems { get; }

        /// <summary>
        /// Gets the repository for Lesson entities
        /// </summary>
        IRepository<Lesson> Lessons { get; }

        /// <summary>
        /// Gets the repository for Course entities
        /// </summary>
        IRepository<Course> Courses { get; }

        /// <summary>
        /// Gets the repository for Exercise entities
        /// </summary>
        IRepository<Exercise> Exercises { get; }

        /// <summary>
        /// Gets the repository for UserActivity entities
        /// </summary>
        IRepository<UserActivity> UserActivities { get; }

        /// <summary>
        /// Begins a transaction
        /// </summary>
        Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes made in this unit of work to the database
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Implementation of unit of work pattern using Entity Framework
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
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

        // Frequently used repositories - lazy initialized
        private IRepository<User>? _users;
        public IRepository<User> Users => _users ??= GetRepository<User>();

        private IRepository<Role>? _roles;
        public IRepository<Role> Roles => _roles ??= GetRepository<Role>();

        private IRepository<Category>? _categories;
        public IRepository<Category> Categories => _categories ??= GetRepository<Category>();

        private IRepository<VocabularyItem>? _vocabularyItems;
        public IRepository<VocabularyItem> VocabularyItems => _vocabularyItems ??= GetRepository<VocabularyItem>();

        private IRepository<Lesson>? _lessons;
        public IRepository<Lesson> Lessons => _lessons ??= GetRepository<Lesson>();

        private IRepository<Course>? _courses;
        public IRepository<Course> Courses => _courses ??= GetRepository<Course>();

        private IRepository<Exercise>? _exercises;
        public IRepository<Exercise> Exercises => _exercises ??= GetRepository<Exercise>();

        private IRepository<UserActivity>? _userActivities;
        public IRepository<UserActivity> UserActivities => _userActivities ??= GetRepository<UserActivity>();

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            var type = typeof(T);

            if (_repositories.ContainsKey(type))
            {
                return (IRepository<T>)_repositories[type];
            }

            var repository = _repositoryFactory.GetRepository<T>();
            _repositories.Add(type, repository);

            return repository;
        }

        public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Beginning transaction");
                return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error beginning transaction");
                throw;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogDebug("Saving changes");
                return await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes");
                throw;
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
            if (_dbContext != null)
            {
                await _dbContext.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}