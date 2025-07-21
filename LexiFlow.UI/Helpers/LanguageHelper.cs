using System.Globalization;
using System.Windows;

namespace LexiFlow.UI.Helpers
{
    public static class LanguageHelper
    {
        private static string _currentLanguage = "VN";
        private static readonly Dictionary<string, CultureInfo> _supportedCultures = new()
        {
            { "VN", new CultureInfo("vi-VN") },
            { "EN", new CultureInfo("en-US") },
            { "JP", new CultureInfo("ja-JP") }
        };

        public static string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value && _supportedCultures.ContainsKey(value))
                {
                    _currentLanguage = value;
                    ChangeLanguage(value);
                    OnLanguageChanged?.Invoke(value);
                }
            }
        }

        public static event Action<string>? OnLanguageChanged;

        public static IEnumerable<string> SupportedLanguages => _supportedCultures.Keys;

        public static void ChangeLanguage(string language)
        {
            try
            {
                if (!_supportedCultures.ContainsKey(language))
                {
                    throw new ArgumentException($"Language '{language}' is not supported.");
                }

                // Update application culture
                var culture = _supportedCultures[language];
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                // Update WPF language
                FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(System.Windows.Markup.XmlLanguage.GetLanguage(culture.IetfLanguageTag)));

                // Update resource dictionaries
                UpdateResourceDictionaries(language);

                // Save language preference
                SaveLanguagePreference(language);

                _currentLanguage = language;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error changing language: {ex.Message}");
                throw;
            }
        }

        private static void UpdateResourceDictionaries(string language)
        {
            var app = System.Windows.Application.Current;
            if (app?.Resources == null) return;

            var resourceDictionaries = app.Resources.MergedDictionaries;

            // Find and remove existing language dictionary
            var existingLangDict = resourceDictionaries
                .Where(rd => rd.Source?.OriginalString?.Contains("Languages_") == true)
                .ToList();

            foreach (var dict in existingLangDict)
            {
                resourceDictionaries.Remove(dict);
            }

            // Add new language dictionary
            try
            {
                var newLangDict = new ResourceDictionary
                {
                    Source = new Uri($"/LexiFlow.UI;component/Resources/Languages/Languages_{language}.xaml",
                                   UriKind.RelativeOrAbsolute)
                };
                resourceDictionaries.Add(newLangDict);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading language resource: {ex.Message}");

                // Fallback to default language if the specific language file is not found
                if (language != "VN")
                {
                    try
                    {
                        var fallbackDict = new ResourceDictionary
                        {
                            Source = new Uri("/LexiFlow.UI;component/Resources/Languages/Languages_VN.xaml",
                                           UriKind.RelativeOrAbsolute)
                        };
                        resourceDictionaries.Add(fallbackDict);
                    }
                    catch (Exception fallbackEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error loading fallback language: {fallbackEx.Message}");
                    }
                }
            }
        }

        public static string GetLocalizedString(string key, params object[] args)
        {
            try
            {
                var app = System.Windows.Application.Current;
                if (app?.Resources != null && app.Resources.Contains(key))
                {
                    var value = app.Resources[key]?.ToString() ?? key;

                    // Handle string formatting if arguments are provided
                    if (args != null && args.Length > 0)
                    {
                        return string.Format(value, args);
                    }

                    return value;
                }

                return key; // Return the key itself if not found
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting localized string for key '{key}': {ex.Message}");
                return key;
            }
        }

        public static string GetLanguageDisplayName(string languageCode)
        {
            return languageCode switch
            {
                "VN" => "Tiếng Việt",
                "EN" => "English",
                "JP" => "日本語",
                _ => languageCode
            };
        }

        public static string GetLanguageNativeName(string languageCode)
        {
            if (_supportedCultures.TryGetValue(languageCode, out var culture))
            {
                return culture.NativeName;
            }
            return languageCode;
        }

        public static CultureInfo GetCultureInfo(string languageCode)
        {
            return _supportedCultures.TryGetValue(languageCode, out var culture)
                ? culture
                : CultureInfo.InvariantCulture;
        }

        public static void InitializeLanguage()
        {
            try
            {
                // Load saved language preference
                var savedLanguage = LoadLanguagePreference();

                if (!string.IsNullOrEmpty(savedLanguage) && _supportedCultures.ContainsKey(savedLanguage))
                {
                    CurrentLanguage = savedLanguage;
                }
                else
                {
                    // Detect system language and set appropriate default
                    var systemLanguage = DetectSystemLanguage();
                    CurrentLanguage = systemLanguage;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing language: {ex.Message}");
                // Fallback to Vietnamese
                CurrentLanguage = "VN";
            }
        }

        private static string DetectSystemLanguage()
        {
            try
            {
                var systemCulture = CultureInfo.CurrentUICulture;

                // Map system cultures to supported languages
                return systemCulture.TwoLetterISOLanguageName.ToUpper() switch
                {
                    "VI" => "VN",
                    "EN" => "EN",
                    "JA" => "JP",
                    _ => "VN" // Default to Vietnamese
                };
            }
            catch
            {
                return "VN"; // Default fallback
            }
        }

        private static void SaveLanguagePreference(string language)
        {
            try
            {
                Properties.Settings.Default.SelectedLanguage = language;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving language preference: {ex.Message}");
            }
        }

        private static string LoadLanguagePreference()
        {
            try
            {
                return Properties.Settings.Default.SelectedLanguage ?? "VN";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading language preference: {ex.Message}");
                return "VN";
            }
        }

        public static bool IsRightToLeft(string languageCode)
        {
            // Currently none of the supported languages are RTL
            // This can be extended for Arabic, Hebrew, etc. in the future
            return false;
        }

        public static string FormatDate(DateTime date, string languageCode)
        {
            try
            {
                var culture = GetCultureInfo(languageCode);
                return date.ToString("d", culture);
            }
            catch
            {
                return date.ToString("yyyy-MM-dd");
            }
        }

        public static string FormatTime(DateTime time, string languageCode)
        {
            try
            {
                var culture = GetCultureInfo(languageCode);
                return time.ToString("t", culture);
            }
            catch
            {
                return time.ToString("HH:mm");
            }
        }

        public static string FormatDateTime(DateTime dateTime, string languageCode)
        {
            try
            {
                var culture = GetCultureInfo(languageCode);
                return dateTime.ToString("F", culture);
            }
            catch
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public static string FormatNumber(double number, string languageCode)
        {
            try
            {
                var culture = GetCultureInfo(languageCode);
                return number.ToString("N", culture);
            }
            catch
            {
                return number.ToString();
            }
        }

        public static string FormatCurrency(decimal amount, string languageCode)
        {
            try
            {
                var culture = GetCultureInfo(languageCode);
                return amount.ToString("C", culture);
            }
            catch
            {
                return amount.ToString();
            }
        }
    }
}
