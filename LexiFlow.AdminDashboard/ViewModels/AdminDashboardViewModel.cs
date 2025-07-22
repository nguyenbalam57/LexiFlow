using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LexiFlow.AdminDashboard.Models;
using LexiFlow.AdminDashboard.Services;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Extensions.Logging;

namespace LexiFlow.AdminDashboard.ViewModels
{
    /// <summary>
    /// View model for the admin dashboard
    /// </summary>
    public class AdminDashboardViewModel : ViewModelBase
    {
        private readonly IAdminDashboardService _dashboardService;
        private readonly ILogger<AdminDashboardViewModel> _logger;
        private readonly IApiSyncService _syncService;

        private DashboardStatistics _statistics = new DashboardStatistics();
        private ObservableCollection<UserActivity> _recentActivities = new ObservableCollection<UserActivity>();
        private bool _isRefreshing;
        private string _syncStatus = "Not synced";
        private int _syncProgress;
        private DateTime? _lastSyncTime;

        /// <summary>
        /// Constructor
        /// </summary>
        public AdminDashboardViewModel(
            IAdminDashboardService dashboardService,
            IApiSyncService syncService,
            ILogger<AdminDashboardViewModel> logger)
        {
            _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
            _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Initialize commands
            RefreshCommand = CreateCommand(RefreshAsync);
            SyncCommand = CreateCommand(SyncAsync);

            // Initialize chart series
            UserSeries = new SeriesCollection();
            ContentSeries = new SeriesCollection();

            // Subscribe to sync events
            _syncService.SyncStatusChanged += OnSyncStatusChanged;

            // Get initial sync status
            UpdateSyncInfo();
        }

        /// <summary>
        /// Dashboard statistics
        /// </summary>
        public DashboardStatistics Statistics
        {
            get => _statistics;
            set => SetProperty(ref _statistics, value);
        }

        /// <summary>
        /// Recent user activities
        /// </summary>
        public ObservableCollection<UserActivity> RecentActivities
        {
            get => _recentActivities;
            set => SetProperty(ref _recentActivities, value);
        }

        /// <summary>
        /// Flag indicating if the view model is refreshing data
        /// </summary>
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        /// <summary>
        /// Sync status
        /// </summary>
        public string SyncStatus
        {
            get => _syncStatus;
            set => SetProperty(ref _syncStatus, value);
        }

        /// <summary>
        /// Sync progress
        /// </summary>
        public int SyncProgress
        {
            get => _syncProgress;
            set => SetProperty(ref _syncProgress, value);
        }

        /// <summary>
        /// Last sync time
        /// </summary>
        public DateTime? LastSyncTime
        {
            get => _lastSyncTime;
            set => SetProperty(ref _lastSyncTime, value);
        }

        /// <summary>
        /// User chart series
        /// </summary>
        public SeriesCollection UserSeries { get; private set; }

        /// <summary>
        /// Content chart series
        /// </summary>
        public SeriesCollection ContentSeries { get; private set; }

        /// <summary>
        /// Month labels for charts
        /// </summary>
        public string[] MonthLabels { get; private set; } = Array.Empty<string>();

        /// <summary>
        /// Refresh command
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Sync command
        /// </summary>
        public ICommand SyncCommand { get; }

        /// <summary>
        /// Loads the dashboard data
        /// </summary>
        protected override async Task LoadDataAsync(object? parameter = null)
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

        /// <summary>
        /// Refreshes the dashboard data
        /// </summary>
        private async Task RefreshAsync(object? parameter = null)
        {
            try
            {
                IsRefreshing = true;
                ErrorMessage = string.Empty;

                await LoadAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing dashboard data");
                ErrorMessage = $"Error refreshing dashboard data: {ex.Message}";
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Synchronizes the data
        /// </summary>
        private async Task SyncAsync(object? parameter = null)
        {
            try
            {
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                // Start synchronization
                var result = await _syncService.SyncAllAsync();

                if (result.Success)
                {
                    SuccessMessage = result.Message;

                    // Refresh dashboard data
                    await LoadAsync();
                }
                else
                {
                    ErrorMessage = result.Message;
                }

                // Update sync info
                UpdateSyncInfo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error synchronizing data");
                ErrorMessage = $"Error synchronizing data: {ex.Message}";
            }
        }

        /// <summary>
        /// Updates the charts
        /// </summary>
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

        /// <summary>
        /// Updates the sync information
        /// </summary>
        private void UpdateSyncInfo()
        {
            var syncInfo = _syncService.GetSyncInfo();

            SyncStatus = syncInfo.Status.ToString();
            SyncProgress = syncInfo.Progress;
            LastSyncTime = syncInfo.LastSyncTime;
        }

        /// <summary>
        /// Handles the sync status changed event
        /// </summary>
        private void OnSyncStatusChanged(object? sender, SyncStatusChangedEventArgs e)
        {
            SyncStatus = e.Status.ToString();
            SyncProgress = e.Progress;

            if (e.Status == SyncStatus.Completed || e.Status == SyncStatus.Failed || e.Status == SyncStatus.Cancelled)
            {
                LastSyncTime = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Disposes the view model
        /// </summary>
        /// <param name="disposing">True if disposing, false if finalizing</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unsubscribe from events
                _syncService.SyncStatusChanged -= OnSyncStatusChanged;
            }

            base.Dispose(disposing);
        }
    }
}