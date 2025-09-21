using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Enhanced interface for API client communication
    /// </summary>
    public interface IApiClient : IDisposable
    {
        #region Properties
        string BaseUrl { get; set; }
        bool IsConnected { get; }
        #endregion

        #region Authentication
        void SetAuthToken(string token);
        void ClearAuthToken();
        #endregion

        #region GET Methods
        Task<T> GetAsync<T>(string endpoint);
        Task<T> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams);
        Task<T> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams, CancellationToken cancellationToken);
        #endregion

        #region POST Methods
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<T> PostAsync<T>(string endpoint, object data);
        Task PostAsync(string endpoint, object data);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken);
        #endregion

        #region PUT Methods
        Task<T> PutAsync<T>(string endpoint, object data);
        Task<T> PutAsync<T>(string endpoint, object data, CancellationToken cancellationToken);
        #endregion

        #region DELETE Methods
        Task DeleteAsync(string endpoint);
        Task DeleteAsync(string endpoint, CancellationToken cancellationToken);
        Task<T> DeleteAsync<T>(string endpoint);
        #endregion

        #region PATCH Methods
        Task<T> PatchAsync<T>(string endpoint, object data);
        #endregion

        #region File Operations
        Task<T> UploadFileAsync<T>(string endpoint, string filePath, string fileParameterName = "file", Dictionary<string, string>? additionalData = null);
        Task<byte[]> DownloadFileAsync(string endpoint);
        #endregion

        #region Health Check
        Task<bool> CheckHealthAsync();
        Task<T> GetHealthAsync<T>();
        #endregion
    }
}