using LexiFlow.AdminDashboard.Services;
using LexiFlow.AdminDashboard.ViewModels;
using LexiFlow.Core.Entities;
using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Validation;
using LexiFlow.Infrastructure;
using LexiFlow.Infrastructure.Migrations;
using LexiFlow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace LexiFlow.API
{
    /// <summary>
    /// Extension methods for service registration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds core services
        /// </summary>
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
            });

            // Add repositories
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // Add custom repositories if needed
            // services.AddScoped<IRepository<User>, UserRepository>();

            // Add repository factory
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>();

            // Add unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add validators
            services.AddSingleton<EntityValidator<User>, UserValidator>();
            services.AddSingleton<EntityValidator<VocabularyItem>, VocabularyItemValidator>();
            services.AddSingleton<IValidatorFactory, ValidatorFactory>();

            // Add migration manager
            services.AddSingleton<DbMigrationManager>();

            return services;
        }

        /// <summary>
        /// Adds API services
        /// </summary>
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add API options
            services.Configure<ApiClientOptions>(configuration.GetSection("ApiClient"));
            services.Configure<ApiEndpoints>(configuration.GetSection("ApiEndpoints"));
            services.Configure<ApiCacheOptions>(configuration.GetSection("ApiCache"));
            services.Configure<ApiSyncOptions>(configuration.GetSection("ApiSync"));

            // Add API services
            services.AddSingleton<IApiClient, ApiClient>();
            services.AddSingleton<IApiCacheService, ApiCacheService>();
            services.AddScoped<IAdminDashboardService, ApiDashboardService>();
            services.AddSingleton<IApiSyncService, ApiSyncService>();

            // Add caching decorator if enabled
            if (configuration.GetSection("ApiCache").GetValue<bool>("EnableCaching", true))
            {
                services.Decorate<IAdminDashboardService, CachingDashboardService>();
            }

            return services;
        }

        /// <summary>
        /// Adds view models
        /// </summary>
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            // Register view models
            services.AddTransient<AdminDashboardViewModel>();
            services.AddTransient<UserManagementViewModel>();

            // Add other view models as needed

            return services;
        }

        /// <summary>
        /// Helper method for decorating services
        /// </summary>
        private static IServiceCollection Decorate<TService, TDecorator>(this IServiceCollection services)
            where TService : class
            where TDecorator : class, TService
        {
            var serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            if (serviceDescriptor == null)
            {
                throw new InvalidOperationException($"Service {typeof(TService).Name} is not registered");
            }

            var decoratorFactory = ActivatorUtilities.CreateFactory(
                typeof(TDecorator),
                new[] { typeof(TService) });

            services.Replace(ServiceDescriptor.Describe(
                serviceDescriptor.ServiceType,
                s => (TService)decoratorFactory(s, new[] { s.GetRequiredService<TService>() }),
                serviceDescriptor.Lifetime));

            return services;
        }
    }
}