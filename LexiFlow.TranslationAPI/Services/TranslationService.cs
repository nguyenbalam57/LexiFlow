using LexiFlow.TranslationAPI.Configuration;
using LexiFlow.TranslationAPI.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LexiFlow.TranslationAPI.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly TranslationServiceOptions _options;
        private readonly ILogger<TranslationService> _logger;

        public TranslationService(
            HttpClient httpClient,
            IOptions<TranslationServiceOptions> options,
            ILogger<TranslationService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
        }

        public async Task<TranslationResponse> TranslateAsync(TranslationRequest request)
        {
            ValidateRequest(request);

            var payload = new
            {
                text = request.Text,
                source_lang = request.SourceLang,
                target_lang = request.TargetLang
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending translation request: {SourceLang} -> {TargetLang}",
                request.SourceLang, request.TargetLang);

            var response = await _httpClient.PostAsync("/translate", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Translation API error: {StatusCode} - {Error}",
                    response.StatusCode, errorContent);

                throw new HttpRequestException($"Translation service error: {response.StatusCode}");
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TranslationResponse>(resultJson, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return result ?? throw new InvalidOperationException("Invalid response from translation service");
        }

        public async Task<Dictionary<string, string>> GetSupportedLanguagesAsync()
        {
            var response = await _httpClient.GetAsync("/languages");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var languages = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            return languages ?? new Dictionary<string, string>();
        }

        public async Task<object> CheckHealthAsync()
        {
            var response = await _httpClient.GetAsync("/health");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var health = JsonSerializer.Deserialize<object>(json);

            return health ?? new { status = "unknown" };
        }

        private static void ValidateRequest(TranslationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
                throw new ArgumentException("Text is required");

            if (request.Text.Length > 1000)
                throw new ArgumentException("Text too long (max 1000 characters)");

            if (string.IsNullOrWhiteSpace(request.SourceLang))
                throw new ArgumentException("Source language is required");

            if (string.IsNullOrWhiteSpace(request.TargetLang))
                throw new ArgumentException("Target language is required");
        }
    }
}
