using LexiFlow.TranslationAPI.Models;
using LexiFlow.TranslationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace LexiFlow.TranslationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TranslationController : ControllerBase
    {
        private readonly ITranslationService _translationService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<TranslationController> _logger;

        public TranslationController(
            ITranslationService translationService,
            IMemoryCache cache,
            ILogger<TranslationController> logger)
        {
            _translationService = translationService;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Translate text between languages
        /// </summary>
        /// <param name="request">Translation request</param>
        /// <returns>Translation result</returns>
        [HttpPost("translate")]
        [ProducesResponseType(typeof(TranslationResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TranslationResponse>> Translate([FromBody] TranslationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Generate cache key
                var cacheKey = GenerateCacheKey(request);

                // Try get from cache
                if (_cache.TryGetValue(cacheKey, out TranslationResponse? cachedResult))
                {
                    _logger.LogInformation("Cache hit for translation: {Text}", request.Text);
                    return Ok(cachedResult);
                }

                // Perform translation
                var result = await _translationService.TranslateAsync(request);

                // Cache the result for 1 hour
                _cache.Set(cacheKey, result, TimeSpan.FromHours(1));

                _logger.LogInformation("Translation completed: {SourceLang} -> {TargetLang}",
                    request.SourceLang, request.TargetLang);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid translation request: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Translation service unavailable");
                return StatusCode(503, new { error = "Translation service unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during translation");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Get supported languages
        /// </summary>
        /// <returns>Dictionary of supported languages</returns>
        [HttpGet("languages")]
        [ProducesResponseType(typeof(Dictionary<string, string>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Dictionary<string, string>>> GetSupportedLanguages()
        {
            try
            {
                const string cacheKey = "supported_languages";

                if (_cache.TryGetValue(cacheKey, out Dictionary<string, string>? cachedLanguages))
                {
                    return Ok(cachedLanguages);
                }

                var languages = await _translationService.GetSupportedLanguagesAsync();

                // Cache for 24 hours
                _cache.Set(cacheKey, languages, TimeSpan.FromHours(24));

                return Ok(languages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching supported languages");
                return StatusCode(500, new { error = "Could not fetch supported languages" });
            }
        }

        /// <summary>
        /// Check translation service health
        /// </summary>
        /// <returns>Service health status</returns>
        [HttpGet("health")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(503)]
        public async Task<IActionResult> CheckHealth()
        {
            try
            {
                var health = await _translationService.CheckHealthAsync();
                return Ok(health);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(503, new
                {
                    status = "unhealthy",
                    error = "Translation service unavailable"
                });
            }
        }

        /// <summary>
        /// Get translation statistics
        /// </summary>
        /// <returns>API usage statistics</returns>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(object), 200)]
        public IActionResult GetStatistics()
        {
            // This could be expanded to track real statistics
            return Ok(new
            {
                uptime = DateTime.UtcNow.Subtract(Process.GetCurrentProcess().StartTime.ToUniversalTime()),
                version = "1.0.0",
                model = "facebook/m2m100_418M"
            });
        }

        private static string GenerateCacheKey(TranslationRequest request)
        {
            var input = $"{request.Text}|{request.SourceLang}|{request.TargetLang}";
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
    }
}
