using System;

namespace LexiFlow.Core.Interfaces
{
    public interface ISettingsService
    {
        // User preferences
        string SavedUsername { get; set; }
        bool RememberMe { get; set; }
        string PreferredLanguage { get; set; }

        // Theme settings
        string ThemeMode { get; set; }

        // Security
        bool AutoLogin { get; set; }

        // Synchronization
        DateTime LastSyncTime { get; set; }
        string AccessToken { get; set; }

        // Methods
        void SaveSettings();
        void LoadSettings();
    }
}