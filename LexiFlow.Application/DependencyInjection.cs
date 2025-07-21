using LexiFlow.App.Services;
using LexiFlow.App.ViewModels;
using LexiFlow.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LexiFlow.App;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<IAuthService, AuthService>();

        // Register ViewModels with factory pattern to avoid circular dependency
        services.AddTransient<LoginViewModel>(provider =>
        {
            var authService = provider.GetRequiredService<IAuthService>();
            var settingsService = provider.GetRequiredService<ISettingsService>();
            return new LoginViewModel(authService, settingsService);
        });

        return services;
    }
}
