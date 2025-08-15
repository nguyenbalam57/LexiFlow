using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LexiFlow.AdminDashboard.Views;
using LexiFlow.AdminDashboard.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows.Threading;
using System;

namespace LexiFlow.AdminDashboard;

/// <summary>
/// Interaction logic for MainWindow.xaml with Dependency Injection
/// </summary>
public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timeTimer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MainWindow> _logger;

    public MainWindow(IServiceProvider serviceProvider, ILogger<MainWindow> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        
        InitializeComponent();
        
        _logger.LogInformation("MainWindow initializing...");
        
        // Initialize timers
        _timeTimer = new DispatcherTimer();
        _timeTimer.Interval = TimeSpan.FromSeconds(1);
        _timeTimer.Tick += UpdateDateTime;
        _timeTimer.Start();

        // Initialize UI
        UpdateDateTime(null, null);
        
        // Load dashboard by default
        NavigateToDashboard();
        
        _logger.LogInformation("MainWindow initialized successfully");
    }

    private void NavigateToView(object sender, RoutedEventArgs e)
    {
        try
        {
            if (sender is Button button)
            {
                _logger.LogDebug("Navigating to view: {ButtonName}", button.Name);
                
                switch (button.Name)
                {
                    case "btnDashboard":
                        NavigateToDashboard();
                        break;
                    case "btnUsers":
                        NavigateToUsers();
                        break;
                    case "btnVocabulary":
                        NavigateToVocabulary();
                        break;
                    case "btnKanji":
                        NavigateToKanji();
                        break;
                    case "btnGrammar":
                        NavigateToGrammar();
                        break;
                    case "btnMedia":
                        NavigateToMedia();
                        break;
                    case "btnExams":
                        NavigateToExams();
                        break;
                    case "btnAnalytics":
                        NavigateToAnalytics();
                        break;
                    case "btnSettings":
                        NavigateToSettings();
                        break;
                    default:
                        _logger.LogWarning("Unknown navigation button: {ButtonName}", button.Name);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during navigation");
            MessageBox.Show($"Navigation error: {ex.Message}", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void NavigateToDashboard()
    {
        txtCurrentView.Text = "Dashboard";
        var dashboardView = _serviceProvider.GetRequiredService<DashboardView>();
        contentFrame.Content = dashboardView;
        _logger.LogDebug("Navigated to Dashboard");
    }

    public void NavigateToUsers()
    {
        txtCurrentView.Text = "User Management";
        var userView = _serviceProvider.GetRequiredService<UserManagementView>();
        contentFrame.Content = userView;
        _logger.LogDebug("Navigated to User Management");
    }

    public void NavigateToVocabulary()
    {
        txtCurrentView.Text = "Vocabulary Management";
        var vocabularyView = _serviceProvider.GetRequiredService<VocabularyManagementView>();
        contentFrame.Content = vocabularyView;
        _logger.LogDebug("Navigated to Vocabulary Management");
    }

    public void NavigateToKanji()
    {
        txtCurrentView.Text = "Kanji Management";
        var kanjiView = _serviceProvider.GetRequiredService<KanjiManagementView>();
        contentFrame.Content = kanjiView;
        _logger.LogDebug("Navigated to Kanji Management");
    }

    public void NavigateToGrammar()
    {
        txtCurrentView.Text = "Grammar Management";
        var grammarView = _serviceProvider.GetRequiredService<GrammarManagementView>();
        contentFrame.Content = grammarView;
        _logger.LogDebug("Navigated to Grammar Management");
    }

    public void NavigateToMedia()
    {
        txtCurrentView.Text = "Media File Management";
        var mediaView = _serviceProvider.GetRequiredService<MediaManagementView>();
        contentFrame.Content = mediaView;
        _logger.LogDebug("Navigated to Media Management");
    }

    public void NavigateToExams()
    {
        txtCurrentView.Text = "Exam Management";
        var examView = _serviceProvider.GetRequiredService<ExamManagementView>();
        contentFrame.Content = examView;
        _logger.LogDebug("Navigated to Exam Management");
    }

    public void NavigateToAnalytics()
    {
        txtCurrentView.Text = "Analytics & Reports";
        var analyticsView = _serviceProvider.GetRequiredService<AnalyticsView>();
        contentFrame.Content = analyticsView;
        _logger.LogDebug("Navigated to Analytics");
    }

    public void NavigateToSettings()
    {
        txtCurrentView.Text = "System Settings";
        var settingsView = _serviceProvider.GetRequiredService<SettingsView>();
        contentFrame.Content = settingsView;
        _logger.LogDebug("Navigated to Settings");
    }

    private async void RefreshData(object sender, RoutedEventArgs e)
    {
        try
        {
            ShowLoading(true);
            _logger.LogInformation("Refreshing current view data");
            
            // Refresh current view data
            if (contentFrame.Content is UserControl currentView)
            {
                // Check if view has DataContext with RefreshAsync method
                if (currentView.DataContext != null)
                {
                    var dataContext = currentView.DataContext;
                    var refreshMethod = dataContext.GetType().GetMethod("RefreshAsync");
                    
                    if (refreshMethod != null)
                    {
                        var task = refreshMethod.Invoke(dataContext, null) as Task;
                        if (task != null)
                        {
                            await task;
                        }
                    }
                    else
                    {
                        // Fallback to synchronous refresh
                        var syncRefreshMethod = dataContext.GetType().GetMethod("RefreshData");
                        syncRefreshMethod?.Invoke(dataContext, null);
                    }
                }
            }
            
            _logger.LogInformation("Data refresh completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data refresh");
            MessageBox.Show($"Refresh error: {ex.Message}", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            ShowLoading(false);
        }
    }

    private void ShowNotifications(object sender, RoutedEventArgs e)
    {
        try
        {
            // TODO: Implement notifications panel
            MessageBox.Show("Notifications feature will be implemented soon.", "Info", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error showing notifications");
        }
    }

    private void ShowUserProfile(object sender, RoutedEventArgs e)
    {
        try
        {
            // TODO: Implement user profile dialog
            MessageBox.Show("User profile feature will be implemented soon.", "Info", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error showing user profile");
        }
    }

    private void UpdateDateTime(object sender, EventArgs e)
    {
        try
        {
            txtCurrentDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy - HH:mm:ss");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating date time");
        }
    }

    private void ShowLoading(bool show)
    {
        try
        {
            loadingOverlay.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating loading overlay");
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        try
        {
            _timeTimer?.Stop();
            _logger.LogInformation("MainWindow closed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during window close");
        }
        finally
        {
            base.OnClosed(e);
        }
    }
}