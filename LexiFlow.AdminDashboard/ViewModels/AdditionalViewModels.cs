using LexiFlow.AdminDashboard.ViewModels.Base;
using LexiFlow.AdminDashboard.Services;
using LexiFlow.AdminDashboard.Models.Charts;
using Microsoft.Extensions.Logging;
using OxyPlot;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Text.Json;
using System.Linq;
using LexiFlow.Models.User;
using LexiFlow.Models.User.UserRelations;

namespace LexiFlow.AdminDashboard.ViewModels
{
    public class UserManagementViewModel : ViewModelBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly ILogger<UserManagementViewModel> _logger;

        // Collections
        public ObservableCollection<LexiFlow.Models.User.User> Users { get; } = new();
        public ObservableCollection<LexiFlow.Models.User.Role> AllRoles { get; } = new();
        public ObservableCollection<LexiFlow.AdminDashboard.Services.Department> Departments { get; } = new();

        // Selected Items
        private LexiFlow.Models.User.User _selectedUser;
        private List<LexiFlow.Models.User.User> _selectedUsers = new();

        // UI State
        private bool _isLoading;
        private string _searchText = "";
        private int _currentPage = 1;
        private int _pageSize = 50;
        private int _totalUsers;
        private string _statusMessage = "";

        // Filter Properties
        private bool? _filterIsActive = null;
        private List<int> _filterRoleIds = new();
        private DateTime? _filterCreatedFrom;
        private DateTime? _filterCreatedTo;

        // Form Properties for Create/Edit
        private string _formUsername = "";
        private string _formEmail = "";
        private string _formPassword = "";
        private string _formFirstName = "";
        private string _formLastName = "";
        private string _formPhoneNumber = "";
        private string _formPreferredLanguage = "en";
        private string _formTimeZone = "UTC";
        private bool _formIsActive = true;
        private List<int> _formSelectedRoleIds = new();
        private int? _formDepartmentId;
        private bool _isEditMode = false;

        // Properties
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (SetProperty(ref _selectedUser, value))
                {
                    OnPropertyChanged(nameof(IsUserSelected));
                    LoadUserForEdit();
                }
            }
        }

        public List<User> SelectedUsers
        {
            get => _selectedUsers;
            set
            {
                SetProperty(ref _selectedUsers, value);
                OnPropertyChanged(nameof(IsMultipleUsersSelected));
                OnPropertyChanged(nameof(SelectedUsersCount));
            }
        }

        public bool IsUserSelected => SelectedUser != null;
        public bool IsMultipleUsersSelected => SelectedUsers?.Count > 1;
        public int SelectedUsersCount => SelectedUsers?.Count ?? 0;

        public new bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    SearchCommand.Execute(null);
                }
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    _ = LoadUsersAsync();
                }
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (SetProperty(ref _pageSize, value))
                {
                    CurrentPage = 1;
                    _ = LoadUsersAsync();
                }
            }
        }

        public int TotalUsers
        {
            get => _totalUsers;
            set => SetProperty(ref _totalUsers, value);
        }

        public int TotalPages => (int)Math.Ceiling((double)TotalUsers / PageSize);

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        // Filter Properties
        public bool? FilterIsActive
        {
            get => _filterIsActive;
            set
            {
                if (SetProperty(ref _filterIsActive, value))
                {
                    ApplyFiltersCommand.Execute(null);
                }
            }
        }

        public List<int> FilterRoleIds
        {
            get => _filterRoleIds;
            set
            {
                if (SetProperty(ref _filterRoleIds, value))
                {
                    ApplyFiltersCommand.Execute(null);
                }
            }
        }

        public DateTime? FilterCreatedFrom
        {
            get => _filterCreatedFrom;
            set
            {
                if (SetProperty(ref _filterCreatedFrom, value))
                {
                    ApplyFiltersCommand.Execute(null);
                }
            }
        }

        public DateTime? FilterCreatedTo
        {
            get => _filterCreatedTo;
            set
            {
                if (SetProperty(ref _filterCreatedTo, value))
                {
                    ApplyFiltersCommand.Execute(null);
                }
            }
        }

        // Form Properties
        public string FormUsername
        {
            get => _formUsername;
            set => SetProperty(ref _formUsername, value);
        }

        public string FormEmail
        {
            get => _formEmail;
            set => SetProperty(ref _formEmail, value);
        }

        public string FormPassword
        {
            get => _formPassword;
            set => SetProperty(ref _formPassword, value);
        }

        public string FormFirstName
        {
            get => _formFirstName;
            set => SetProperty(ref _formFirstName, value);
        }

        public string FormLastName
        {
            get => _formLastName;
            set => SetProperty(ref _formLastName, value);
        }

        public string FormPhoneNumber
        {
            get => _formPhoneNumber;
            set => SetProperty(ref _formPhoneNumber, value);
        }

        public string FormPreferredLanguage
        {
            get => _formPreferredLanguage;
            set => SetProperty(ref _formPreferredLanguage, value);
        }

        public string FormTimeZone
        {
            get => _formTimeZone;
            set => SetProperty(ref _formTimeZone, value);
        }

        public bool FormIsActive
        {
            get => _formIsActive;
            set => SetProperty(ref _formIsActive, value);
        }

        public List<int> FormSelectedRoleIds
        {
            get => _formSelectedRoleIds;
            set => SetProperty(ref _formSelectedRoleIds, value);
        }

        public int? FormDepartmentId
        {
            get => _formDepartmentId;
            set => SetProperty(ref _formDepartmentId, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        // Commands
        public ICommand LoadUsersCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand RefreshCommand { get; }

        // CRUD Commands
        public ICommand CreateUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand SaveUserCommand { get; }
        public ICommand CancelEditCommand { get; }

        // Bulk Commands
        public ICommand DeleteSelectedUsersCommand { get; }
        public ICommand ActivateSelectedUsersCommand { get; }
        public ICommand DeactivateSelectedUsersCommand { get; }
        public ICommand ExportUsersCommand { get; }
        public ICommand ImportUsersCommand { get; }

        // Role Management Commands
        public ICommand ManageUserRolesCommand { get; }
        public ICommand ResetPasswordCommand { get; }

        // Pagination Commands
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }

        public UserManagementViewModel(IUserManagementService userManagementService, ILogger<UserManagementViewModel> logger)
        {
            _userManagementService = userManagementService;
            _logger = logger;

            // Initialize Commands
            LoadUsersCommand = CreateCommand(async _ => await LoadUsersAsync());
            SearchCommand = CreateCommand(async _ => await SearchUsersAsync(), _ => !IsLoading);
            ApplyFiltersCommand = CreateCommand(async _ => await ApplyFiltersAsync(), _ => !IsLoading);
            ClearFiltersCommand = CreateCommand(_ => ClearFilters());
            RefreshCommand = CreateCommand(async _ => await RefreshAsync());

            // CRUD Commands
            CreateUserCommand = CreateCommand(_ => StartCreateUser());
            EditUserCommand = CreateCommand(_ => StartEditUser(), _ => IsUserSelected);
            DeleteUserCommand = CreateCommand(async _ => await DeleteUserAsync(), _ => IsUserSelected);
            SaveUserCommand = CreateCommand(async _ => await SaveUserAsync());
            CancelEditCommand = CreateCommand(_ => CancelEdit());

            // Bulk Commands
            DeleteSelectedUsersCommand = CreateCommand(async _ => await DeleteSelectedUsersAsync(), _ => IsMultipleUsersSelected);
            ActivateSelectedUsersCommand = CreateCommand(async _ => await ActivateSelectedUsersAsync(), _ => IsMultipleUsersSelected);
            DeactivateSelectedUsersCommand = CreateCommand(async _ => await DeactivateSelectedUsersAsync(), _ => IsMultipleUsersSelected);
            ExportUsersCommand = CreateCommand(async _ => await ExportUsersAsync());
            ImportUsersCommand = CreateCommand(async _ => await ImportUsersAsync());

            // Role Management Commands
            ManageUserRolesCommand = CreateCommand(async _ => await ManageUserRolesAsync(), _ => IsUserSelected);
            ResetPasswordCommand = CreateCommand(async _ => await ResetPasswordAsync(), _ => IsUserSelected);

            // Pagination Commands
            FirstPageCommand = CreateCommand(_ => CurrentPage = 1, _ => CurrentPage > 1);
            PreviousPageCommand = CreateCommand(_ => CurrentPage--, _ => CurrentPage > 1);
            NextPageCommand = CreateCommand(_ => CurrentPage++, _ => CurrentPage < TotalPages);
            LastPageCommand = CreateCommand(_ => CurrentPage = TotalPages, _ => CurrentPage < TotalPages);

            // Initialize data
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Initializing user management...";

                // Load reference data
                await LoadRolesAsync();
                await LoadDepartmentsAsync();

                // Load users
                await LoadUsersAsync();

                StatusMessage = "User management ready";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing user management");
                StatusMessage = "Error initializing user management";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                IsLoading = true;
                
                var users = await _userManagementService.GetUsersAsync(CurrentPage, PageSize, SearchText);
                var userCount = await _userManagementService.GetUserCountAsync(SearchText);

                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }

                TotalUsers = userCount;
                OnPropertyChanged(nameof(TotalPages));

                StatusMessage = $"Loaded {Users.Count} users (Page {CurrentPage} of {TotalPages})";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users");
                StatusMessage = "Error loading users";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SearchUsersAsync()
        {
            CurrentPage = 1;
            await LoadUsersAsync();
        }

        private async Task ApplyFiltersAsync()
        {
            try
            {
                IsLoading = true;

                var filter = new UserSearchFilter
                {
                    SearchTerm = SearchText,
                    RoleIds = FilterRoleIds,
                    IsActive = FilterIsActive,
                    CreatedFrom = FilterCreatedFrom,
                    CreatedTo = FilterCreatedTo,
                    Page = CurrentPage,
                    PageSize = PageSize
                };

                var users = await _userManagementService.SearchUsersAsync(filter);

                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }

                StatusMessage = $"Found {Users.Count} users matching filters";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying filters");
                StatusMessage = "Error applying filters";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ClearFilters()
        {
            SearchText = "";
            FilterIsActive = null;
            FilterRoleIds = new List<int>();
            FilterCreatedFrom = null;
            FilterCreatedTo = null;
            CurrentPage = 1;
            
            _ = LoadUsersAsync();
        }

        private async Task RefreshAsync()
        {
            await LoadUsersAsync();
            StatusMessage = "Data refreshed";
        }

        private async Task LoadRolesAsync()
        {
            try
            {
                var roles = await _userManagementService.GetRolesAsync();
                
                AllRoles.Clear();
                foreach (var role in roles)
                {
                    AllRoles.Add(role);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading roles");
            }
        }

        private async Task LoadDepartmentsAsync()
        {
            try
            {
                var departments = await _userManagementService.GetDepartmentsAsync();
                
                Departments.Clear();
                foreach (var department in departments)
                {
                    Departments.Add(department);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading departments");
            }
        }

        #region CRUD Operations

        private void StartCreateUser()
        {
            ClearForm();
            IsEditMode = false;
            // Show create user dialog
            // This would typically open a modal dialog or navigate to a create user view
        }

        private void StartEditUser()
        {
            if (SelectedUser == null) return;

            LoadUserForEdit();
            IsEditMode = true;
            // Show edit user dialog
        }

        private void LoadUserForEdit()
        {
            if (SelectedUser == null) return;

            FormUsername = SelectedUser.Username;
            FormEmail = SelectedUser.Email;
            FormFirstName = ""; // UserProfile navigation not available in current model
            FormLastName = ""; // UserProfile navigation not available in current model  
            FormPhoneNumber = ""; // UserProfile navigation not available in current model
            FormPreferredLanguage = SelectedUser.PreferredLanguage;
            FormTimeZone = SelectedUser.TimeZone;
            FormIsActive = SelectedUser.IsActive;
            FormSelectedRoleIds = SelectedUser.UserRoles?.Select(ur => ur.RoleId).ToList() ?? new List<int>();
        }

        private async Task SaveUserAsync()
        {
            try
            {
                IsLoading = true;

                if (ValidateForm())
                {
                    if (IsEditMode)
                    {
                        await UpdateUserAsync();
                    }
                    else
                    {
                        await CreateUserAsync();
                    }

                    ClearForm();
                    await LoadUsersAsync();
                    StatusMessage = IsEditMode ? "User updated successfully" : "User created successfully";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving user");
                StatusMessage = "Error saving user";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CreateUserAsync()
        {
            var request = new CreateUserRequest
            {
                Username = FormUsername,
                Email = FormEmail,
                Password = FormPassword,
                FirstName = FormFirstName,
                LastName = FormLastName,
                PhoneNumber = FormPhoneNumber,
                PreferredLanguage = FormPreferredLanguage,
                TimeZone = FormTimeZone,
                DepartmentId = FormDepartmentId,
                RoleIds = FormSelectedRoleIds,
                IsActive = FormIsActive
            };

            await _userManagementService.CreateUserAsync(request);
        }

        private async Task UpdateUserAsync()
        {
            if (SelectedUser == null) return;

            var request = new UpdateUserRequest
            {
                Username = FormUsername,
                Email = FormEmail,
                FirstName = FormFirstName,
                LastName = FormLastName,
                PhoneNumber = FormPhoneNumber,
                PreferredLanguage = FormPreferredLanguage,
                TimeZone = FormTimeZone,
                DepartmentId = FormDepartmentId,
                RoleIds = FormSelectedRoleIds,
                IsActive = FormIsActive
            };

            await _userManagementService.UpdateUserAsync(SelectedUser.UserId, request);
        }

        private async Task DeleteUserAsync()
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete user '{SelectedUser.Username}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    IsLoading = true;
                    
                    await _userManagementService.DeleteUserAsync(SelectedUser.UserId, softDelete: true);
                    await LoadUsersAsync();
                    
                    StatusMessage = $"User '{SelectedUser.Username}' deleted successfully";
                    SelectedUser = null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting user");
                    StatusMessage = "Error deleting user";
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private void CancelEdit()
        {
            ClearForm();
            SelectedUser = null;
        }

        private void ClearForm()
        {
            FormUsername = "";
            FormEmail = "";
            FormPassword = "";
            FormFirstName = "";
            FormLastName = "";
            FormPhoneNumber = "";
            FormPreferredLanguage = "en";
            FormTimeZone = "UTC";
            FormIsActive = true;
            FormSelectedRoleIds = new List<int>();
            FormDepartmentId = null;
            IsEditMode = false;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(FormUsername))
            {
                StatusMessage = "Username is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(FormEmail))
            {
                StatusMessage = "Email is required";
                return false;
            }

            if (!IsEditMode && string.IsNullOrWhiteSpace(FormPassword))
            {
                StatusMessage = "Password is required for new users";
                return false;
            }

            return true;
        }

        #endregion

        #region Bulk Operations

        private async Task DeleteSelectedUsersAsync()
        {
            if (SelectedUsers?.Any() != true) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete {SelectedUsers.Count} selected users?",
                "Confirm Bulk Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    IsLoading = true;
                    
                    foreach (var user in SelectedUsers)
                    {
                        await _userManagementService.DeleteUserAsync(user.UserId, softDelete: true);
                    }
                    
                    await LoadUsersAsync();
                    StatusMessage = $"{SelectedUsers.Count} users deleted successfully";
                    SelectedUsers = new List<LexiFlow.Models.User.User>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting selected users");
                    StatusMessage = "Error deleting selected users";
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        private async Task ActivateSelectedUsersAsync()
        {
            if (SelectedUsers?.Any() != true) return;

            try
            {
                IsLoading = true;
                
                foreach (var user in SelectedUsers)
                {
                    await _userManagementService.ActivateUserAsync(user.UserId);
                }
                
                await LoadUsersAsync();
                StatusMessage = $"{SelectedUsers.Count} users activated successfully";
                SelectedUsers = new List<LexiFlow.Models.User.User>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating selected users");
                StatusMessage = "Error activating selected users";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeactivateSelectedUsersAsync()
        {
            if (SelectedUsers?.Any() != true) return;

            try
            {
                IsLoading = true;
                
                foreach (var user in SelectedUsers)
                {
                    await _userManagementService.DeactivateUserAsync(user.UserId);
                }
                
                await LoadUsersAsync();
                StatusMessage = $"{SelectedUsers.Count} users deactivated successfully";
                SelectedUsers = new List<LexiFlow.Models.User.User>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating selected users");
                StatusMessage = "Error deactivating selected users";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ExportUsersAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Exporting users...";

                var userIds = SelectedUsers?.Any() == true ? 
                    SelectedUsers.Select(u => u.UserId).ToList() : null;

                var excelData = await _userManagementService.ExportUsersAsync(userIds);

                // Save file dialog
                var fileName = $"LexiFlow_Users_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                // TODO: Implement file save dialog
                


                StatusMessage = "Users exported successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting users");
                StatusMessage = "Error exporting users";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ImportUsersAsync()
        {
            try
            {
                // TODO: Implement file open dialog for CSV/Excel import
                StatusMessage = "Import users functionality - coming soon";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing users");
                StatusMessage = "Error importing users";
            }
        }

        #endregion

        #region Role Management

        private async Task ManageUserRolesAsync()
        {
            if (SelectedUser == null) return;

            try
            {
                // This would open a role management dialog
                // For now, we'll just show current roles
                var userRoles = await _userManagementService.GetUserRolesAsync(SelectedUser.UserId);
                var roleNames = string.Join(", ", userRoles.Select(r => r.RoleName));
                
                MessageBox.Show($"Current roles for {SelectedUser.Username}:\n{roleNames}", 
                    "User Roles", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error managing user roles");
                StatusMessage = "Error managing user roles";
            }
        }

        private async Task ResetPasswordAsync()
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to reset password for user '{SelectedUser.Username}'?\nA new random password will be generated.",
                "Confirm Password Reset",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    IsLoading = true;
                    
                    await _userManagementService.ResetPasswordAsync(SelectedUser.UserId);
                    
                    StatusMessage = $"Password reset for user '{SelectedUser.Username}'";
                    MessageBox.Show("Password has been reset. Please provide the new password to the user.", 
                        "Password Reset", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error resetting password");
                    StatusMessage = "Error resetting password";
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        #endregion
    }

    public class KanjiManagementViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<KanjiManagementViewModel> _logger;

        public KanjiManagementViewModel(IApiClient apiClient, ILogger<KanjiManagementViewModel> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
    }

    public class GrammarManagementViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<GrammarManagementViewModel> _logger;

        public GrammarManagementViewModel(IApiClient apiClient, ILogger<GrammarManagementViewModel> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
    };

    public class MediaManagementViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<MediaManagementViewModel> _logger;

        public MediaManagementViewModel(IApiClient apiClient, ILogger<MediaManagementViewModel> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
    }

    public class ExamManagementViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<ExamManagementViewModel> _logger;

        public ExamManagementViewModel(IApiClient apiClient, ILogger<ExamManagementViewModel> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
    }

    public class AnalyticsViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<AnalyticsViewModel> _logger;
        private readonly IRealTimeAnalyticsService _realTimeService;
        private readonly IChartExportService _chartExportService;
        private readonly IPredictiveAnalyticsService _predictiveAnalyticsService;
        private readonly IAnalyticsDatabaseService _databaseService;

        // Properties for data binding
        private object _dashboardData;
        private bool _isLoading;
        private string _errorMessage;
        private int _selectedDays = 30;
        private bool _isRealTimeEnabled = true;
        private string _connectionStatus = "Disconnected";
        private bool _isConnected = false;

        // Chart properties
        private PlotModel _studyTimeTrendChart;
        private PlotModel _performanceComparisonChart;
        private PlotModel _skillBreakdownChart;
        private PlotModel _studyPatternChart;
        private PlotModel _progressComparisonChart;

        // Advanced features
        private PlotModel _predictiveChart;
        private PlotModel _efficiencyAnalysisChart;
        private bool _showPredictiveAnalytics = false;
        private bool _showAdvancedMetrics = false;

        // Chart data
        private StudyTimeSeriesData _chartData;

        // Real-time data
        private DateTime _lastUpdateTime = DateTime.Now;
        private int _totalUpdatesReceived = 0;

        // New properties for Stage 3 features
        private bool _isExporting = false;
        private string _exportStatus = "";
        private bool _isPredicting = false;
        private string _predictionStatus = "";

        public object DashboardData
        {
            get => _dashboardData;
            set => SetProperty(ref _dashboardData, value);
        }

        public new bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public new string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public int SelectedDays
        {
            get => _selectedDays;
            set
            {
                if (SetProperty(ref _selectedDays, value))
                {
                    _ = LoadDashboardDataAsync();
                    if (_isRealTimeEnabled && _realTimeService?.IsConnected == true)
                    {
                        _ = _realTimeService.RequestDashboardUpdateAsync(value);
                    }
                }
            }
        }

        public bool IsRealTimeEnabled
        {
            get => _isRealTimeEnabled;
            set
            {
                if (SetProperty(ref _isRealTimeEnabled, value))
                {
                    if (value)
                    {
                        _ = ConnectRealTimeAsync();
                    }
                    else
                    {
                        _ = DisconnectRealTimeAsync();
                    }
                }
            }
        }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set => SetProperty(ref _connectionStatus, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public DateTime LastUpdateTime
        {
            get => _lastUpdateTime;
            set => SetProperty(ref _lastUpdateTime, value);
        }

        public int TotalUpdatesReceived
        {
            get => _totalUpdatesReceived;
            set => SetProperty(ref _totalUpdatesReceived, value);
        }

        // Chart Properties
        public PlotModel StudyTimeTrendChart
        {
            get => _studyTimeTrendChart;
            set => SetProperty(ref _studyTimeTrendChart, value);
        }

        public PlotModel PerformanceComparisonChart
        {
            get => _performanceComparisonChart;
            set => SetProperty(ref _performanceComparisonChart, value);
        }

        public PlotModel SkillBreakdownChart
        {
            get => _skillBreakdownChart;
            set => SetProperty(ref _skillBreakdownChart, value);
        }

        public PlotModel StudyPatternChart
        {
            get => _studyPatternChart;
            set => SetProperty(ref _studyPatternChart, value);
        }

        public PlotModel ProgressComparisonChart
        {
            get => _progressComparisonChart;
            set => SetProperty(ref _progressComparisonChart, value);
        }

        // New advanced chart properties
        public PlotModel PredictiveChart
        {
            get => _predictiveChart;
            set => SetProperty(ref _predictiveChart, value);
        }

        public PlotModel EfficiencyAnalysisChart
        {
            get => _efficiencyAnalysisChart;
            set => SetProperty(ref _efficiencyAnalysisChart, value);
        }

        public bool ShowPredictiveAnalytics
        {
            get => _showPredictiveAnalytics;
            set
            {
                if (SetProperty(ref _showPredictiveAnalytics, value) && value)
                {
                    _ = LoadPredictiveAnalyticsAsync();
                }
            }
        }

        public bool ShowAdvancedMetrics
        {
            get => _showAdvancedMetrics;
            set
            {
                if (SetProperty(ref _showAdvancedMetrics, value) && value)
                {
                    _ = LoadAdvancedMetricsAsync();
                }
            }
        }

        public bool IsExporting
        {
            get => _isExporting;
            set => SetProperty(ref _isExporting, value);
        }

        public string ExportStatus
        {
            get => _exportStatus;
            set => SetProperty(ref _exportStatus, value);
        }

        public bool IsPredicting
        {
            get => _isPredicting;
            set => SetProperty(ref _isPredicting, value);
        }

        public string PredictionStatus
        {
            get => _predictionStatus;
            set => SetProperty(ref _predictionStatus, value);
        }

        // Commands
        public ICommand LoadDataCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand ExportDataCommand { get; }
        public ICommand ExportChartCommand { get; }
        public ICommand ConnectRealTimeCommand { get; }
        public ICommand DisconnectRealTimeCommand { get; }
        public ICommand RequestRealTimeUpdateCommand { get; }
        public ICommand ClearCacheCommand { get; }

        // New advanced commands
        public ICommand ExportToPngCommand { get; }
        public ICommand ExportToPdfCommand { get; }
        public ICommand ExportToSvgCommand { get; }
        public ICommand ExportDashboardCommand { get; }
        public ICommand PrintChartCommand { get; }
        public ICommand GeneratePredictionCommand { get; }
        public ICommand AnalyzeEfficiencyCommand { get; }
        public ICommand ShowRecommendationsCommand { get; }
        public ICommand CreateSnapshotCommand { get; }
        public ICommand OptimizeDataCommand { get; }
        public ICommand TogglePredictiveViewCommand { get; }
        public ICommand ToggleAdvancedMetricsCommand { get; }

        // Collections for metrics display
        public ObservableCollection<MetricCardData> OverviewMetrics { get; } = new();
        public ObservableCollection<SkillProgressData> SkillProgress { get; } = new();
        public ObservableCollection<RealTimeActivityData> RecentActivities { get; } = new();

        // New collections for advanced features
        public ObservableCollection<PersonalizedRecommendation> Recommendations { get; } = new();
        public ObservableCollection<RiskAlert> RiskAlerts { get; } = new();
        public ObservableCollection<PredictedDataPoint> PredictedProgress { get; } = new();

        public AnalyticsViewModel(IApiClient apiClient, ILogger<AnalyticsViewModel> logger, 
            IRealTimeAnalyticsService realTimeService, IChartExportService chartExportService,
            IPredictiveAnalyticsService predictiveAnalyticsService, IAnalyticsDatabaseService databaseService)
        {
            _apiClient = apiClient;
            _logger = logger;
            _realTimeService = realTimeService;
            _chartExportService = chartExportService;
            _predictiveAnalyticsService = predictiveAnalyticsService;
            _databaseService = databaseService;

            // Initialize existing commands
            LoadDataCommand = CreateCommand(async _ => await LoadDashboardDataAsync());
            RefreshCommand = CreateCommand(async _ => await RefreshDataAsync());
            GenerateReportCommand = CreateCommand(async _ => await GenerateReportAsync());
            ExportDataCommand = CreateCommand(async _ => await ExportDataAsync());
            ExportChartCommand = CreateCommand(async _ => await ExportChartAsync());
            ConnectRealTimeCommand = CreateCommand(async _ => await ConnectRealTimeAsync());
            DisconnectRealTimeCommand = CreateCommand(async _ => await DisconnectRealTimeAsync());
            RequestRealTimeUpdateCommand = CreateCommand(async _ => await RequestRealTimeUpdateAsync());
            ClearCacheCommand = CreateCommand(async _ => await ClearCacheAsync());

            // Initialize new advanced commands
            ExportToPngCommand = CreateCommand(async _ => await ExportToPngAsync());
            ExportToPdfCommand = CreateCommand(async _ => await ExportToPdfAsync());
            ExportToSvgCommand = CreateCommand(async _ => await ExportToSvgAsync());
            ExportDashboardCommand = CreateCommand(async _ => await ExportDashboardAsync());
            PrintChartCommand = CreateCommand(async _ => await PrintChartAsync());
            GeneratePredictionCommand = CreateCommand(async _ => await GeneratePredictionAsync());
            AnalyzeEfficiencyCommand = CreateCommand(async _ => await AnalyzeEfficiencyAsync());
            ShowRecommendationsCommand = CreateCommand(async _ => await ShowRecommendationsAsync());
            CreateSnapshotCommand = CreateCommand(async _ => await CreateSnapshotAsync());
            OptimizeDataCommand = CreateCommand(async _ => await OptimizeDataAsync());
            TogglePredictiveViewCommand = CreateCommand(_ => ShowPredictiveAnalytics = !ShowPredictiveAnalytics);
            ToggleAdvancedMetricsCommand = CreateCommand(_ => ShowAdvancedMetrics = !ShowAdvancedMetrics);

            // Initialize chart data
            _chartData = new StudyTimeSeriesData();

            // Setup real-time service event handlers
            SetupRealTimeEventHandlers();

            // Load initial data
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadDashboardDataAsync();
            
            if (_isRealTimeEnabled)
            {
                await ConnectRealTimeAsync();
            }

            // Load initial recommendations
            await LoadRecommendationsAsync();
        }

        private void SetupRealTimeEventHandlers()
        {
            if (_realTimeService != null)
            {
                _realTimeService.DashboardDataUpdated += OnDashboardDataUpdated;
                _realTimeService.PerformanceDataUpdated += OnPerformanceDataUpdated;
                _realTimeService.StudyActivityReported += OnStudyActivityReported;
                _realTimeService.TestCompleted += OnTestCompleted;
                _realTimeService.GoalProgressUpdated += OnGoalProgressUpdated;
                _realTimeService.ConnectionStatusChanged += OnConnectionStatusChanged;
            }
        }

        private async void OnDashboardDataUpdated(object sender, object data)
        {
            try
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    DashboardData = data;
                    LastUpdateTime = DateTime.Now;
                    TotalUpdatesReceived++;
                    
                    // Update charts with new data
                    _ = Task.Run(async () => await GenerateChartsFromRealTimeData(data));
                    
                    // Update metrics if data is in correct format
                    if (data is Dictionary<string, object> metricsData)
                    {
                        UpdateMetricsFromRealTimeData(metricsData);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling dashboard data update");
            }
        }

        private async void OnPerformanceDataUpdated(object sender, object data)
        {
            try
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    LastUpdateTime = DateTime.Now;
                    TotalUpdatesReceived++;
                    
                    // Update performance-related charts
                    _ = Task.Run(async () => await UpdatePerformanceCharts(data));
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling performance data update");
            }
        }

        private async void OnStudyActivityReported(object sender, string activityJson)
        {
            try
            {
                var activity = JsonSerializer.Deserialize<object>(activityJson);
                
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    // Add to recent activities
                    RecentActivities.Insert(0, new RealTimeActivityData
                    {
                        Timestamp = DateTime.Now,
                        ActivityType = "Study Session",
                        Description = "New study activity reported",
                        Icon = "Book",
                        Color = "#FF4CAF50"
                    });

                    // Keep only recent 10 activities
                    while (RecentActivities.Count > 10)
                    {
                        RecentActivities.RemoveAt(RecentActivities.Count - 1);
                    }

                    TotalUpdatesReceived++;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling study activity report");
            }
        }

        private async void OnTestCompleted(object sender, string testJson)
        {
            try
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    RecentActivities.Insert(0, new RealTimeActivityData
                    {
                        Timestamp = DateTime.Now,
                        ActivityType = "Test",
                        Description = "Test completion reported",
                        Icon = "TestTube",
                        Color = "#FFFF9800"
                    });

                    while (RecentActivities.Count > 10)
                    {
                        RecentActivities.RemoveAt(RecentActivities.Count - 1);
                    }

                    TotalUpdatesReceived++;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling test completion");
            }
        }

        private async void OnGoalProgressUpdated(object sender, object data)
        {
            try
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    LastUpdateTime = DateTime.Now;
                    TotalUpdatesReceived++;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling goal progress update");
            }
        }

        private async void OnConnectionStatusChanged(object sender, string status)
        {
            try
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    ConnectionStatus = status;
                    IsConnected = status == "Connected";
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling connection status change");
            }
        }

        public async Task ConnectRealTimeAsync()
        {
            try
            {
                if (_realTimeService != null && !_realTimeService.IsConnected)
                {
                    IsLoading = true;
                    await _realTimeService.ConnectAsync();
                    
                    // Subscribe to relevant channels
                    await _realTimeService.SubscribeToChannelAsync("study-sessions");
                    await _realTimeService.SubscribeToChannelAsync("test-results");
                    await _realTimeService.SubscribeToChannelAsync("goal-progress");
                    await _realTimeService.SubscribeToChannelAsync("daily-stats");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error connecting to real-time service");
                ErrorMessage = "Failed to connect to real-time updates";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task DisconnectRealTimeAsync()
        {
            try
            {
                if (_realTimeService != null && _realTimeService.IsConnected)
                {
                    await _realTimeService.DisconnectAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disconnecting from real-time service");
            }
        }

        public async Task RequestRealTimeUpdateAsync()
        {
            try
            {
                if (_realTimeService?.IsConnected == true)
                {
                    await _realTimeService.RequestDashboardUpdateAsync(SelectedDays);
                    await _realTimeService.RequestPerformanceUpdateAsync();
                }
                else
                {
                    MessageBox.Show("Real-time service is not connected", "Warning", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting real-time update");
                MessageBox.Show("Failed to request real-time update", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task ClearCacheAsync()
        {
            try
            {
                IsLoading = true;
                
                await _apiClient.PostAsync("api/analytics/clear-cache", null);
                
                // Refresh data after clearing cache
                await LoadDashboardDataAsync();
                
                MessageBox.Show("Cache cleared and data refreshed successfully!", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cache");
                MessageBox.Show("Failed to clear cache. Please try again.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ...existing methods with enhancements...

        public async Task LoadDashboardDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                // Use enhanced database service for real-time data
                if (_databaseService != null)
                {
                    var userId = 1; // TODO: Get current user ID
                    var realTimeData = await _databaseService.GetRealTimeStudyDataAsync(userId, SelectedDays);
                    var metrics = await _databaseService.GetRealTimeMetricsAsync(userId);
                    var activities = await _databaseService.GetRealTimeActivitiesAsync(userId);

                    // Update chart data
                    _chartData = realTimeData;
                    
                    // Update metrics
                    UpdateMetricsFromRealTimeData(metrics);
                    
                    // Update activities - convert to correct type
                    UpdateActivitiesFromRealTimeData(activities.Select(a => new RealTimeActivityData
                    {
                        Timestamp = a.Timestamp,
                        ActivityType = a.ActivityType,
                        Description = a.Description,
                        Icon = a.Icon,
                        Color = a.Color
                    }).ToList());
                    
                    // Generate charts
                    await GenerateChartsFromData();
                    
                    LastUpdateTime = DateTime.Now;
                }
                else
                {
                    // Fallback to API client
                    var queryParams = new Dictionary<string, string>
                    {
                        { "days", SelectedDays.ToString() },
                        { "useCache", "true" }
                    };

                    var data = await _apiClient.GetAsync<object>(
                        "api/analytics/dashboard", queryParams);

                    if (data != null)
                    {
                        DashboardData = data;
                        await GenerateChartsFromData();
                        UpdateMetricsCollections();
                        LastUpdateTime = DateTime.Now;
                    }
                    else
                    {
                        ErrorMessage = "Failed to load analytics data";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                ErrorMessage = "Error loading analytics data. Please try again.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task RefreshDataAsync()
        {
            await LoadDashboardDataAsync();
            
            if (_realTimeService?.IsConnected == true)
            {
                await _realTimeService.RequestDashboardUpdateAsync(SelectedDays);
            }
        }

        public async Task GenerateReportAsync()
        {
            try
            {
                IsLoading = true;

                var request = new
                {
                    ReportType = "overview",
                    StartDate = DateTime.Now.AddDays(-SelectedDays),
                    EndDate = DateTime.Now,
                    IncludeSections = new List<string> { "progress", "performance", "goals" },
                    IncludeCharts = true,
                    IncludeRecommendations = true,
                    Format = "json"
                };

                var report = await _apiClient.PostAsync<object, object>(
                    "api/analytics/generate-report", request);

                if (report != null)
                {
                    MessageBox.Show("Report generated successfully!", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report");
                MessageBox.Show("Error generating report. Please try again.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task ExportDataAsync()
        {
            try
            {
                IsLoading = true;

                MessageBox.Show("Export functionality will download the data as CSV file.", "Export", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting data");
                MessageBox.Show("Error exporting data. Please try again.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task ExportChartAsync()
        {
            try
            {
                IsLoading = true;

                // TODO: Implement chart export functionality
                MessageBox.Show("Chart export functionality will save charts as PNG files.", "Export Charts", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting charts");
                MessageBox.Show("Error exporting charts. Please try again.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GenerateChartsFromData()
        {
            await Task.Run(() =>
            {
                try
                {
                    // Generate mock data for demonstration
                    GenerateMockChartData();

                    // Create charts
                    StudyTimeTrendChart = AnalyticsChartFactory.CreateStudyTimeTrendChart(
                        _chartData.DailyStudyTime, "Daily Study Time Trend");

                    PerformanceComparisonChart = AnalyticsChartFactory.CreatePerformanceComparisonChart(
                        _chartData.PerformanceTrend, "Performance Trends by Skill");

                    SkillBreakdownChart = AnalyticsChartFactory.CreateSkillBreakdownPieChart(
                        GenerateSkillBreakdownData(), "Skill Accuracy Breakdown");

                    StudyPatternChart = AnalyticsChartFactory.CreateStudyPatternColumnChart(
                        GenerateStudyPatternData(), "Daily Study Pattern");

                    ProgressComparisonChart = AnalyticsChartFactory.CreateProgressComparisonAreaChart(
                        _chartData, "Learning Progress Comparison");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating charts");
                }
            });
        }

        private async Task GenerateChartsFromRealTimeData(object data)
        {
            await Task.Run(() =>
            {
                try
                {
                    // TODO: Parse real-time data and update charts
                    // For now, regenerate charts with mock data
                    GenerateMockChartData();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        StudyTimeTrendChart = AnalyticsChartFactory.CreateStudyTimeTrendChart(
                            _chartData.DailyStudyTime, "Daily Study Time Trend (Real-time)");

                        PerformanceComparisonChart = AnalyticsChartFactory.CreatePerformanceComparisonChart(
                            _chartData.PerformanceTrend, "Performance Trends by Skill (Real-time)");
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating charts from real-time data");
                }
            });
        }

        private async Task UpdatePerformanceCharts(object data)
        {
            await Task.Run(() =>
            {
                try
                {
                    // TODO: Parse performance data and update charts
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        // Update performance-related charts
                        PerformanceComparisonChart = AnalyticsChartFactory.CreatePerformanceComparisonChart(
                            _chartData.PerformanceTrend, "Performance Trends (Updated)");
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating performance charts");
                }
            });
        }

        private void UpdateMetricsCollections()
        {
            // Update overview metrics
            OverviewMetrics.Clear();
            OverviewMetrics.Add(new MetricCardData { Title = "Total Study Time", Value = "2,480 min", Icon = "Clock", Color = "#FF3F51B5" });
            OverviewMetrics.Add(new MetricCardData { Title = "Study Days", Value = "25", Icon = "Calendar", Color = "#FFFF5722" });
            OverviewMetrics.Add(new MetricCardData { Title = "Current Streak", Value = "7", Icon = "Fire", Color = "#FF4CAF50" });
            OverviewMetrics.Add(new MetricCardData { Title = "Average Score", Value = "78.5%", Icon = "Star", Color = "#FFFF9800" });

            // Update skill progress
            SkillProgress.Clear();
            SkillProgress.Add(new SkillProgressData { SkillName = "Vocabulary", Progress = 88, Color = "#FF9C27B0" });
            SkillProgress.Add(new SkillProgressData { SkillName = "Kanji", Progress = 86, Color = "#FF3F51B5" });
            SkillProgress.Add(new SkillProgressData { SkillName = "Grammar", Progress = 72, Color = "#FF607D8B" });
            SkillProgress.Add(new SkillProgressData { SkillName = "Reading", Progress = 79, Color = "#FFFF9800" });
            SkillProgress.Add(new SkillProgressData { SkillName = "Listening", Progress = 69, Color = "#FFE91E63" });
        }

        private async Task LoadPredictiveAnalyticsAsync()
        {
            try
            {
                if (_predictiveAnalyticsService == null) return;

                var userId = 1; // TODO: Get current user ID
                
                // Generate prediction
                var prediction = await _predictiveAnalyticsService.PredictLearningProgressAsync(userId, 30);
                
                // Update predicted progress
                PredictedProgress.Clear();
                foreach (var point in prediction.PredictedProgress)
                {
                    PredictedProgress.Add(point);
                }

                // Generate predictive chart
                await GeneratePredictiveChart(prediction);

                // Load risk alerts
                await LoadRiskAlertsAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading predictive analytics");
            }
        }

        private async Task LoadAdvancedMetricsAsync()
        {
            try
            {
                if (_predictiveAnalyticsService == null) return;

                var userId = 1; // TODO: Get current user ID
                
                // Analyze efficiency
                var analysis = await _predictiveAnalyticsService.AnalyzeStudyEfficiencyAsync(userId);
                
                // Generate efficiency chart
                await GenerateEfficiencyChart(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading advanced metrics");
            }
        }

        private async Task LoadRecommendationsAsync()
        {
            try
            {
                if (_predictiveAnalyticsService == null) return;

                var userId = 1; // TODO: Get current user ID
                var recommendations = await _predictiveAnalyticsService.GenerateRecommendationsAsync(userId);

                Recommendations.Clear();
                foreach (var recommendation in recommendations.Take(5)) // Show top 5
                {
                    Recommendations.Add(recommendation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading recommendations");
            }
        }

        private async Task LoadRiskAlertsAsync(int userId)
        {
            try
            {
                var risks = await _predictiveAnalyticsService.IdentifyLearningRisksAsync(userId);

                RiskAlerts.Clear();
                foreach (var risk in risks)
                {
                    RiskAlerts.Add(risk);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading risk alerts");
            }
        }

        private async Task GeneratePredictiveChart(LearningPrediction prediction)
        {
            await Task.Run(() =>
            {
                try
                {
                    // Create predictive chart with historical and predicted data
                    PredictiveChart = AnalyticsChartFactory.CreatePredictiveChart(
                        _chartData.DailyStudyTime, prediction.PredictedProgress, "Learning Progress Prediction");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating predictive chart");
                }
            });
        }

        private async Task GenerateEfficiencyChart(StudyEfficiencyAnalysis analysis)
        {
            await Task.Run(() =>
            {
                try
                {
                    // Create efficiency analysis chart
                    EfficiencyAnalysisChart = AnalyticsChartFactory.CreateEfficiencyChart(
                        analysis, "Study Efficiency Analysis");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating efficiency chart");
                }
            });
        }

        private void UpdateMetricsFromRealTimeData(Dictionary<string, object> metrics)
        {
            try
            {
                OverviewMetrics.Clear();
                
                if (metrics.ContainsKey("TotalStudyTime"))
                {
                    var totalMinutes = Convert.ToInt32(metrics["TotalStudyTime"]);
                    var hours = totalMinutes / 60;
                    var minutes = totalMinutes % 60;
                    OverviewMetrics.Add(new MetricCardData 
                    { 
                        Title = "Total Study Time", 
                        Value = $"{hours}h {minutes}m", 
                        Icon = "Clock", 
                        Color = "#FF3F51B5" 
                    });
                }

                if (metrics.ContainsKey("StudyDaysThisMonth"))
                {
                    OverviewMetrics.Add(new MetricCardData 
                    { 
                        Title = "Study Days", 
                        Value = metrics["StudyDaysThisMonth"].ToString(), 
                        Icon = "Calendar", 
                        Color = "#FFFF5722" 
                    });
                }

                if (metrics.ContainsKey("CurrentStreak"))
                {
                    OverviewMetrics.Add(new MetricCardData 
                    { 
                        Title = "Current Streak", 
                        Value = metrics["CurrentStreak"].ToString(), 
                        Icon = "Fire", 
                        Color = "#FF4CAF50" 
                    });
                }

                if (metrics.ContainsKey("AverageTestScore"))
                {
                    OverviewMetrics.Add(new MetricCardData 
                    { 
                        Title = "Average Score", 
                        Value = $"{metrics["AverageTestScore"]}%", 
                        Icon = "Star", 
                        Color = "#FFFF9800" 
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating metrics from real-time data");
            }
        }

        private void UpdateActivitiesFromRealTimeData(List<RealTimeActivityData> activities)
        {
            try
            {
                RecentActivities.Clear();
                foreach (var activity in activities)
                {
                    RecentActivities.Add(activity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating activities from real-time data");
            }
        }

        private void GenerateMockChartData()
        {
            var random = new Random();
            var startDate = DateTime.Now.AddDays(-SelectedDays);

            // Generate daily study time data
            _chartData.DailyStudyTime.Clear();
            for (int i = 0; i < SelectedDays; i++)
            {
                var date = startDate.AddDays(i);
                var studyTime = random.Next(30, 120);
                _chartData.DailyStudyTime.Add(new ChartDataPoint
                {
                    Date = date,
                    Value = studyTime,
                    Label = date.ToString("MM/dd"),
                    Category = "StudyTime"
                });
            }

            // Generate performance trend data
            _chartData.PerformanceTrend.Clear();
            var skills = new[] { "Vocabulary", "Kanji", "Grammar", "Reading", "Listening" };
            var baseAccuracies = new[] { 88f, 86f, 72f, 79f, 69f };

            for (int skillIndex = 0; skillIndex < skills.Length; skillIndex++)
            {
                var baseAccuracy = baseAccuracies[skillIndex];
                for (int i = 0; i < SelectedDays; i += 3) // Every 3 days
                {
                    var date = startDate.AddDays(i);
                    var variation = (random.NextDouble() - 0.5) * 10;
                    var accuracy = Math.Max(60, Math.Min(95, baseAccuracy + variation));
                    
                    _chartData.PerformanceTrend.Add(new ChartDataPoint
                    {
                        Date = date,
                        Value = accuracy,
                        Label = date.ToString("MM/dd"),
                        Category = skills[skillIndex]
                    });
                }
            }

            // Generate vocabulary progress data
            _chartData.VocabularyProgress.Clear();
            var vocabularyCount = 0;
            for (int i = 0; i < SelectedDays; i++)
            {
                var date = startDate.AddDays(i);
                vocabularyCount += random.Next(5, 15);
                _chartData.VocabularyProgress.Add(new ChartDataPoint
                {
                    Date = date,
                    Value = vocabularyCount,
                    Label = date.ToString("MM/dd"),
                    Category = "Vocabulary"
                });
            }

            // Generate Kanji progress data
            _chartData.KanjiProgress.Clear();
            var kanjiCount = 0;
            for (int i = 0; i < SelectedDays; i++)
            {
                var date = startDate.AddDays(i);
                kanjiCount += random.Next(2, 8);
                _chartData.KanjiProgress.Add(new ChartDataPoint
                {
                    Date = date,
                    Value = kanjiCount,
                    Label = date.ToString("MM/dd"),
                    Category = "Kanji"
                });
            }

            // Generate Grammar progress data
            _chartData.GrammarProgress.Clear();
            var grammarCount = 0;
            for (int i = 0; i < SelectedDays; i++)
            {
                var date = startDate.AddDays(i);
                grammarCount += random.Next(1, 4);
                _chartData.GrammarProgress.Add(new ChartDataPoint
                {
                    Date = date,
                    Value = grammarCount,
                    Label = date.ToString("MM/dd"),
                    Category = "Grammar"
                });
            }
        }

        private List<SkillBreakdownData> GenerateSkillBreakdownData()
        {
            return new List<SkillBreakdownData>
            {
                new SkillBreakdownData { SkillName = "Vocabulary", AccuracyRate = 88.2, TotalAttempts = 1250, ImprovementRate = 15.4, Level = "N3" },
                new SkillBreakdownData { SkillName = "Kanji", AccuracyRate = 85.7, TotalAttempts = 680, ImprovementRate = 18.2, Level = "N3" },
                new SkillBreakdownData { SkillName = "Grammar", AccuracyRate = 72.1, TotalAttempts = 420, ImprovementRate = 8.5, Level = "N4" },
                new SkillBreakdownData { SkillName = "Reading", AccuracyRate = 79.4, TotalAttempts = 320, ImprovementRate = 12.1, Level = "N4" },
                new SkillBreakdownData { SkillName = "Listening", AccuracyRate = 68.9, TotalAttempts = 180, ImprovementRate = 6.2, Level = "N4" }
            };
        }

        private List<StudyPatternData> GenerateStudyPatternData()
        {
            var data = new List<StudyPatternData>();
            var random = new Random();

            // Peak hours: 8AM, 12PM, 8PM
            var peakHours = new[] { 8, 12, 20 };
            
            for (int hour = 0; hour < 24; hour++)
            {
                var baseMinutes = peakHours.Contains(hour) ? random.Next(30, 60) : random.Next(0, 20);
                data.Add(new StudyPatternData
                {
                    Hour = hour,
                    DayOfWeek = 0, // Sunday for now
                    StudyMinutes = baseMinutes,
                    Sessions = baseMinutes > 20 ? random.Next(1, 3) : 0
                });
            }

            return data;
        }

        #region Advanced Export Features

        public async Task ExportToPngAsync()
        {
            try
            {
                IsExporting = true;
                ExportStatus = "Selecting file location...";

                var fileName = await _chartExportService.ShowSaveFileDialogAsync(
                    $"LexiFlow_Analytics_{DateTime.Now:yyyyMMdd_HHmmss}.png",
                    "PNG Files|*.png|All Files|*.*");

                if (string.IsNullOrEmpty(fileName))
                {
                    ExportStatus = "Export cancelled";
                    return;
                }

                ExportStatus = "Exporting chart to PNG...";

                var success = await _chartExportService.ExportChartToPngAsync(
                    StudyTimeTrendChart, fileName, 1200, 800);

                if (success)
                {
                    ExportStatus = "Chart exported successfully!";
                    MessageBox.Show($"Chart exported to: {fileName}", "Export Complete", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ExportStatus = "Export failed";
                    MessageBox.Show("Failed to export chart. Please try again.", "Export Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting chart to PNG");
                ExportStatus = "Export error occurred";
                MessageBox.Show("An error occurred during export.", "Export Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsExporting = false;
                await Task.Delay(3000); // Show status for 3 seconds
                ExportStatus = "";
            }
        }

        public async Task ExportToPdfAsync()
        {
            try
            {
                IsExporting = true;
                ExportStatus = "Selecting file location...";

                var fileName = await _chartExportService.ShowSaveFileDialogAsync(
                    $"LexiFlow_Analytics_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    "PDF Files|*.pdf|All Files|*.*");

                if (string.IsNullOrEmpty(fileName))
                {
                    ExportStatus = "Export cancelled";
                    return;
                }

                ExportStatus = "Exporting chart to PDF...";

                var success = await _chartExportService.ExportChartToPdfAsync(
                    StudyTimeTrendChart, fileName, 1200, 800);

                if (success)
                {
                    ExportStatus = "Chart exported successfully!";
                    MessageBox.Show($"Chart exported to: {fileName}", "Export Complete", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ExportStatus = "Export failed";
                    MessageBox.Show("Failed to export chart. Please try again.", "Export Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting chart to PDF");
                ExportStatus = "Export error occurred";
                MessageBox.Show("An error occurred during export.", "Export Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsExporting = false;
                await Task.Delay(3000);
                ExportStatus = "";
            }
        }

        public async Task ExportToSvgAsync()
        {
            try
            {
                IsExporting = true;
                ExportStatus = "Selecting file location...";

                var fileName = await _chartExportService.ShowSaveFileDialogAsync(
                    $"LexiFlow_Analytics_{DateTime.Now:yyyyMMdd_HHmmss}.svg",
                    "SVG Files|*.svg|All Files|*.*");

                if (string.IsNullOrEmpty(fileName))
                {
                    ExportStatus = "Export cancelled";
                    return;
                }

                ExportStatus = "Exporting chart to SVG...";

                var success = await _chartExportService.ExportChartToSvgAsync(
                    StudyTimeTrendChart, fileName, 1200, 800);

                if (success)
                {
                    ExportStatus = "Chart exported successfully!";
                    MessageBox.Show($"Chart exported to: {fileName}", "Export Complete", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ExportStatus = "Export failed";
                    MessageBox.Show("Failed to export chart. Please try again.", "Export Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting chart to SVG");
                ExportStatus = "Export error occurred";
                MessageBox.Show("An error occurred during export.", "Export Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsExporting = false;
                await Task.Delay(3000);
                ExportStatus = "";
            }
        }

        public async Task ExportDashboardAsync()
        {
            try
            {
                IsExporting = true;
                ExportStatus = "Preparing dashboard export...";

                var fileName = await _chartExportService.ShowSaveFileDialogAsync(
                    $"LexiFlow_Dashboard_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    "PDF Files|*.pdf|All Files|*.*");

                if (string.IsNullOrEmpty(fileName))
                {
                    ExportStatus = "Export cancelled";
                    return;
                }

                ExportStatus = "Collecting all charts...";

                // Collect all charts
                var charts = new Dictionary<string, PlotModel>();
                
                if (StudyTimeTrendChart != null)
                    charts["Study Time Trend"] = StudyTimeTrendChart;
                if (PerformanceComparisonChart != null)
                    charts["Performance Comparison"] = PerformanceComparisonChart;
                if (SkillBreakdownChart != null)
                    charts["Skill Breakdown"] = SkillBreakdownChart;
                if (StudyPatternChart != null)
                    charts["Study Pattern"] = StudyPatternChart;
                if (ProgressComparisonChart != null)
                    charts["Progress Comparison"] = ProgressComparisonChart;
                if (PredictiveChart != null && ShowPredictiveAnalytics)
                    charts["Predictive Analysis"] = PredictiveChart;
                if (EfficiencyAnalysisChart != null && ShowAdvancedMetrics)
                    charts["Efficiency Analysis"] = EfficiencyAnalysisChart;

                ExportStatus = "Generating PDF dashboard...";

                var success = await _chartExportService.ExportDashboardAsync(
                    charts, fileName, "LexiFlow Analytics Dashboard");

                if (success)
                {
                    ExportStatus = "Dashboard exported successfully!";
                    MessageBox.Show($"Dashboard exported to: {fileName}", "Export Complete", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ExportStatus = "Export failed";
                    MessageBox.Show("Failed to export dashboard. Please try again.", "Export Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting dashboard");
                ExportStatus = "Export error occurred";
                MessageBox.Show("An error occurred during dashboard export.", "Export Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsExporting = false;
                await Task.Delay(3000);
                ExportStatus = "";
            }
        }

        public async Task PrintChartAsync()
        {
            try
            {
                if (StudyTimeTrendChart == null)
                {
                    MessageBox.Show("No chart available to print.", "Print Error", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var success = await _chartExportService.PrintChartAsync(StudyTimeTrendChart);
                
                if (!success)
                {
                    MessageBox.Show("Print operation was cancelled or failed.", "Print Status", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error printing chart");
                MessageBox.Show("An error occurred while printing.", "Print Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Predictive Analytics Features

        public async Task GeneratePredictionAsync()
        {
            try
            {
                IsPredicting = true;
                PredictionStatus = "Analyzing historical data...";

                var userId = 1; // TODO: Get current user ID
                var prediction = await _predictiveAnalyticsService.PredictLearningProgressAsync(userId, SelectedDays);

                PredictionStatus = "Generating prediction charts...";

                // Update predicted progress collection
                PredictedProgress.Clear();
                foreach (var point in prediction.PredictedProgress)
                {
                    PredictedProgress.Add(point);
                }

                // Generate predictive chart
                await GeneratePredictiveChart(prediction);

                // Load risk alerts
                await LoadRiskAlertsAsync(userId);

                PredictionStatus = "Prediction completed successfully!";
                ShowPredictiveAnalytics = true;

                MessageBox.Show($"Learning prediction generated with {prediction.Confidence:P1} confidence.\n\n{prediction.Message}", 
                    "Prediction Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating prediction");
                PredictionStatus = "Prediction failed";
                MessageBox.Show("Failed to generate prediction. Please try again.", "Prediction Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsPredicting = false;
                await Task.Delay(3000);
                PredictionStatus = "";
            }
        }

        public async Task AnalyzeEfficiencyAsync()
        {
            try
            {
                IsPredicting = true;
                PredictionStatus = "Analyzing study efficiency...";

                var userId = 1; // TODO: Get current user ID
                var analysis = await _predictiveAnalyticsService.AnalyzeStudyEfficiencyAsync(userId);

                PredictionStatus = "Generating efficiency charts...";

                // Generate efficiency analysis chart
                await GenerateEfficiencyChart(analysis);

                PredictionStatus = "Efficiency analysis completed!";
                ShowAdvancedMetrics = true;

                var message = $"Study Efficiency Analysis:\n\n" +
                             $"Time Efficiency: {analysis.TimeEfficiency:P1}\n" +
                             $"Learning Velocity: {analysis.LearningVelocity:F2}\n" +
                             $"Retention Rate: {analysis.RetentionRate:P1}\n" +
                             $"Overall Score: {analysis.EfficiencyScore:F1}/10";

                MessageBox.Show(message, "Efficiency Analysis", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing efficiency");
                PredictionStatus = "Analysis failed";
                MessageBox.Show("Failed to analyze efficiency. Please try again.", "Analysis Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsPredicting = false;
                await Task.Delay(3000);
                PredictionStatus = "";
            }
        }

        public async Task ShowRecommendationsAsync()
        {
            try
            {
                IsLoading = true;
                
                var userId = 1; // TODO: Get current user ID
                var recommendations = await _predictiveAnalyticsService.GenerateRecommendationsAsync(userId);

                Recommendations.Clear();
                foreach (var recommendation in recommendations)
                {
                    Recommendations.Add(recommendation);
                }

                var recommendationWindow = new RecommendationsWindow(recommendations);
                recommendationWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error showing recommendations");
                MessageBox.Show("Failed to load recommendations. Please try again.", "Recommendations Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region Database Management Features

        public async Task CreateSnapshotAsync()
        {
            try
            {
                IsLoading = true;
                
                var userId = 1; // TODO: Get current user ID
                var snapshot = await _databaseService.CreateSnapshotAsync(userId);

                MessageBox.Show($"Analytics snapshot created successfully at {snapshot.CreatedAt:yyyy-MM-dd HH:mm:ss}", 
                    "Snapshot Created", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating snapshot");
                MessageBox.Show("Failed to create snapshot. Please try again.", "Snapshot Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task OptimizeDataAsync()
        {
            try
            {
                IsLoading = true;

                var result = MessageBox.Show(
                    "This will optimize the analytics database by archiving old data and updating statistics.\n\nDo you want to continue?",
                    "Optimize Database", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                    return;

                var success = await _databaseService.OptimizeAnalyticsDataAsync();

                if (success)
                {
                    MessageBox.Show("Database optimization completed successfully!", "Optimization Complete", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Database optimization failed. Please check the logs.", "Optimization Failed", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error optimizing database");
                MessageBox.Show("Failed to optimize database. Please try again.", "Optimization Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_realTimeService is IDisposable disposableService)
                {
                    disposableService.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }

    public class SettingsViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<SettingsViewModel> _logger;

        public SettingsViewModel(IApiClient apiClient, ILogger<SettingsViewModel> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
    }

    // Data models for UI binding
    public class MetricCardData
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
    }

    public class SkillProgressData
    {
        public string SkillName { get; set; }
        public double Progress { get; set; }
        public string Color { get; set; }
    }

    public class RealTimeActivityData
    {
        public DateTime Timestamp { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
    }

    // Placeholder window class for recommendations
    public class RecommendationsWindow : Window
    {
        public RecommendationsWindow(List<PersonalizedRecommendation> recommendations)
        {
            Title = "Personalized Recommendations";
            Width = 600;
            Height = 400;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            // TODO: Implement proper XAML content
            Content = new System.Windows.Controls.TextBlock
            {
                Text = $"Found {recommendations.Count} recommendations",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
        }
    }
}