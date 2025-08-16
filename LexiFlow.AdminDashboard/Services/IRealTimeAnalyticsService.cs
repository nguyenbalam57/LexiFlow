using System;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Interface for real-time analytics service
    /// </summary>
    public interface IRealTimeAnalyticsService
    {
        bool IsConnected { get; }
        string ConnectionStatus { get; }
        
        event Action<object> DataReceived;
        event Action<bool> ConnectionStateChanged;
        // Add these events
        event EventHandler<object> DashboardDataUpdated;
        event EventHandler<object> PerformanceDataUpdated;
        event EventHandler<string> StudyActivityReported;
        event EventHandler<string> TestCompleted;
        event EventHandler<object> GoalProgressUpdated;
        event EventHandler<string> ConnectionStatusChanged;

        Task StartAsync();
        Task StopAsync();
        Task RequestDataUpdateAsync(string dataType);
        Task SubscribeToDataAsync(params string[] dataTypes);
        Task UnsubscribeFromDataAsync(params string[] dataTypes);
        Task<bool> CheckConnectionHealthAsync();
        ValueTask DisposeAsync();
        // Add these methods
        Task ConnectAsync();
        Task DisconnectAsync();
        Task SubscribeToChannelAsync(string channel);
        Task RequestDashboardUpdateAsync(int days);
        Task RequestPerformanceUpdateAsync();

    }
}