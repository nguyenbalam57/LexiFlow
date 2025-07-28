using FluentValidation;
using FluentValidation.AspNetCore;
using LexiFlow.API.DTOs.Auth;
using LexiFlow.API.Filters;
using LexiFlow.API.Validators;
using LexiFlow.API.Validators.AuthValidators;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LexiFlow.API.Extensions
{
    /// <summary>
    /// Extension methods for setting up validation
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Add validation services and filters
        /// </summary>
        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            // Add FluentValidation
            services.AddFluentValidationAutoValidation();

            // Register validators
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordRequestValidator>();

            // Add validation filter
            services.AddScoped<ValidationFilter>();

            return services;
        }
    }
}