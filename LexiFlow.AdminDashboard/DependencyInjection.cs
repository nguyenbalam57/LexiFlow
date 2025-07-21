using LexiFlow.AdminDashboard.Services;
using LexiFlow.AdminDashboard.ViewModels;
using LexiFlow.AdminDashboard.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LexiFlow.AdminDashboard
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAdminDashboard(this IServiceCollection services, IConfiguration configuration)
        {
            // Register services
            services.AddSingleton<IAdminDashboardService>(provider =>
            {
                var dbContext = provider.GetRequiredService<LexiFlow.Infrastructure.Data.ApplicationDbContext>();
                var logger = provider.GetRequiredService<ILogger<DashboardDataService>>();
                var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";

                return new DashboardDataService(dbContext, logger, connectionString);
            });

            services.AddSingleton<AdminConfigService>();

            // Register ViewModels
            services.AddTransient<AdminDashboardViewModel>();
            services.AddTransient<UserManagementViewModel>();
            services.AddTransient<SystemConfigViewModel>();

            // Register Views
            services.AddTransient<AdminDashboardView>();
            services.AddTransient<UserManagementView>();
            services.AddTransient<SystemConfigView>();

            return services;
        }
    }
}
