using LexiFlow.TranslationAPI.Models;

namespace LexiFlow.TranslationAPI.Services
{
    public interface ITranslationService
    {
        Task<TranslationResponse> TranslateAsync(TranslationRequest request);
        Task<Dictionary<string, string>> GetSupportedLanguagesAsync();
        Task<object> CheckHealthAsync();
    }
}
