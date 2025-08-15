using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Interface for API client communication
    /// </summary>
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<T> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task PostAsync(string endpoint, object data);
        Task<T> PutAsync<T>(string endpoint, object data);
        Task DeleteAsync(string endpoint);
        string BaseUrl { get; set; }
        bool IsConnected { get; }
    }
}