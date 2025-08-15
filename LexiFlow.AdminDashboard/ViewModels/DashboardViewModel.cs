using LexiFlow.AdminDashboard.ViewModels.Base;
using LexiFlow.AdminDashboard.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.Logging;

namespace LexiFlow.AdminDashboard.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<DashboardViewModel> _logger;

        public DashboardViewModel(IApiClient apiClient, ILogger<DashboardViewModel> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
            
            RefreshCommand = new AsyncRelayCommand(async (param) => await RefreshData());
        }

        public ICommand RefreshCommand { get; }

        private async Task RefreshData()
        {
            try
            {
                // Refresh dashboard data
                _logger.LogInformation("Refreshing dashboard data");
                
                // TODO: Implement actual data refresh logic
                await Task.Delay(1000); // Simulate loading
                
                OnPropertyChanged(nameof(RefreshCommand));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing dashboard data");
            }
        }
    }
}