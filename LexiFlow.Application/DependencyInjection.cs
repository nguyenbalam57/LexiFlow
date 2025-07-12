using LexiFlow.Application.Services;
using LexiFlow.Application.ViewModels;
using LexiFlow.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LexiFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<IAuthService, AuthService>();

        // Register ViewModels
        services.AddTransient<LoginViewModel>();

        return services;
    }
}
