using LexiFlow.Core.Interfaces;
using LexiFlow.UI.Properties;
using System;

namespace LexiFlow.UI.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly Settings _settings;

        public SettingsService()
        {
            _settings = Settings.Default;
            LoadSettings();
        }

        // User preferences
        public string SavedUsername
        {
            get => _settings.SavedUsername;
            set => _settings.SavedUsername = value;
        }

        public bool RememberMe
        {
            get => _settings.RememberMe;
            set => _settings.RememberMe = value;
        }

        public string PreferredLanguage
        {
            get => _settings.SelectedLanguage;
            set => _settings.SelectedLanguage = value;
        }

        // Theme settings
        public string ThemeMode
        {
            get => _settings.Theme;
            set => _settings.Theme = value;
        }

        // Security
        public bool AutoLogin
        {
            get => _settings.AutoLogin;
            set => _settings.AutoLogin = value;
        }

        public DateTime LastSyncTime
        {
            get => _settings.LastSyncTime;
            set => _settings.LastSyncTime = value;
        }

        public string AccessToken
        {
            get => _settings.AccessToken;
            set => _settings.AccessToken = value;
        }

        public void SaveSettings()
        {
            try
            {
                _settings.Save();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        public void LoadSettings()
        {
            try
            {
                _settings.Reload();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading settings: {ex.Message}");
            }
        }

        public void UpdateFailedLoginAttempts()
        {
            // Note: LoginAttempts is not in the Settings file
            // Implement alternative approach or add this setting to the file
            SaveSettings();
        }

        public void ResetFailedLoginAttempts()
        {
            // Note: LoginAttempts is not in the Settings file
            // Implement alternative approach or add this setting to the file
            SaveSettings();
        }
    }
}