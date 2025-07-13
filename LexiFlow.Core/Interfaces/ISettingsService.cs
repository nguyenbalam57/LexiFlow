using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Interfaces
{
    public interface ISettingsService
    {
        // User preferences
        string SavedUsername { get; set; }
        bool RememberMe { get; set; }
        string PreferredLanguage { get; set; }
        DateTime LastLoginDate { get; set; }

        // Window settings
        string ThemeMode { get; set; }
        double WindowWidth { get; set; }
        double WindowHeight { get; set; }
        double WindowLeft { get; set; }
        double WindowTop { get; set; }
        bool IsMaximized { get; set; }
        double FontSize { get; set; }
        bool EnableAnimations { get; set; }

        // Security
        bool AutoLogin { get; set; }
        int LoginAttempts { get; set; }
        DateTime LastFailedLogin { get; set; }

        // Methods
        void SaveSettings();
        void LoadSettings();
        void UpdateFailedLoginAttempts();
        void ResetFailedLoginAttempts();
    }
}
