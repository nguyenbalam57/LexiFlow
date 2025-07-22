using System;
using System.Collections.Concurrent;
using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using LexiFlow.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LexiFlow.Infrastructure
{
    /// <summary>
    /// Factory for creating repositories
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// Gets a repository for the specified entity type
        /// </summary>
        IRepository<T> GetRepository<T>() where T : BaseEntity;

        /// <summary>
        /// Creates a unit of work
        /// </summary>
        IUnitOfWork CreateUnitOfWork();
    }

    /// <summary>
    /// Factory implementation for creating repositories and unit of work
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, object> _repositories = new ConcurrentDictionary<Type, object>();
        private readonly ILogger<RepositoryFactory> _logger;

        public RepositoryFactory(IServiceProvider serviceProvider, ILogger<RepositoryFactory> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return (IRepository<T>)_repositories.GetOrAdd(typeof(T), CreateRepository<T>);
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
    }
}