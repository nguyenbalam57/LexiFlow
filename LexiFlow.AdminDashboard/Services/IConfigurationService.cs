using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LexiFlow.AdminDashboard.Services
{
    public interface IConfigurationService
    {
        ApiSettings ApiSettings { get; }
        DatabaseSettings DatabaseSettings { get; }
        CacheSettings CacheSettings { get; }
        UISettings UISettings { get; }
        FeatureSettings FeatureSettings { get; }
        ApiEndpointsSettings ApiEndpoints { get; }
        ApiSyncOptions SyncOptions { get; }
        
        T GetSection<T>(string sectionName) where T : new();
        string GetConnectionString(string name);
        void ReloadConfiguration();
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly IOptionsMonitor<ApiSettings> _apiSettings;
        private readonly IOptionsMonitor<DatabaseSettings> _databaseSettings;
        private readonly IOptionsMonitor<CacheSettings> _cacheSettings;
        private readonly IOptionsMonitor<UISettings> _uiSettings;
        private readonly IOptionsMonitor<FeatureSettings> _featureSettings;
        private readonly IOptionsMonitor<ApiEndpointsSettings> _apiEndpoints;
        private readonly IOptionsMonitor<ApiSyncOptions> _syncOptions;
        private readonly IConfiguration _configuration;

        public ConfigurationService(
            IOptionsMonitor<ApiSettings> apiSettings,
            IOptionsMonitor<DatabaseSettings> databaseSettings,
            IOptionsMonitor<CacheSettings> cacheSettings,
            IOptionsMonitor<UISettings> uiSettings,
            IOptionsMonitor<FeatureSettings> featureSettings,
            IOptionsMonitor<ApiEndpointsSettings> apiEndpoints,
            IOptionsMonitor<ApiSyncOptions> syncOptions,
            IConfiguration configuration)
        {
            _apiSettings = apiSettings;
            _databaseSettings = databaseSettings;
            _cacheSettings = cacheSettings;
            _uiSettings = uiSettings;
            _featureSettings = featureSettings;
            _apiEndpoints = apiEndpoints;
            _syncOptions = syncOptions;
            _configuration = configuration;
        }

        public ApiSettings ApiSettings => _apiSettings.CurrentValue;
        public DatabaseSettings DatabaseSettings => _databaseSettings.CurrentValue;
        public CacheSettings CacheSettings => _cacheSettings.CurrentValue;
        public UISettings UISettings => _uiSettings.CurrentValue;
        public FeatureSettings FeatureSettings => _featureSettings.CurrentValue;
        public ApiEndpointsSettings ApiEndpoints => _apiEndpoints.CurrentValue;
        public ApiSyncOptions SyncOptions => _syncOptions.CurrentValue;

        public T GetSection<T>(string sectionName) where T : new()
        {
            return _configuration.GetSection(sectionName).Get<T>() ?? new T();
        }

        public string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name) ?? string.Empty;
        }

        public void ReloadConfiguration()
        {
            if (_configuration is IConfigurationRoot configRoot)
            {
                configRoot.Reload();
            }
        }
    }
}