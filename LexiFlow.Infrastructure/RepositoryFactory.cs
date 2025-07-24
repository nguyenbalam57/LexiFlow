// LexiFlow.Infrastructure/RepositoryFactory.cs (Complete implementation)
using LexiFlow.Core.Interfaces;
using LexiFlow.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LexiFlow.Infrastructure
{
    /// <summary>
    /// Interface for repository factory
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Gets a repository for the specified entity type
        /// </summary>
        IRepository<T> GetRepository<T>() where T : BaseEntity;

        /// <summary>
        /// Gets a specific repository implementation
        /// </summary>
        TRepo GetRepository<TRepo>() where TRepo : class;

        /// <summary>
        /// Creates a new unit of work
        /// </summary>
        IUnitOfWork CreateUnitOfWork();
    }

    /// <summary>
    /// Repository factory implementation
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RepositoryFactory> _logger;
        private readonly ConcurrentDictionary<Type, object> _repositories = new ConcurrentDictionary<Type, object>();

        public RepositoryFactory(
            IServiceProvider serviceProvider,
            ILogger<RepositoryFactory> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return (IRepository<T>)_repositories.GetOrAdd(typeof(T), CreateRepository<T>);
        }

        public TRepo GetRepository<TRepo>() where TRepo : class
        {
            return (TRepo)_repositories.GetOrAdd(typeof(TRepo), CreateSpecificRepository<TRepo>);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            try
            {
                return _serviceProvider.GetRequiredService<IUnitOfWork>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating unit of work");
                throw;
            }
        }

        private IRepository<T> CreateRepository<T>() where T : BaseEntity
        {
            try
            {
                // First try to get a custom repository implementation
                var customRepo = _serviceProvider.GetService<IRepository<T>>();
                if (customRepo != null)
                {
                    _logger.LogDebug("Using custom repository for {EntityType}", typeof(T).Name);
                    return customRepo;
                }

                // Fall back to generic repository
                var dbContext = _serviceProvider.GetRequiredService<AppDbContext>();
                var logger = _serviceProvider.GetRequiredService<ILogger<EfRepository<T>>>();

                _logger.LogDebug("Creating generic repository for {EntityType}", typeof(T).Name);
                return new EfRepository<T>(dbContext, logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating repository for {EntityType}", typeof(T).Name);
                throw;
            }
        }

        private object CreateSpecificRepository<TRepo>() where TRepo : class
        {
            try
            {
                var repo = _serviceProvider.GetService<TRepo>();
                if (repo != null)
                {
                    _logger.LogDebug("Using registered repository for {RepositoryType}", typeof(TRepo).Name);
                    return repo;
                }

                // Handle specific repository types
                if (typeof(TRepo) == typeof(IUserRepository))
                {
                    _logger.LogDebug("Creating UserRepository");
                    return _serviceProvider.GetRequiredService<IUserRepository>();
                }
                else if (typeof(TRepo) == typeof(IRoleRepository))
                {
                    _logger.LogDebug("Creating RoleRepository");
                    return _serviceProvider.GetRequiredService<IRoleRepository>();
                }
                else if (typeof(TRepo) == typeof(IVocabularyRepository))
                {
                    _logger.LogDebug("Creating VocabularyRepository");
                    return _serviceProvider.GetRequiredService<IVocabularyRepository>();
                }
                else if (typeof(TRepo) == typeof(ICategoryRepository))
                {
                    _logger.LogDebug("Creating CategoryRepository");
                    return _serviceProvider.GetRequiredService<ICategoryRepository>();
                }
                else if (typeof(TRepo) == typeof(IUserActivityRepository))
                {
                    _logger.LogDebug("Creating UserActivityRepository");
                    return _serviceProvider.GetRequiredService<IUserActivityRepository>();
                }

                throw new InvalidOperationException($"Repository type {typeof(TRepo).Name} is not registered");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating repository for {RepositoryType}", typeof(TRepo).Name);
                throw;
            }
        }
    }
}