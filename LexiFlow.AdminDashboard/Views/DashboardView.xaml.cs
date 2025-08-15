using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LexiFlow.AdminDashboard.Services;
using LexiFlow.AdminDashboard.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LexiFlow.AdminDashboard.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        private readonly DashboardViewModel _viewModel;
        private readonly IApiClient _apiClient;

        public DashboardView()
        {
            InitializeComponent();
            
            _apiClient = App.ServiceProvider.GetRequiredService<IApiClient>();
            _viewModel = App.ServiceProvider.GetRequiredService<DashboardViewModel>();
            
            DataContext = _viewModel;
            
            Loaded += DashboardView_Loaded;
        }

        private async void DashboardView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDashboardData();
            LoadRecentActivities();
        }

        private async Task LoadDashboardData()
        {
            try
            {
                txtLastUpdate.Text = $"Last updated: {DateTime.Now:HH:mm:ss}";
                
                // Load dashboard data from API
                var dashboardData = await _apiClient.GetAsync<AdminDashboardDto>("api/admin/dashboard");
                
                if (dashboardData != null)
                {
                    // Update system stats
                    txtTotalUsers.Text = dashboardData.SystemStats.TotalUsers.ToString("N0");
                    txtActiveUsers.Text = dashboardData.SystemStats.ActiveUsers.ToString("N0");
                    txtTotalContent.Text = dashboardData.SystemStats.TotalContent.ToString("N0");
                    txtSystemHealth.Text = dashboardData.SystemHealth.Status;
                    txtUptime.Text = dashboardData.SystemStats.SystemUptime;
                    
                    txtUsersChange.Text = $"+{dashboardData.SystemStats.NewUsersToday} today";
                    
                    // Calculate active percentage
                    var activePercentage = (double)dashboardData.SystemStats.ActiveUsers / dashboardData.SystemStats.TotalUsers * 100;
                    txtActiveChange.Text = $"{activePercentage:F1}%";
                    
                    // Update content stats
                    txtVocabTotal.Text = dashboardData.SystemStats.TotalVocabulary.ToString("N0");
                    txtKanjiTotal.Text = dashboardData.SystemStats.TotalKanji.ToString("N0");
                    txtGrammarTotal.Text = dashboardData.SystemStats.TotalGrammar.ToString("N0");
                    
                    // Update content by level if available
                    if (dashboardData.ContentStats?.ContentByLevel != null)
                    {
                        var contentByLevel = dashboardData.ContentStats.ContentByLevel;
                        
                        if (contentByLevel.ContainsKey("N5"))
                        {
                            txtVocabN5.Text = (contentByLevel["N5"] * 0.6).ToString("N0"); // Estimate vocabulary portion
                            txtKanjiN5.Text = "103"; // JLPT N5 kanji count
                            txtGrammarN5.Text = (contentByLevel["N5"] * 0.175).ToString("N0"); // Estimate grammar portion
                        }
                        
                        if (contentByLevel.ContainsKey("N4"))
                        {
                            txtVocabN4.Text = (contentByLevel["N4"] * 0.6).ToString("N0");
                            txtKanjiN4.Text = "167"; // JLPT N4 kanji count
                            txtGrammarN4.Text = (contentByLevel["N4"] * 0.18).ToString("N0");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error - could show notification or log
                txtLastUpdate.Text = $"Error loading data: {ex.Message}";
            }
        }

        private void LoadRecentActivities()
        {
            // Sample recent activities - in real app, load from API
            var activities = new List<RecentActivity>
            {
                new RecentActivity { Activity = "New user registered", User = "user123@example.com", Time = "2 min ago" },
                new RecentActivity { Activity = "Vocabulary updated", User = "admin", Time = "5 min ago" },
                new RecentActivity { Activity = "Kanji collection added", User = "editor1", Time = "12 min ago" },
                new RecentActivity { Activity = "User completed JLPT N5 test", User = "student456", Time = "15 min ago" },
                new RecentActivity { Activity = "Grammar rule modified", User = "admin", Time = "23 min ago" },
                new RecentActivity { Activity = "Media file uploaded", User = "content_manager", Time = "31 min ago" },
                new RecentActivity { Activity = "User study session", User = "learner789", Time = "45 min ago" },
                new RecentActivity { Activity = "System backup completed", User = "system", Time = "1 hour ago" }
            };
            
            recentActivitiesList.ItemsSource = activities;
        }

        public async void RefreshData()
        {
            await LoadDashboardData();
            LoadRecentActivities();
        }

        private void AddNewVocabulary(object sender, RoutedEventArgs e)
        {
            // Navigate to vocabulary management with add mode
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToVocabulary();
            }
        }

        private void AddNewKanji(object sender, RoutedEventArgs e)
        {
            // Navigate to kanji management with add mode
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToKanji();
            }
        }

        private void ViewAnalytics(object sender, RoutedEventArgs e)
        {
            // Navigate to analytics
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.NavigateToAnalytics();
            }
        }

        private void ExportData(object sender, RoutedEventArgs e)
        {
            // TODO: Implement data export functionality
            MessageBox.Show("Data export functionality will be implemented soon.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    // Helper classes for data binding
    public class RecentActivity
    {
        public string Activity { get; set; }
        public string User { get; set; }
        public string Time { get; set; }
    }

    // DTO classes (should be moved to a shared project in real implementation)
    public class AdminDashboardDto
    {
        public SystemStatsDto SystemStats { get; set; }
        public ContentStatsDto ContentStats { get; set; }
        public SystemHealthDto SystemHealth { get; set; }
    }

    public class SystemStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersToday { get; set; }
        public int TotalContent { get; set; }
        public int TotalVocabulary { get; set; }
        public int TotalKanji { get; set; }
        public int TotalGrammar { get; set; }
        public string SystemUptime { get; set; }
    }

    public class ContentStatsDto
    {
        public Dictionary<string, int> ContentByLevel { get; set; }
    }

    public class SystemHealthDto
    {
        public string Status { get; set; }
    }
}