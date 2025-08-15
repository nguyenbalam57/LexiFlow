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

        Task StartAsync();
        Task StopAsync();
        Task RequestDataUpdateAsync(string dataType);
        Task SubscribeToDataAsync(params string[] dataTypes);
        Task UnsubscribeFromDataAsync(params string[] dataTypes);
        Task<bool> CheckConnectionHealthAsync();
        ValueTask DisposeAsync();
    }
}