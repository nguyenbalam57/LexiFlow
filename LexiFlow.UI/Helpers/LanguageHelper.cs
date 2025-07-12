using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LexiFlow.UI.Helpers
{
    public static class LanguageHelper
    {
        // Default language is Vietnamese
        private static string _currentLanguage = "VN";

        public static string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    ChangeLanguage(value);
                }
            }
        }

        public static void ChangeLanguage(string language)
        {
            // Get all resource dictionaries from the application
            var resourceDictionaries = Application.Current.Resources.MergedDictionaries;

            // Find the language resource dictionary
            ResourceDictionary? languageDict = null;
            int langDictIndex = -1;

            for (int i = 0; i < resourceDictionaries.Count; i++)
            {
                var dictionary = resourceDictionaries[i];
                if (dictionary.Source != null && dictionary.Source.OriginalString.Contains("Languages_"))
                {
                    languageDict = dictionary;
                    langDictIndex = i;
                    break;
                }
            }

            // Create new resource dictionary with the selected language
            string packUri = $"/LexiFlow.UI;component/Resources/Languages/Languages_{language}.xaml";
            var newLangDict = new ResourceDictionary
            {
                Source = new Uri(packUri, UriKind.RelativeOrAbsolute)
            };

            // Replace or add the new dictionary
            if (langDictIndex >= 0)
            {
                resourceDictionaries[langDictIndex] = newLangDict;
            }
            else
            {
                resourceDictionaries.Add(newLangDict);
            }

            // Save the current language setting
            _currentLanguage = language;
        }
    }
}
