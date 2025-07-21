using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Models
{
    public class SystemSettings
    {
        public DatabaseSettings Database { get; set; } = new DatabaseSettings();
        public BackupSettings Backup { get; set; } = new BackupSettings();
        public NotificationSettings Notifications { get; set; } = new NotificationSettings();
        public SecuritySettings Security { get; set; } = new SecuritySettings();
        public UISettings UI { get; set; } = new UISettings();
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int CommandTimeout { get; set; } = 30;
        public int MaxPoolSize { get; set; } = 100;
        public bool EnableDetailedLogging { get; set; }
        public string BackupPath { get; set; } = string.Empty;
    }

    public class BackupSettings
    {
        public bool AutoBackupEnabled { get; set; } = true;
        public int BackupIntervalDays { get; set; } = 7;
        public string BackupPath { get; set; } = string.Empty;
        public int MaxBackupCount { get; set; } = 10;
        public bool CompressBackup { get; set; } = true;
        public DateTime LastBackupDate { get; set; }
        public List<string> BackupHistory { get; set; } = new List<string>();
    }

    public class NotificationSettings
    {
        public bool SystemNotificationsEnabled { get; set; } = true;
        public bool EmailNotificationsEnabled { get; set; }
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public bool SmtpUseSsl { get; set; } = true;
        public string AdminEmail { get; set; } = string.Empty;
    }

    public class SecuritySettings
    {
        public int MaxLoginAttempts { get; set; } = 5;
        public int LockoutDurationMinutes { get; set; } = 30;
        public int PasswordExpiryDays { get; set; } = 90;
        public int MinPasswordLength { get; set; } = 8;
        public bool RequireSpecialCharacters { get; set; } = true;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireNumbers { get; set; } = true;
        public int SessionTimeoutMinutes { get; set; } = 30;
        public bool EnableAuditLogging { get; set; } = true;
    }

    public class UISettings
    {
        public string DefaultTheme { get; set; } = "Light";
        public string DefaultLanguage { get; set; } = "VN";
        public bool EnableAnimations { get; set; } = true;
        public double DefaultFontSize { get; set; } = 13;
        public string PrimaryColor { get; set; } = "#4338ca";
        public string SecondaryColor { get; set; } = "#8b5cf6";
    }
}
