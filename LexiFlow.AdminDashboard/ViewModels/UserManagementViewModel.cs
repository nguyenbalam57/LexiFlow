using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LexiFlow.AdminDashboard.Models;
using LexiFlow.AdminDashboard.Services;
using LexiFlow.Core.Entities;
using Microsoft.Extensions.Logging;

namespace LexiFlow.AdminDashboard.ViewModels
{
    /// <summary>
    /// View model for user management
    /// </summary>
    public class UserManagementViewModel : ViewModelBase
    {
        private readonly IAdminDashboardService _dashboardService;
        private readonly ILogger<UserManagementViewModel> _logger;

        private ObservableCollection<User> _users = new ObservableCollection<User>();
        private ObservableCollection<Role> _roles = new ObservableCollection<Role>();
        private User? _selectedUser;
        private Role? _selectedRole;
        private UserManagementStats _stats = new UserManagementStats();
        private int _currentPage = 1;
        private int _pageSize = 20;
        private int _totalPages = 1;
        private int _totalUsers = 0;
        private string _searchText = string.Empty;
        private bool _showActiveOnly = false;
        private bool _isEditing = false;
        private bool _isCreating = false;
        private string _newPassword = string.Empty;
        private User _editUser = new User();
        private string _confirmPassword = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserManagementViewModel(
            IAdminDashboardService dashboardService,
            ILogger<UserManagementViewModel> logger)
        {
            _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Initialize commands
            RefreshCommand = CreateCommand(RefreshAsync);
            NextPageCommand = CreateCommand(NextPageAsync, CanGoToNextPage);
            PreviousPageCommand = CreateCommand(PreviousPageAsync, CanGoToPreviousPage);
            SearchCommand = CreateCommand(SearchAsync);
            CreateUserCommand = CreateCommand(CreateUserAsync);
            EditUserCommand = CreateCommand(EditUserAsync, CanEditUser);
            SaveUserCommand = CreateCommand(SaveUserAsync, CanSaveUser);
            CancelEditCommand = CreateCommand(CancelEditAsync);
            DeleteUserCommand = CreateCommand(DeleteUserAsync, CanDeleteUser);
            ActivateUserCommand = CreateCommand(ActivateUserAsync, CanActivateUser);
            DeactivateUserCommand = CreateCommand(DeactivateUserAsync, CanDeactivateUser);
            ResetPasswordCommand = CreateCommand(ResetPasswordAsync, CanResetPassword);
            AssignRoleCommand = CreateCommand(AssignRoleAsync, CanAssignRole);
        }

        /// <summary>
        /// Users collection
        /// </summary>
        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        /// <summary>
        /// Roles collection
        /// </summary>
        public ObservableCollection<Role> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }

        /// <summary>
        /// Selected user
        /// </summary>
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (SetProperty(ref _selectedUser, value))
                {
                    if (value != null)
                    {
                        // Copy user for editing
                        EditUser = new User
                        {
                            Id = value.Id,
                            Username = value.Username,
                            Email = value.Email,
                            FirstName = value.FirstName,
                            LastName = value.LastName,
                            IsActive = value.IsActive,
                            RoleIds = new List<int>(value.RoleIds),
                            RoleNames = new List<string>(value.RoleNames)
                        };
                    }

                    // Update commands
                    (EditUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeleteUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (ActivateUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (DeactivateUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (ResetPasswordCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (AssignRoleCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Selected role
        /// </summary>
        public Role? SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (SetProperty(ref _selectedRole, value))
                {
                    // Update commands
                    (AssignRoleCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// User management statistics
        /// </summary>
        public UserManagementStats Stats
        {
            get => _stats;
            set => SetProperty(ref _stats, value);
        }

        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    // Update commands
                    (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => SetProperty(ref _pageSize, value);
        }

        /// <summary>
        /// Total pages
        /// </summary>
        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value);
        }

        /// <summary>
        /// Total users
        /// </summary>
        public int TotalUsers
        {
            get => _totalUsers;
            set => SetProperty(ref _totalUsers, value);
        }

        /// <summary>
        /// Search text
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        /// <summary>
        /// Show active users only
        /// </summary>
        public bool ShowActiveOnly
        {
            get => _showActiveOnly;
            set => SetProperty(ref _showActiveOnly, value);
        }

        /// <summary>
        /// Flag indicating if the view model is in edit mode
        /// </summary>
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (SetProperty(ref _isEditing, value))
                {
                    // Update commands
                    (SaveUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Flag indicating if the view model is in create mode
        /// </summary>
        public bool IsCreating
        {
            get => _isCreating;
            set
            {
                if (SetProperty(ref _isCreating, value))
                {
                    // Update commands
                    (SaveUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// User being edited
        /// </summary>
        public User EditUser
        {
            get => _editUser;
            set => SetProperty(ref _editUser, value);
        }

        /// <summary>
        /// New password
        /// </summary>
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        /// <summary>
        /// Confirm password
        /// </summary>
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        /// <summary>
        /// Refresh command
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Next page command
        /// </summary>
        public ICommand NextPageCommand { get; }

        /// <summary>
        /// Previous page command
        /// </summary>
        public ICommand PreviousPageCommand { get; }

        /// <summary>
        /// Search command
        /// </summary>
        public ICommand SearchCommand { get; }

        /// <summary>
        /// Create user command
        /// </summary>
        public ICommand CreateUserCommand { get; }

        /// <summary>
        /// Edit user command
        /// </summary>
        public ICommand EditUserCommand { get; }

        /// <summary>
        /// Save user command
        /// </summary>
        public ICommand SaveUserCommand { get; }

        /// <summary>
        /// Cancel edit command
        /// </summary>
        public ICommand CancelEditCommand { get; }

        /// <summary>
        /// Delete user command
        /// </summary>
        public ICommand DeleteUserCommand { get; }

        /// <summary>
        /// Activate user command
        /// </summary>
        public ICommand ActivateUserCommand { get; }

        /// <summary>
        /// Deactivate user command
        /// </summary>
        public ICommand DeactivateUserCommand { get; }

        /// <summary>
        /// Reset password command
        /// </summary>
        public ICommand ResetPasswordCommand { get; }

        /// <summary>
        /// Assign role command
        /// </summary>
        public ICommand AssignRoleCommand { get; }

        /// <summary>
        /// Loads the view model data
        /// </summary>
        protected override async Task LoadDataAsync(object? parameter = null)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Load roles
                await LoadRolesAsync();

                // Load users
                await LoadUsersAsync();

                // Load user stats
                await LoadUserStatsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user management data");
                ErrorMessage = $"Error loading user management data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Loads the roles
        /// </summary>
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
                throw;
            }
        }

        /// <summary>
        /// Loads the users
        /// </summary>
        private async Task LoadUsersAsync()
        {
            try
            {
                // Apply filters
                var filter = ShowActiveOnly ? (Func<User, bool>)(u => u.IsActive) : null;

                // Load users
                var users = await _dashboardService.GetAllUsersAsync(CurrentPage, PageSize);

                // Apply filter if needed
                if (filter != null)
                {
                    users = users.Where(filter).ToList();
                }

                // Update users collection
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }

                // Update pagination info
                TotalUsers = users.Count;
                TotalPages = (int)Math.Ceiling(TotalUsers / (double)PageSize);

                // Ensure current page is valid
                if (CurrentPage > TotalPages && TotalPages > 0)
                {
                    CurrentPage = TotalPages;
                }

                // Update commands
                (NextPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (PreviousPageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users");
                throw;
            }
        }

        /// <summary>
        /// Loads the user statistics
        /// </summary>
        private async Task LoadUserStatsAsync()
        {
            try
            {
                Stats = await _dashboardService.GetUserManagementStatsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user statistics");
                throw;
            }
        }

        /// <summary>
        /// Refreshes the user management data
        /// </summary>
        private async Task RefreshAsync(object? parameter = null)
        {
            await LoadAsync();
        }

        /// <summary>
        /// Goes to the next page
        /// </summary>
        private async Task NextPageAsync(object? parameter = null)
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                await LoadUsersAsync();
            }
        }

        /// <summary>
        /// Determines if the next page command can execute
        /// </summary>
        private bool CanGoToNextPage(object? parameter = null)
        {
            return CurrentPage < TotalPages;
        }

        /// <summary>
        /// Goes to the previous page
        /// </summary>
        private async Task PreviousPageAsync(object? parameter = null)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadUsersAsync();
            }
        }

        /// <summary>
        /// Determines if the previous page command can execute
        /// </summary>
        private bool CanGoToPreviousPage(object? parameter = null)
        {
            return CurrentPage > 1;
        }

        /// <summary>
        /// Searches for users
        /// </summary>
        private async Task SearchAsync(object? parameter = null)
        {
            // Reset page
            CurrentPage = 1;

            // Load users
            await LoadUsersAsync();
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        private async Task CreateUserAsync(object? parameter = null)
        {
            // Reset edit user
            EditUser = new User
            {
                IsActive = true
            };

            // Reset passwords
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;

            // Enter create mode
            IsCreating = true;
            IsEditing = false;
        }

        /// <summary>
        /// Edits the selected user
        /// </summary>
        private async Task EditUserAsync(object? parameter = null)
        {
            if (SelectedUser == null)
            {
                return;
            }

            // Enter edit mode
            IsEditing = true;
            IsCreating = false;

            // Reset passwords
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        /// <summary>
        /// Determines if the edit user command can execute
        /// </summary>
        private bool CanEditUser(object? parameter = null)
        {
            return SelectedUser != null;
        }

        /// <summary>
        /// Saves the user
        /// </summary>
        private async Task SaveUserAsync(object? parameter = null)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                if (IsCreating)
                {
                    // Validate passwords
                    if (string.IsNullOrEmpty(NewPassword))
                    {
                        ErrorMessage = "Please enter a password";
                        return;
                    }

                    if (NewPassword != ConfirmPassword)
                    {
                        ErrorMessage = "Passwords do not match";
                        return;
                    }

                    // Create user
                    bool success = await _dashboardService.CreateUserAsync(EditUser, NewPassword);

                    if (success)
                    {
                        SuccessMessage = "User created successfully";

                        // Exit create mode
                        IsCreating = false;

                        // Refresh users
                        await LoadUsersAsync();

                        // Refresh user stats
                        await LoadUserStatsAsync();
                    }
                    else
                    {
                        ErrorMessage = "Failed to create user";
                    }
                }
                else if (IsEditing)
                {
                    // Update user
                    bool success = await _dashboardService.UpdateUserAsync(EditUser);

                    if (success)
                    {
                        SuccessMessage = "User updated successfully";

                        // Exit edit mode
                        IsEditing = false;

                        // Refresh users
                        await LoadUsersAsync();
                    }
                    else
                    {
                        ErrorMessage = "Failed to update user";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving user");
                ErrorMessage = $"Error saving user: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Determines if the save user command can execute
        /// </summary>
        private bool CanSaveUser(object? parameter = null)
        {
            return IsEditing || IsCreating;
        }

        /// <summary>
        /// Cancels the edit
        /// </summary>
        private async Task CancelEditAsync(object? parameter = null)
        {
            // Exit edit mode
            IsEditing = false;
            IsCreating = false;

            // Reset edit user
            if (SelectedUser != null)
            {
                EditUser = new User
                {
                    Id = SelectedUser.Id,
                    Username = SelectedUser.Username,
                    Email = SelectedUser.Email,
                    FirstName = SelectedUser.FirstName,
                    LastName = SelectedUser.LastName,
                    IsActive = SelectedUser.IsActive,
                    RoleIds = new List<int>(SelectedUser.RoleIds),
                    RoleNames = new List<string>(SelectedUser.RoleNames)
                };
            }
            else
            {
                EditUser = new User();
            }

            // Reset passwords
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        /// <summary>
        /// Deletes the selected user
        /// </summary>
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

                    // Refresh users
                    await LoadUsersAsync();

                    // Refresh user stats
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

        /// <summary>
        /// Determines if the delete user command can execute
        /// </summary>
        private bool CanDeleteUser(object? parameter = null)
        {
            return SelectedUser != null;
        }

        /// <summary>
        /// Activates the selected user
        /// </summary>
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

        /// <summary>
        /// Determines if the activate user command can execute
        /// </summary>
        private bool CanActivateUser(object? parameter = null)
        {
            return SelectedUser != null && !SelectedUser.IsActive;
        }

        /// <summary>
        /// Deactivates the selected user
        /// </summary>
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

        /// <summary>
        /// Determines if the deactivate user command can execute
        /// </summary>
        private bool CanDeactivateUser(object? parameter = null)
        {
            return SelectedUser != null && SelectedUser.IsActive;
        }

        /// <summary>
        /// Resets the selected user's password
        /// </summary>
        private async Task ResetPasswordAsync(object? parameter = null)
        {
            try
            {
                if (SelectedUser == null)
                    return;

                // Validate passwords
                if (string.IsNullOrEmpty(NewPassword))
                {
                    ErrorMessage = "Please enter a new password";
                    return;
                }

                if (NewPassword != ConfirmPassword)
                {
                    ErrorMessage = "Passwords do not match";
                    return;
                }

                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.ResetUserPasswordAsync(SelectedUser.Id, NewPassword);

                if (success)
                {
                    SuccessMessage = "Password reset successfully";

                    // Reset passwords
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;
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

        /// <summary>
        /// Determines if the reset password command can execute
        /// </summary>
        private bool CanResetPassword(object? parameter = null)
        {
            return SelectedUser != null;
        }

        /// <summary>
        /// Assigns a role to the selected user
        /// </summary>
        private async Task AssignRoleAsync(object? parameter = null)
        {
            try
            {
                if (SelectedUser == null || SelectedRole == null)
                    return;

                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                bool success = await _dashboardService.AssignUserToRoleAsync(SelectedUser.Id, SelectedRole.Id);

                if (success)
                {
                    SuccessMessage = $"Role '{SelectedRole.Name}' assigned to user '{SelectedUser.Username}' successfully";

                    // Update user
                    if (!SelectedUser.RoleIds.Contains(SelectedRole.Id))
                    {
                        SelectedUser.RoleIds.Add(SelectedRole.Id);
                    }

                    if (!SelectedUser.RoleNames.Contains(SelectedRole.Name))
                    {
                        SelectedUser.RoleNames.Add(SelectedRole.Name);
                    }

                    // Refresh user stats
                    await LoadUserStatsAsync();
                }
                else
                {
                    ErrorMessage = "Failed to assign role";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role");
                ErrorMessage = $"Error assigning role: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Determines if the assign role command can execute
        /// </summary>
        private bool CanAssignRole(object? parameter = null)
        {
            return SelectedUser != null && SelectedRole != null;
        }
    }
}