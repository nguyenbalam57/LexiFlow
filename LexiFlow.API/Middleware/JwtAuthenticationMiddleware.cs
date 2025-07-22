using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.API.Middleware
{
    /// <summary>
    /// Middleware for handling JWT authentication
    /// </summary>
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtAuthenticationMiddleware> _logger;
        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        public JwtAuthenticationMiddleware(
            RequestDelegate next,
            ILogger<JwtAuthenticationMiddleware> logger,
            IOptions<JwtOptions> jwtOptions)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        /// <summary>
        /// Invokes the middleware
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the endpoint requires authentication
            var endpoint = context.GetEndpoint();

            // Skip authentication for endpoints with AllowAnonymous attribute
            if (endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>() != null)
            {
                await _next(context);
                return;
            }

            // Check for the Authorization header
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                // If no token is provided, continue to the next middleware
                // The authentication handler will handle unauthorized access
                await _next(context);
                return;
            }

            // Extract token
            var token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                // Validate the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Validate and parse the token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Set the user claims
                context.User = principal;

                // Log successful authentication
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation("User {UserId} successfully authenticated", userId);
                }
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogWarning("Token expired: {Token}", token);
                // Let the authentication handler handle the expired token
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Invalid token: {Token}", token);
                // Let the authentication handler handle the invalid token
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token: {Token}", token);
                // Let the authentication handler handle the error
            }

            // Continue to the next middleware
            await _next(context);
        }
    }

    /// <summary>
    /// JWT configuration options
    /// </summary>
    public class JwtOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; } = 60;
    }
}