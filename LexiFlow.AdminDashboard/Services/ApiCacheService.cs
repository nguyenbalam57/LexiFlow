using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LexiFlow.AdminDashboard.Services
{
    public class ApiCacheOptions
    {
        public bool EnableCaching { get; set; } = true;
        public int DefaultCacheTimeMinutes { get; set; } = 5;
        public int MaxCacheItems { get; set; } = 1000;
        public bool LogCacheOperations { get; set; } = true;
    }

    public interface IApiCacheService
    {
        Task<T?> GetOrAddAsync<T>(string key, Func<Task<T?>> dataFactory, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
        bool TryGetValue<T>(string key, out T? value);
        void Remove(string key);
        void Clear();
    }

    public class ApiCacheService : IApiCacheService
    {
        private readonly ConcurrentDictionary<string, CacheItem> _cache = new();
        private readonly ILogger<ApiCacheService> _logger;
        private readonly ApiCacheOptions _options;
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        public ApiCacheService(IOptions<ApiCacheOptions> options, ILogger<ApiCacheService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<T?> GetOrAddAsync<T>(string key, Func<Task<T?>> dataFactory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            if (!_options.EnableCaching)
            {
                return await dataFactory();
            }

            if (TryGetValue<T>(key, out var cachedValue))
            {
                if (_options.LogCacheOperations)
                {
                    _logger.LogDebug("Cache hit for key: {Key}", key);
                }
                return cachedValue;
            }

            try
            {
                await _semaphoreSlim.WaitAsync(cancellationToken);

                // Double-check after acquiring the lock
                if (TryGetValue<T>(key, out cachedValue))
                {
                    if (_options.LogCacheOperations)
                    {
                        _logger.LogDebug("Cache hit after lock for key: {Key}", key);
                    }
                    return cachedValue;
                }

                if (_options.LogCacheOperations)
                {
                    _logger.LogDebug("Cache miss for key: {Key}", key);
                }

                // Get data from the factory
                var data = await dataFactory();

                // Cache the result
                var actualExpiration = expiration ?? TimeSpan.FromMinutes(_options.DefaultCacheTimeMinutes);
                AddToCache(key, data, actualExpiration);

                return data;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public bool TryGetValue<T>(string key, out T? value)
        {
            if (!_options.EnableCaching)
            {
                value = default;
                return false;
            }

            if (_cache.TryGetValue(key, out var cacheItem) && !cacheItem.IsExpired)
            {
                if (cacheItem.Value is T typedValue)
                {
                    value = typedValue;
                    return true;
                }
            }

            if (_cache.TryGetValue(key, out cacheItem) && cacheItem.IsExpired)
            {
                // Remove expired item
                Remove(key);
            }

            value = default;
            return false;
        }

        public void Remove(string key)
        {
            if (_options.LogCacheOperations)
            {
                _logger.LogDebug("Removing cache key: {Key}", key);
            }

            _cache.TryRemove(key, out _);
        }

        public void Clear()
        {
            if (_options.LogCacheOperations)
            {
                _logger.LogDebug("Clearing entire cache with {Count} items", _cache.Count);
            }

            _cache.Clear();
        }

        private void AddToCache<T>(string key, T? value, TimeSpan expiration)
        {
            // If cache is at capacity, remove oldest items
            if (_cache.Count >= _options.MaxCacheItems)
            {
                if (_options.LogCacheOperations)
                {
                    _logger.LogInformation("Cache at capacity. Removing oldest items.");
                }

                // Get keys ordered by expiration time
                var oldestKeys = _cache
                    .OrderBy(x => x.Value.ExpirationTime)
                    .Take(_cache.Count - _options.MaxCacheItems + 1)
                    .Select(x => x.Key)
                    .ToList();

                foreach (var oldKey in oldestKeys)
                {
                    _cache.TryRemove(oldKey, out _);
                }
            }

            // Add new item
            var cacheItem = new CacheItem
            {
                Value = value,
                ExpirationTime = DateTime.UtcNow.Add(expiration)
            };

            _cache[key] = cacheItem;

            if (_options.LogCacheOperations)
            {
                _logger.LogDebug("Added to cache: {Key} (expires: {ExpirationTime})", key, cacheItem.ExpirationTime);
            }
        }

        private class CacheItem
        {
            public object? Value { get; set; }
            public DateTime ExpirationTime { get; set; }
            public bool IsExpired => DateTime.UtcNow > ExpirationTime;
        }
    }

    /// <summary>
    /// Extension for IApiCacheService to simplify common caching scenarios
    /// </summary>
    public static class ApiCacheServiceExtensions
    {
        /// <summary>
        /// Generate a standard cache key using object type and parameters
        /// </summary>
        public static string GenerateCacheKey<T>(string methodName, params object?[] parameters)
        {
            string typeName = typeof(T).Name;
            string paramString = string.Join("_", parameters.Select(p => p?.ToString() ?? "null"));
            return $"{typeName}_{methodName}_{paramString}";
        }

        /// <summary>
        /// Invalidate all cache entries for a specific type
        /// </summary>
        public static void InvalidateType<T>(this IApiCacheService cacheService, ILogger logger)
        {
            // This is a simplification as we don't have direct access to keys by type
            // In a real implementation, we might use a pattern or store keys by type
            logger.LogInformation("Cache invalidation requested for type {TypeName}", typeof(T).Name);
            cacheService.Clear();
        }
    }
}