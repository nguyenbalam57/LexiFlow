namespace LexiFlow.TranslationAPI.Models
{
    public class LanguageInfo
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NativeName { get; set; } = string.Empty;
        public string Family { get; set; } = string.Empty;
        public bool IsRightToLeft { get; set; }
        public string Region { get; set; } = string.Empty;
    }

    public static class SupportedLanguages
    {
        public static readonly Dictionary<string, LanguageInfo> Languages = new()
        {
            ["en"] = new() { Code = "en", Name = "English", NativeName = "English", Family = "Germanic", Region = "Global" },
            ["vi"] = new() { Code = "vi", Name = "Vietnamese", NativeName = "Tiếng Việt", Family = "Austroasiatic", Region = "Southeast Asia" },
            ["zh"] = new() { Code = "zh", Name = "Chinese", NativeName = "中文", Family = "Sino-Tibetan", Region = "East Asia" },
            ["ja"] = new() { Code = "ja", Name = "Japanese", NativeName = "日本語", Family = "Japonic", Region = "East Asia" },
            ["ko"] = new() { Code = "ko", Name = "Korean", NativeName = "한국어", Family = "Koreanic", Region = "East Asia" },
            ["fr"] = new() { Code = "fr", Name = "French", NativeName = "Français", Family = "Romance", Region = "Europe" },
            ["de"] = new() { Code = "de", Name = "German", NativeName = "Deutsch", Family = "Germanic", Region = "Europe" },
            ["es"] = new() { Code = "es", Name = "Spanish", NativeName = "Español", Family = "Romance", Region = "Europe/Americas" },
            ["it"] = new() { Code = "it", Name = "Italian", NativeName = "Italiano", Family = "Romance", Region = "Europe" },
            ["ru"] = new() { Code = "ru", Name = "Russian", NativeName = "Русский", Family = "Slavic", Region = "Eastern Europe" },
            ["ar"] = new() { Code = "ar", Name = "Arabic", NativeName = "العربية", Family = "Semitic", Region = "Middle East", IsRightToLeft = true },
            ["hi"] = new() { Code = "hi", Name = "Hindi", NativeName = "हिन्दी", Family = "Indo-Aryan", Region = "South Asia" },
            ["th"] = new() { Code = "th", Name = "Thai", NativeName = "ไทย", Family = "Tai-Kadai", Region = "Southeast Asia" },
            ["pt"] = new() { Code = "pt", Name = "Portuguese", NativeName = "Português", Family = "Romance", Region = "Europe/Americas" },
            ["nl"] = new() { Code = "nl", Name = "Dutch", NativeName = "Nederlands", Family = "Germanic", Region = "Europe" },
            ["pl"] = new() { Code = "pl", Name = "Polish", NativeName = "Polski", Family = "Slavic", Region = "Europe" },
            ["tr"] = new() { Code = "tr", Name = "Turkish", NativeName = "Türkçe", Family = "Turkic", Region = "Western Asia" },
            ["he"] = new() { Code = "he", Name = "Hebrew", NativeName = "עברית", Family = "Semitic", Region = "Middle East", IsRightToLeft = true }
        };

        public static LanguageInfo? GetLanguage(string code)
        {
            return Languages.TryGetValue(code, out var language) ? language : null;
        }

        public static bool IsSupported(string code)
        {
            return Languages.ContainsKey(code);
        }

        public static IEnumerable<LanguageInfo> GetLanguagesByRegion(string region)
        {
            return Languages.Values.Where(l => l.Region.Contains(region, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<LanguageInfo> GetLanguagesByFamily(string family)
        {
            return Languages.Values.Where(l => l.Family.Equals(family, StringComparison.OrdinalIgnoreCase));
        }
    }
}
