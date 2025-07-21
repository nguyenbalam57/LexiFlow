using LexiFlow.AdminDashboard.Models;
using LexiFlow.AdminDashboard.Services;
using LexiFlow.AdminDashboard.ViewModels.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LexiFlow.AdminDashboard.ViewModels
{
    public class SystemConfigViewModel : ViewModelBase
    {
        private readonly IAdminDashboardService _dashboardService;
        private readonly AdminConfigService _configService;
        private readonly ILogger<SystemConfigViewModel> _logger;

        private bool _isLoading;
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;
        private SystemSettings _settings = new();
        private string _selectedBackupFile = string.Empty;
        private ObservableCollection<string> _backupFiles = new();
        private DateTime _activityLogsFromDate = DateTime.Now.AddDays(-7);
        private DateTime _activityLogsToDate = DateTime.Now;
        private string _activityLogsModule = string.Empty;
        private ObservableCollection<string> _activityLogs = new();
        private bool _isTestingEmail;
        private bool _isTestingDatabase;
        private bool _isBackingUp;
        private bool _isRestoring;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set => SetProperty(ref _successMessage, value);
        }

        public SystemSettings Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        public string SelectedBackupFile
        {
            get => _selectedBackupFile;
            set => SetProperty(ref _selectedBackupFile, value);
        }

        public ObservableCollection<string> BackupFiles
        {
            get => _backupFiles;
            set => SetProperty(ref _backupFiles, value);
        }

        public DateTime ActivityLogsFromDate
        {
            get => _activityLogsFromDate;
            set => SetProperty(ref _activityLogsFromDate, value);
        }

        public DateTime ActivityLogsToDate
        {
            get => _activityLogsToDate;
            set => SetProperty(ref _activityLogsToDate, value);
        }

        public string ActivityLogsModule
        {
            get => _activityLogsModule;
            set => SetProperty(ref _activityLogsModule, value);
        }

        public ObservableCollection<string> ActivityLogs
        {
            get => _activityLogs;
            set => SetProperty(ref _activityLogs, value);
        }

        public bool IsTestingEmail
        {
            get => _isTestingEmail;
            set => SetProperty(ref _isTestingEmail, value);
        }

        public bool IsTestingDatabase
        {
            get => _isTestingDatabase;
            set => SetProperty(ref _isTestingDatabase, value);
        }

        public bool IsBackingUp
        {
            get => _isBackingUp;
            set => SetProperty(ref _isBackingUp, value);
        }

        public bool IsRestoring
        {
            get => _isRestoring;
            set => SetProperty(ref _isRestoring, value);
        }

        public ICommand LoadSettingsCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand TestEmailCommand { get; }
        public ICommand TestDatabaseCommand { get; }
        public ICommand BackupDatabaseCommand { get; }
        public ICommand RestoreDatabaseCommand { get; }
        public ICommand LoadBackupFilesCommand { get; }
        public ICommand LoadActivityLogsCommand { get; }
        public ICommand ClearActivityLogsCommand { get; }
        public ICommand ResetSettingsCommand { get; }

        public SystemConfigViewModel(
            IAdminDashboardService dashboardService,
            AdminConfigService configService,
            ILogger<SystemConfigViewModel> logger)
        {
            _dashboardService = dashboardService;
            _configService = configService;
            _logger = logger;

            // Initialize commands
            LoadSettingsCommand = new AsyncRelayCommand(LoadSettingsAsync);
            SaveSettingsCommand = new AsyncRelayCommand(SaveSettingsAsync);
            TestEmailCommand = new AsyncRelayCommand(TestEmailAsync);
            TestDatabaseCommand = new AsyncRelayCommand(TestDatabaseAsync);
            BackupDatabaseCommand = new AsyncRelayCommand(BackupDatabaseAsync);
            RestoreDatabaseCommand = new AsyncRelayCommand(RestoreDatabaseAsync);
            LoadBackupFilesCommand = new AsyncRelayCommand(LoadBackupFilesAsync);
            LoadActivityLogsCommand = new AsyncRelayCommand(LoadActivityLogsAsync);
            ClearActivityLogsCommand = new AsyncRelayCommand(ClearActivityLogsAsync);
            ResetSettingsCommand = new RelayCommand(_ => ResetSettings());
        }

        public async Task InitializeAsync()
        {
            await Task.WhenAll(
                LoadSettingsAsync(),
                LoadBackupFilesAsync()
            );
        }

        private async Task LoadSettingsAsync(object? parameter = null)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                Settings = await _dashboardService.GetSystemSettingsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading system settings");
                ErrorMessage = $"Error loading system settings: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveSettingsAsync(object? parameter = null)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.UpdateSystemSettingsAsync(Settings);

                if (success)
                {
                    SuccessMessage = "Settings saved successfully";

                    // Also save to local config
                    await _configService.SaveConfigAsync(Settings);
                }
                else
                {
                    ErrorMessage = "Failed to save settings";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving system settings");
                ErrorMessage = $"Error saving system settings: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task TestEmailAsync(object? parameter = null)
        {
            try
            {
                IsTestingEmail = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.TestEmailSettingsAsync(
                    Settings.Notifications.SmtpServer,
                    Settings.Notifications.SmtpPort,
                    Settings.Notifications.SmtpUsername,
                    Settings.Notifications.SmtpPassword,
                    Settings.Notifications.SmtpUseSsl);

                if (success)
                {
                    SuccessMessage = "Email settings tested successfully";
                }
                else
                {
                    ErrorMessage = "Email test failed. Please check your settings.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing email settings");
                ErrorMessage = $"Error testing email settings: {ex.Message}";
            }
            finally
            {
                IsTestingEmail = false;
            }
        }

        private async Task TestDatabaseAsync(object? parameter = null)
        {
            try
            {
                IsTestingDatabase = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.TestDatabaseConnectionAsync(
                    Settings.Database.ConnectionString);

                if (success)
                {
                    SuccessMessage = "Database connection tested successfully";
                }
                else
                {
                    ErrorMessage = "Database connection test failed. Please check your settings.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing database connection");
                ErrorMessage = $"Error testing database connection: {ex.Message}";
            }
            finally
            {
                IsTestingDatabase = false;
            }
        }

        private async Task BackupDatabaseAsync(object? parameter = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.Backup.BackupPath))
                {
                    ErrorMessage = "Please set a backup path first";
                    return;
                }

                IsBackingUp = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                // Generate backup file name
                string backupFileName = $"LexiFlow_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string backupPath = Path.Combine(Settings.Backup.BackupPath, backupFileName);

                // Ensure directory exists
                Directory.CreateDirectory(Settings.Backup.BackupPath);

                bool success = await _dashboardService.BackupDatabaseAsync(backupPath);

                if (success)
                {
                    SuccessMessage = "Database backed up successfully";
                    Settings.Backup.LastBackupDate = DateTime.Now;

                    // Refresh backup files list
                    await LoadBackupFilesAsync();
                }
                else
                {
                    ErrorMessage = "Failed to backup database";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error backing up database");
                ErrorMessage = $"Error backing up database: {ex.Message}";
            }
            finally
            {
                IsBackingUp = false;
            }
        }
        private async Task RestoreDatabaseAsync(object? parameter = null)
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedBackupFile))
                {
                    ErrorMessage = "Please select a backup file first";
                    return;
                }

                IsRestoring = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success
    }
