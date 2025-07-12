
using LexiFlow.Core.Interfaces;
using LexiFlow.Infrastructure.Data;
using LexiFlow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LexiFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Entity Framework
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Register database initializer
        services.AddSingleton<SqlDatabaseInitializer>();

        // Register SQL Entity Adapter
        services.AddScoped<SqlEntityAdapter>();

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs/lexiflow_.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddSingleton(Log.Logger);

        return services;
    }

    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        var initializer = serviceProvider.GetRequiredService<SqlDatabaseInitializer>();
        await initializer.InitializeDatabaseAsync();
    }
}