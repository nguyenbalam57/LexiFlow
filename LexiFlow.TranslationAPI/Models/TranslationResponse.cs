namespace LexiFlow.TranslationAPI.Models
{
    public class TranslationResponse
    {
        public bool Success { get; set; }
        public string TranslatedText { get; set; } = string.Empty;
        public string SourceLang { get; set; } = string.Empty;
        public string TargetLang { get; set; } = string.Empty;
        public string OriginalText { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
