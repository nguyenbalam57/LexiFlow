using LexiFlow.AdminDashboard.Models;
using LexiFlow.AdminDashboard.Services;
using LexiFlow.AdminDashboard.ViewModels.Base;
using LiveCharts;
using LiveCharts.Wpf;
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
    public class AdminDashboardViewModel : ViewModelBase
    {
        private readonly IAdminDashboardService _dashboardService;
        private readonly ILogger<AdminDashboardViewModel> _logger;

        private DashboardStatistics _statistics = new();
        private bool _isLoading;
        private string _errorMessage = string.Empty;
        private string _currentUser = string.Empty;
        private ObservableCollection<UserActivity> _recentActivities = new();
        private ObservableCollection<MenuItemModel> _menuItems = new();
        private MenuItemModel? _selectedMenuItem;

        public DashboardStatistics Statistics
        {
            get => _statistics;
            set => SetProperty(ref _statistics, value);
        }

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

        public string CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public ObservableCollection<UserActivity> RecentActivities
        {
            get => _recentActivities;
            set => SetProperty(ref _recentActivities, value);
        }

        public ObservableCollection<MenuItemModel> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }

        public MenuItemModel? SelectedMenuItem
        {
            get => _selectedMenuItem;
            set => SetProperty(ref _selectedMenuItem, value);
        }

        // Chart Properties
        public SeriesCollection UserSeries { get; set; } = new SeriesCollection();
        public SeriesCollection ContentSeries { get; set; } = new SeriesCollection();
        public string[] MonthLabels { get; set; } = Array.Empty<string>();
        public Func<double, string> YFormatter { get; set; } = value => value.ToString("N0");

        public ICommand RefreshCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand OpenUserManagementCommand { get; }
        public ICommand OpenVocabularyManagementCommand { get; }
        public ICommand OpenSystemConfigCommand { get; }

        public AdminDashboardViewModel(
            IAdminDashboardService dashboardService,
            ILogger<AdminDashboardViewModel> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;

            // Initialize commands
            RefreshCommand = new AsyncRelayCommand(LoadDashboardDataAsync);
            NavigateCommand = new RelayCommand(Navigate);
            OpenUserManagementCommand = new RelayCommand(_ => Navigate("UserManagement"));
            OpenVocabularyManagementCommand = new RelayCommand(_ => Navigate("VocabularyManagement"));
            OpenSystemConfigCommand = new RelayCommand(_ => Navigate("SystemConfig"));

            // Initialize menu
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            MenuItems.Clear();

            MenuItems.Add(new MenuItemModel
            {
                Name = "Dashboard",
                Icon = "ViewDashboard",
                ViewName = "Dashboard",
                IsSelected = true
            });

            MenuItems.Add(new MenuItemModel
            {
                Name = "User Management",
                Icon = "AccountMultiple",
                ViewName = "UserManagement"
            });

            MenuItems.Add(new MenuItemModel
            {
                Name = "Content Management",
                Icon = "BookOpenVariant",
                ViewName = "ContentManagement",
                IsExpanded = false,
                SubItems = new List<MenuItemModel>
                {
                    new MenuItemModel
                    {
                        Name = "Vocabulary",
                        Icon = "TextBox",
                        ViewName = "VocabularyManagement"
                    },
                    new MenuItemModel
                    {
                        Name = "Kanji",
                        Icon = "AlphabetJapanese",
                        ViewName = "KanjiManagement"
                    },
                    new MenuItemModel
                    {
                        Name = "Grammar",
                        Icon = "TextBoxCheck",
                        ViewName = "GrammarManagement"
                    }
                }
            });

            MenuItems.Add(new MenuItemModel
            {
                Name = "Test Management",
                Icon = "TestTube",
                ViewName = "TestManagement"
            });

            MenuItems.Add(new MenuItemModel
            {
                Name = "Submissions",
                Icon = "InboxArrowUp",
                ViewName = "Submissions"
            });

            MenuItems.Add(new MenuItemModel
            {
                Name = "System",
                Icon = "Cog",
                ViewName = "System",
                IsExpanded = false,
                SubItems = new List<MenuItemModel>
                {
                    new MenuItemModel
                    {
                        Name = "Configuration",
                        Icon = "Tune",
                        ViewName = "SystemConfig"
                    },
                    new MenuItemModel
                    {
                        Name = "Backup & Restore",
                        Icon = "DatabaseSync",
                        ViewName = "BackupRestore"
                    },
                    new MenuItemModel
                    {
                        Name = "Activity Logs",
                        Icon = "FileDocument",
                        ViewName = "ActivityLogs"
                    }
                }
            });

            SelectedMenuItem = MenuItems.FirstOrDefault();
        }

        private void Navigate(object? parameter)
        {
            if (parameter is not string viewName)
                return;

            // Find the menu item with the specified view name
            var menuItem = FindMenuItemByViewName(viewName);
            if (menuItem != null)
            {
                // Deselect all items
                foreach (var item in MenuItems)
                {
                    item.IsSelected = false;
                    foreach (var subItem in item.SubItems)
                    {
                        subItem.IsSelected = false;
                    }
                }

                // Select the target item
                menuItem.IsSelected = true;

                // If it's a sub-item, expand its parent
                if (menuItem.ViewName != viewName)
                {
                    var parentItem = MenuItems.FirstOrDefault(m => m.SubItems.Contains(menuItem));
                    if (parentItem != null)
                    {
                        parentItem.IsExpanded = true;
                    }
                }

                SelectedMenuItem = menuItem;

                // Refresh menu to update UI
                OnPropertyChanged(nameof(MenuItems));
            }
        }

        private MenuItemModel? FindMenuItemByViewName(string viewName)
        {
            foreach (var item in MenuItems)
            {
                if (item.ViewName == viewName)
                    return item;

                var subItem = item.SubItems.FirstOrDefault(s => s.ViewName == viewName);
                if (subItem != null)
                    return subItem;
            }

            return null;
        }

        public async Task LoadDashboardDataAsync(object? parameter = null)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Load dashboard statistics
                Statistics = await _dashboardService.GetDashboardStatisticsAsync();

                // Update recent activities
                RecentActivities.Clear();
                foreach (var activity in Statistics.RecentUserActivities)
                {
                    RecentActivities.Add(activity);
                }

                // Update charts
                UpdateCharts();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                ErrorMessage = $"Error loading dashboard data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void UpdateCharts()
        {
            try
            {
                // Process monthly user stats
                var userMonths = Statistics.MonthlyUserStats
                    .OrderBy(m => m.Month)
                    .ToList();

                if (userMonths.Any())
                {
                    // Generate labels for last 12 months
                    MonthLabels = userMonths
                        .Select(m => m.Month.ToString("MMM yyyy"))
                        .ToArray();

                    // Create user series
                    UserSeries.Clear();
                    UserSeries.Add(new LineSeries
                    {
                        Title = "New Users",
                        Values = new ChartValues<int>(userMonths.Select(m => m.Value)),
                        PointGeometry = null,
                        LineSmoothness = 0.5
                    });
                }

                // Process content distribution
                ContentSeries.Clear();
                if (Statistics.ContentByCategory.Any())
                {
                    ContentSeries.Add(new PieSeries
                    {
                        Title = "Vocabulary Distribution",
                        Values = new ChartValues<int>(Statistics.ContentByCategory.Values),
                        DataLabels = true,
                        LabelPoint = chartPoint =>
                            $"{Statistics.ContentByCategory.Keys.ElementAt((int)chartPoint.Key)}: {chartPoint.Y} ({chartPoint.Participation:P1})"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating charts");
            }
        }
    }
}
