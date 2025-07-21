using LexiFlow.AdminDashboard.Models;
using LexiFlow.AdminDashboard.Services;
using LexiFlow.AdminDashboard.ViewModels.Base;
using LexiFlow.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LexiFlow.AdminDashboard.ViewModels
{
    public class UserManagementViewModel : ViewModelBase
    {
        private readonly IAdminDashboardService _dashboardService;
        private readonly ILogger<UserManagementViewModel> _logger;

        private bool _isLoading;
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;
        private ObservableCollection<User> _users = new();
        private ObservableCollection<Role> _roles = new();
        private User? _selectedUser;
        private User _newUser = new();
        private string _newPassword = string.Empty;
        private string _searchText = string.Empty;
        private int _currentPage = 1;
        private int _totalPages = 1;
        private bool _isEditMode;
        private bool _showNewUserDialog;
        private UserManagementStats _stats = new();

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

        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public ObservableCollection<Role> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (SetProperty(ref _selectedUser, value))
                {
                    IsEditMode = value != null;
                }
            }
        }

        public User NewUser
        {
            get => _newUser;
            set => SetProperty(ref _newUser, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    LoadUsersAsync().ConfigureAwait(false);
                }
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public bool ShowNewUserDialog
        {
            get => _showNewUserDialog;
            set => SetProperty(ref _showNewUserDialog, value);
        }

        public UserManagementStats Stats
        {
            get => _stats;
            set => SetProperty(ref _stats, value);
        }

        public ICommand LoadUsersCommand { get; }
        public ICommand CreateUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand ActivateUserCommand { get; }
        public ICommand DeactivateUserCommand { get; }
        public ICommand ResetPasswordCommand { get; }
        public ICommand ShowNewUserDialogCommand { get; }
        public ICommand CancelEditCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public UserManagementViewModel(
            IAdminDashboardService dashboardService,
            ILogger<UserManagementViewModel> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;

            // Initialize commands
            LoadUsersCommand = new AsyncRelayCommand(LoadUsersAsync);
            CreateUserCommand = new AsyncRelayCommand(CreateUserAsync);
            UpdateUserCommand = new AsyncRelayCommand(UpdateUserAsync);
            DeleteUserCommand = new AsyncRelayCommand(DeleteUserAsync);
            ActivateUserCommand = new AsyncRelayCommand(ActivateUserAsync);
            DeactivateUserCommand = new AsyncRelayCommand(DeactivateUserAsync);
            ResetPasswordCommand = new AsyncRelayCommand(ResetPasswordAsync);
            ShowNewUserDialogCommand = new RelayCommand(_ => ShowNewUserDialog = true);
            CancelEditCommand = new RelayCommand(_ =>
            {
                IsEditMode = false;
                SelectedUser = null;
                ShowNewUserDialog = false;
                NewUser = new User();
                NewPassword = string.Empty;
            });
            SearchCommand = new AsyncRelayCommand(SearchUsersAsync);
            NextPageCommand = new RelayCommand(_ => CurrentPage++, _ => CurrentPage < TotalPages);
            PreviousPageCommand = new RelayCommand(_ => CurrentPage--, _ => CurrentPage > 1);
        }

        public async Task InitializeAsync()
        {
            await Task.WhenAll(
                LoadUsersAsync(),
                LoadRolesAsync(),
                LoadUserStatsAsync()
            );
        }

        private async Task LoadUsersAsync(object? parameter = null)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var users = await _dashboardService.GetAllUsersAsync(CurrentPage);

                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }

                // Update pagination
                TotalPages = (int)Math.Ceiling(Users.Count / 20.0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users");
                ErrorMessage = $"Error loading users: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadRolesAsync()
        {
            try
            {
                var roles = await _dashboardService.GetAllRolesAsync();

                Roles.Clear();
                foreach (var role in roles)
                {
                    Roles.Add(role);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading roles");
                ErrorMessage = $"Error loading roles: {ex.Message}";
            }
        }

        private async Task LoadUserStatsAsync()
        {
            try
            {
                Stats = await _dashboardService.GetUserManagementStatsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user statistics");
            }
        }

        private async Task CreateUserAsync(object? parameter = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewUser.Username) ||
                    string.IsNullOrWhiteSpace(NewPassword) ||
                    string.IsNullOrWhiteSpace(NewUser.Email))
                {
                    ErrorMessage = "Please fill in all required fields";
                    return;
                }

                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                NewUser.IsActive = true;
                NewUser.CreatedAt = DateTime.Now;

                bool success = await _dashboardService.RestoreDatabaseAsync(SelectedBackupFile);

                if (success)
                {
                    SuccessMessage = "Database restored successfully";
                }
                else
                {
                    ErrorMessage = "Failed to restore database";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring database");
                ErrorMessage = $"Error restoring database: {ex.Message}";
            }
            finally
            {
                IsRestoring = false;
            }
        }

        private async Task LoadBackupFilesAsync(object? parameter = null)
        {
            try
            {
                var backups = await _dashboardService.GetDatabaseBackupsAsync();

                BackupFiles.Clear();
                foreach (var backup in backups)
                {
                    BackupFiles.Add(backup);
                }

                if (BackupFiles.Count > 0)
                {
                    SelectedBackupFile = BackupFiles[0];
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading backup files");
                ErrorMessage = $"Error loading backup files: {ex.Message}";
            }
        }

        private async Task LoadActivityLogsAsync(object? parameter = null)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var logs = await _dashboardService.GetActivityLogsAsync(
                    ActivityLogsFromDate,
                    ActivityLogsToDate,
                    string.IsNullOrEmpty(ActivityLogsModule) ? null : ActivityLogsModule);

                ActivityLogs.Clear();
                foreach (var log in logs)
                {
                    ActivityLogs.Add(log);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading activity logs");
                ErrorMessage = $"Error loading activity logs: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ClearActivityLogsAsync(object? parameter = null)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                await _dashboardService.ClearActivityLogsAsync(ActivityLogsFromDate);

                SuccessMessage = "Activity logs cleared successfully";

                // Refresh logs
                await LoadActivityLogsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing activity logs");
                ErrorMessage = $"Error clearing activity logs: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ResetSettings()
        {
            Settings = new SystemSettings();
            SuccessMessage = "Settings reset to defaults";
        }


        = await _dashboardService.CreateUserAsync(NewUser, NewPassword);
                
                if (success)
                {
                    SuccessMessage = "User created successfully";
                    ShowNewUserDialog = false;
                    NewUser = new User();
        NewPassword = string.Empty;
                    
                    // Refresh users list
                    await LoadUsersAsync();
        await LoadUserStatsAsync();
    }
                else
                {
                    ErrorMessage = "Failed to create user";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
ErrorMessage = $"Error creating user: {ex.Message}";
            }
            finally
            {
    IsLoading = false;
}
        }

        private async Task UpdateUserAsync(object? parameter = null)
        {
            try
            {
                if (SelectedUser == null)
                    return;

                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.UpdateUserAsync(SelectedUser);

                if (success)
                {
                    SuccessMessage = "User updated successfully";
                    IsEditMode = false;

                    // Refresh users list
                    await LoadUsersAsync();
                }
                else
                {
                    ErrorMessage = "Failed to update user";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                ErrorMessage = $"Error updating user: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteUserAsync(object? parameter = null)
        {
            try
            {
                if (SelectedUser == null)
                    return;

                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.DeleteUserAsync(SelectedUser.Id);

                if (success)
                {
                    SuccessMessage = "User deleted successfully";
                    SelectedUser = null;
                    IsEditMode = false;

                    // Refresh users list
                    await LoadUsersAsync();
                    await LoadUserStatsAsync();
                }
                else
                {
                    ErrorMessage = "Failed to delete user";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                ErrorMessage = $"Error deleting user: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ActivateUserAsync(object? parameter = null)
        {
            try
            {
                if (SelectedUser == null)
                    return;

                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.ActivateUserAsync(SelectedUser.Id);

                if (success)
                {
                    SuccessMessage = "User activated successfully";
                    SelectedUser.IsActive = true;

                    // Refresh user stats
                    await LoadUserStatsAsync();
                }
                else
                {
                    ErrorMessage = "Failed to activate user";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating user");
                ErrorMessage = $"Error activating user: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeactivateUserAsync(object? parameter = null)
        {
            try
            {
                if (SelectedUser == null)
                    return;

                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.DeactivateUserAsync(SelectedUser.Id);

                if (success)
                {
                    SuccessMessage = "User deactivated successfully";
                    SelectedUser.IsActive = false;

                    // Refresh user stats
                    await LoadUserStatsAsync();
                }
                else
                {
                    ErrorMessage = "Failed to deactivate user";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating user");
                ErrorMessage = $"Error deactivating user: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ResetPasswordAsync(object? parameter = null)
        {
            try
            {
                if (SelectedUser == null || string.IsNullOrWhiteSpace(NewPassword))
                {
                    ErrorMessage = "Please enter a new password";
                    return;
                }

                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.ResetUserPasswordAsync(SelectedUser.Id, NewPassword);

                if (success)
                {
                    SuccessMessage = "Password reset successfully";
                    NewPassword = string.Empty;
                }
                else
                {
                    ErrorMessage = "Failed to reset password";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password");
                ErrorMessage = $"Error resetting password: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchUsersAsync(object? parameter = null)
        {
            // Reset pagination
            CurrentPage = 1;

            // Load users with search filter
            await LoadUsersAsync();
        }
    }
}
