using LexiFlow.Core.Interfaces;
using LexiFlow.UI.Properties;

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
            get => _settings.PreferredLanguage;
            set => _settings.PreferredLanguage = value;
        }

        public DateTime LastLoginDate
        {
            get => _settings.LastLoginDate;
            set => _settings.LastLoginDate = value;
        }

        // Window settings
        public string ThemeMode
        {
            get => _settings.ThemeMode;
            set => _settings.ThemeMode = value;
        }

        public double WindowWidth
        {
            get => _settings.WindowWidth;
            set => _settings.WindowWidth = value;
        }

        public double WindowHeight
        {
            get => _settings.WindowHeight;
            set => _settings.WindowHeight = value;
        }

        public double WindowLeft
        {
            get => _settings.WindowLeft;
            set => _settings.WindowLeft = value;
        }

        public double WindowTop
        {
            get => _settings.WindowTop;
            set => _settings.WindowTop = value;
        }

        public bool IsMaximized
        {
            get => _settings.IsMaximized;
            set => _settings.IsMaximized = value;
        }

        public double FontSize
        {
            get => _settings.FontSize;
            set => _settings.FontSize = value;
        }

        public bool EnableAnimations
        {
            get => _settings.EnableAnimations;
            set => _settings.EnableAnimations = value;
        }

        // Security
        public bool AutoLogin
        {
            get => _settings.AutoLogin;
            set => _settings.AutoLogin = value;
        }

        public int LoginAttempts
        {
            get => _settings.LoginAttempts;
            set => _settings.LoginAttempts = value;
        }

        public DateTime LastFailedLogin
        {
            get => _settings.LastFailedLogin;
            set => _settings.LastFailedLogin = value;
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
            LoginAttempts++;
            LastFailedLogin = DateTime.Now;
            SaveSettings();
        }

        public void ResetFailedLoginAttempts()
        {
            LoginAttempts = 0;
            SaveSettings();
        }
    }
}
