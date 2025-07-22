using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LexiFlow.AdminDashboard.Models;
using LexiFlow.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LexiFlow.AdminDashboard.Services
{
    public class ApiSyncOptions
    {
        /// <summary>
        /// Maximum number of items to sync in a single batch
        /// </summary>
        public int MaxBatchSize { get; set; } = 100;

        /// <summary>
        /// Auto sync interval in minutes (0 to disable)
        /// </summary>
        public int AutoSyncIntervalMinutes { get; set; } = 5;

        /// <summary>
        /// Enable background synchronization
        /// </summary>
        public bool EnableBackgroundSync { get; set; } = true;

        /// <summary>
        /// Sync on startup
        /// </summary>
        public bool SyncOnStartup { get; set; } = true;

        /// <summary>
        /// Sync direction (Push, Pull, Both)
        /// </summary>
        public SyncDirection Direction { get; set; } = SyncDirection.Both;

        /// <summary>
        /// Retry count for failed sync operations
        /// </summary>
        public int RetryCount { get; set; } = 3;

        /// <summary>
        /// Base delay between retries in seconds
        /// </summary>
        public int RetryDelaySeconds { get; set; } = 5;

        /// <summary>
        /// Tables to sync
        /// </summary>
        public List<string> SyncTables { get; set; } = new List<string>
        {
            "Users",
            "Roles",
            "Categories",
            "VocabularyItems",
            "Lessons",
            "Courses",
            "Exercises"
        };
    }

    public enum SyncDirection
    {
        Push,
        Pull,
        Both
    }

    public enum SyncStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Failed,
        Cancelled
    }

    public class SyncResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int ItemsProcessed { get; set; }
        public int ItemsCreated { get; set; }
        public int ItemsUpdated { get; set; }
        public int ItemsDeleted { get; set; }
        public int Errors { get; set; }
        public TimeSpan Duration { get; set; }
        public string? TableName { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }

    public class SyncInfo
    {
        public SyncStatus Status { get; set; } = SyncStatus.NotStarted;
        public DateTime? LastSyncTime { get; set; }
        public bool IsSyncing { get; set; }
        public string? CurrentOperation { get; set; }
        public int Progress { get; set; }
        public int TotalItems { get; set; }
        public Dictionary<string, SyncResult> TableResults { get; set; } = new Dictionary<string, SyncResult>();
        public string? LastError { get; set; }
        public DateTime? NextScheduledSync { get; set; }
    }

    public interface IApiSyncService
    {
        /// <summary>
        /// Gets the current sync information
        /// </summary>
        SyncInfo GetSyncInfo();

        /// <summary>
        /// Starts a full synchronization process
        /// </summary>
        Task<SyncResult> SyncAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Synchronizes a specific table
        /// </summary>
        Task<SyncResult> SyncTableAsync(string tableName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Pushes local changes to the server
        /// </summary>
        Task<SyncResult> PushChangesAsync(string tableName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Pulls remote changes from the server
        /// </summary>
        Task<SyncResult> PullChangesAsync(string tableName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels the current synchronization process
        /// </summary>
        void CancelSync();

        /// <summary>
        /// Clears the sync log
        /// </summary>
        void ClearSyncLog();

        /// <summary>
        /// Gets the sync log entries
        /// </summary>
        List<SyncLogEntry> GetSyncLog(int maxEntries = 100);

        /// <summary>
        /// Event raised when sync status changes
        /// </summary>
        event EventHandler<SyncStatusChangedEventArgs> SyncStatusChanged;
    }

    public class SyncStatusChangedEventArgs : EventArgs
    {
        public SyncStatus Status { get; }
        public string? TableName { get; }
        public int Progress { get; }
        public int TotalItems { get; }
        public string? Message { get; }

        public SyncStatusChangedEventArgs(SyncStatus status, string? tableName = null, int progress = 0, int totalItems = 0, string? message = null)
        {
            Status = status;
            TableName = tableName;
            Progress = progress;
            TotalItems = totalItems;
            Message = message;
        }
    }

    public class SyncLogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Operation { get; set; } = string.Empty;
        public string? TableName { get; set; }
        public SyncStatus Status { get; set; }
        public string? Message { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? ItemsProcessed { get; set; }
    }

    public class ApiSyncService : IApiSyncService
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<ApiSyncService> _logger;
        private readonly ApiSyncOptions _options;
        private readonly SyncInfo _syncInfo = new SyncInfo();
        private readonly List<SyncLogEntry> _syncLog = new List<SyncLogEntry>();
        private readonly object _syncLock = new object();
        private CancellationTokenSource? _cancellationTokenSource;
        private Timer? _syncTimer;

        public event EventHandler<SyncStatusChangedEventArgs>? SyncStatusChanged;

        public ApiSyncService(
            IApiClient apiClient,
            IOptions<ApiSyncOptions> options,
            ILogger<ApiSyncService> logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _options = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Start timer for background sync if enabled
            if (_options.EnableBackgroundSync && _options.AutoSyncIntervalMinutes > 0)
            {
                var interval = TimeSpan.FromMinutes(_options.AutoSyncIntervalMinutes);
                _syncTimer = new Timer(async _ => await AutoSyncAsync(), null, interval, interval);
                _syncInfo.NextScheduledSync = DateTime.UtcNow.Add(interval);
            }

            // Perform initial sync if configured
            if (_options.SyncOnStartup)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        // Wait a short time to allow application to initialize
                        await Task.Delay(5000);
                        await SyncAllAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error during startup sync");
                    }
                });
            }
        }

        public SyncInfo GetSyncInfo()
        {
            lock (_syncLock)
            {
                return new SyncInfo
                {
                    Status = _syncInfo.Status,
                    LastSyncTime = _syncInfo.LastSyncTime,
                    IsSyncing = _syncInfo.IsSyncing,
                    CurrentOperation = _syncInfo.CurrentOperation,
                    Progress = _syncInfo.Progress,
                    TotalItems = _syncInfo.TotalItems,
                    TableResults = new Dictionary<string, SyncResult>(_syncInfo.TableResults),
                    LastError = _syncInfo.LastError,
                    NextScheduledSync = _syncInfo.NextScheduledSync
                };
            }
        }

        public async Task<SyncResult> SyncAllAsync(CancellationToken cancellationToken = default)
        {
            if (_syncInfo.IsSyncing)
            {
                _logger.LogWarning("Sync already in progress");
                return new SyncResult { Success = false, Message = "Sync already in progress" };
            }

            lock (_syncLock)
            {
                _syncInfo.IsSyncing = true;
                _syncInfo.Status = SyncStatus.InProgress;
                _syncInfo.Progress = 0;
                _syncInfo.CurrentOperation = "Starting full sync";
                _syncInfo.TotalItems = _options.SyncTables.Count;
                _syncInfo.LastError = null;

                _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            }

            OnSyncStatusChanged(SyncStatus.InProgress, null, 0, _options.SyncTables.Count, "Starting full sync");
            AddSyncLogEntry("SyncAll", null, SyncStatus.InProgress, "Starting full sync");

            var startTime = DateTime.UtcNow;
            var overallResult = new SyncResult { TableName = "All", Timestamp = startTime };

            try
            {
                for (int i = 0; i < _options.SyncTables.Count; i++)
                {
                    string tableName = _options.SyncTables[i];

                    // Update progress
                    lock (_syncLock)
                    {
                        _syncInfo.CurrentOperation = $"Syncing {tableName}";
                        _syncInfo.Progress = i;
                    }

                    OnSyncStatusChanged(SyncStatus.InProgress, tableName, i, _options.SyncTables.Count, $"Syncing {tableName}");

                    // Check if cancelled
                    if (_cancellationTokenSource!.Token.IsCancellationRequested)
                    {
                        _logger.LogInformation("Sync cancelled during full sync");
                        throw new OperationCanceledException();
                    }

                    // Sync the table
                    var result = await SyncTableAsync(tableName, _cancellationTokenSource.Token);

                    // Aggregate results
                    overallResult.ItemsProcessed += result.ItemsProcessed;
                    overallResult.ItemsCreated += result.ItemsCreated;
                    overallResult.ItemsUpdated += result.ItemsUpdated;
                    overallResult.ItemsDeleted += result.ItemsDeleted;
                    overallResult.Errors += result.Errors;

                    // Store result for this table
                    lock (_syncLock)
                    {
                        _syncInfo.TableResults[tableName] = result;
                    }

                    if (!result.Success)
                    {
                        _logger.LogError("Sync failed for table {TableName}: {Message}", tableName, result.Message);
                        overallResult.ErrorMessages.AddRange(result.ErrorMessages);
                    }
                }

                overallResult.Success = overallResult.Errors == 0;
                overallResult.Duration = DateTime.UtcNow - startTime;
                overallResult.Message = overallResult.Success
                    ? $"Full sync completed successfully in {overallResult.Duration.TotalSeconds:F2}s"
                    : $"Full sync completed with {overallResult.Errors} errors in {overallResult.Duration.TotalSeconds:F2}s";

                lock (_syncLock)
                {
                    _syncInfo.Status = overallResult.Success ? SyncStatus.Completed : SyncStatus.Failed;
                    _syncInfo.LastSyncTime = DateTime.UtcNow;
                    _syncInfo.IsSyncing = false;
                    _syncInfo.CurrentOperation = null;
                    _syncInfo.Progress = _options.SyncTables.Count;
                    _syncInfo.LastError = overallResult.Success ? null : overallResult.Message;

                    if (_options.EnableBackgroundSync && _options.AutoSyncIntervalMinutes > 0)
                    {
                        _syncInfo.NextScheduledSync = DateTime.UtcNow.AddMinutes(_options.AutoSyncIntervalMinutes);
                    }
                }

                OnSyncStatusChanged(
                    overallResult.Success ? SyncStatus.Completed : SyncStatus.Failed,
                    null,
                    _options.SyncTables.Count,
                    _options.SyncTables.Count,
                    overallResult.Message);

                AddSyncLogEntry(
                    "SyncAll",
                    null,
                    overallResult.Success ? SyncStatus.Completed : SyncStatus.Failed,
                    overallResult.Message,
                    overallResult.Duration,
                    overallResult.ItemsProcessed);

                return overallResult;
            }
            catch (OperationCanceledException)
            {
                var duration = DateTime.UtcNow - startTime;
                var message = $"Sync cancelled after {duration.TotalSeconds:F2}s";

                lock (_syncLock)
                {
                    _syncInfo.Status = SyncStatus.Cancelled;
                    _syncInfo.LastSyncTime = DateTime.UtcNow;
                    _syncInfo.IsSyncing = false;
                    _syncInfo.CurrentOperation = null;
                    _syncInfo.LastError = message;

                    if (_options.EnableBackgroundSync && _options.AutoSyncIntervalMinutes > 0)
                    {
                        _syncInfo.NextScheduledSync = DateTime.UtcNow.AddMinutes(_options.AutoSyncIntervalMinutes);
                    }
                }

                OnSyncStatusChanged(SyncStatus.Cancelled, null, _syncInfo.Progress, _options.SyncTables.Count, message);
                AddSyncLogEntry("SyncAll", null, SyncStatus.Cancelled, message, duration);

                return new SyncResult
                {
                    Success = false,
                    Message = message,
                    Duration = duration,
                    TableName = "All",
                    Timestamp = startTime
                };
            }
            catch (Exception ex)
            {
                var duration = DateTime.UtcNow - startTime;
                var message = $"Sync failed: {ex.Message}";

                _logger.LogError(ex, "Error during full sync");

                lock (_syncLock)
                {
                    _syncInfo.Status = SyncStatus.Failed;
                    _syncInfo.LastSyncTime = DateTime.UtcNow;
                    _syncInfo.IsSyncing = false;
                    _syncInfo.CurrentOperation = null;
                    _syncInfo.LastError = message;

                    if (_options.EnableBackgroundSync && _options.AutoSyncIntervalMinutes > 0)
                    {
                        _syncInfo.NextScheduledSync = DateTime.UtcNow.AddMinutes(_options.AutoSyncIntervalMinutes);
                    }
                }

                OnSyncStatusChanged(SyncStatus.Failed, null, _syncInfo.Progress, _options.SyncTables.Count, message);
                AddSyncLogEntry("SyncAll", null, SyncStatus.Failed, message, duration);

                return new SyncResult
                {
                    Success = false,
                    Message = message,
                    Duration = duration,
                    TableName = "All",
                    Timestamp = startTime,
                    ErrorMessages = { ex.Message }
                };
            }
            finally
            {
                // Clean up
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }
        }

        public async Task<SyncResult> SyncTableAsync(string tableName, CancellationToken cancellationToken = default)
        {
            if (!_options.SyncTables.Contains(tableName))
            {
                var message = $"Table {tableName} is not configured for synchronization";
                _logger.LogWarning(message);
                return new SyncResult { Success = false, Message = message, TableName = tableName };
            }

            var startTime = DateTime.UtcNow;
            var result = new SyncResult { TableName = tableName, Timestamp = startTime };

            OnSyncStatusChanged(SyncStatus.InProgress, tableName, 0, 100, $"Starting sync for {tableName}");
            AddSyncLogEntry("SyncTable", tableName, SyncStatus.InProgress, $"Starting sync for {tableName}");

            try
            {
                switch (_options.Direction)
                {
                    case SyncDirection.Push:
                        result = await PushChangesAsync(tableName, cancellationToken);
                        break;

                    case SyncDirection.Pull:
                        result = await PullChangesAsync(tableName, cancellationToken);
                        break;

                    case SyncDirection.Both:
                        // First push local changes, then pull remote changes
                        var pushResult = await PushChangesAsync(tableName, cancellationToken);
                        var pullResult = await PullChangesAsync(tableName, cancellationToken);

                        // Combine results
                        result.Success = pushResult.Success && pullResult.Success;
                        result.Message = $"Push: {pushResult.Message}, Pull: {pullResult.Message}";
                        result.ItemsProcessed = pushResult.ItemsProcessed + pullResult.ItemsProcessed;
                        result.ItemsCreated = pushResult.ItemsCreated + pullResult.ItemsCreated;
                        result.ItemsUpdated = pushResult.ItemsUpdated + pullResult.ItemsUpdated;
                        result.ItemsDeleted = pushResult.ItemsDeleted + pullResult.ItemsDeleted;
                        result.Errors = pushResult.Errors + pullResult.Errors;
                        result.ErrorMessages.AddRange(pushResult.ErrorMessages);
                        result.ErrorMessages.AddRange(pullResult.ErrorMessages);
                        break;
                }

                result.Duration = DateTime.UtcNow - startTime;

                var status = result.Success ? SyncStatus.Completed : SyncStatus.Failed;
                OnSyncStatusChanged(status, tableName, 100, 100, result.Message);
                AddSyncLogEntry("SyncTable", tableName, status, result.Message, result.Duration, result.ItemsProcessed);

                return result;
            }
            catch (OperationCanceledException)
            {
                var duration = DateTime.UtcNow - startTime;
                var message = $"Sync cancelled for {tableName}";

                OnSyncStatusChanged(SyncStatus.Cancelled, tableName, 0, 100, message);
                AddSyncLogEntry("SyncTable", tableName, SyncStatus.Cancelled, message, duration);

                return new SyncResult
                {
                    Success = false,
                    Message = message,
                    Duration = duration,
                    TableName = tableName,
                    Timestamp = startTime
                };
            }
            catch (Exception ex)
            {
                var duration = DateTime.UtcNow - startTime;
                var message = $"Sync failed for {tableName}: {ex.Message}";

                _logger.LogError(ex, "Error during sync of {TableName}", tableName);

                OnSyncStatusChanged(SyncStatus.Failed, tableName, 0, 100, message);
                AddSyncLogEntry("SyncTable", tableName, SyncStatus.Failed, message, duration);

                return new SyncResult
                {
                    Success = false,
                    Message = message,
                    Duration = duration,
                    TableName = tableName,
                    Timestamp = startTime,
                    ErrorMessages = { ex.Message }
                };
            }
        }

        public async Task<SyncResult> PushChangesAsync(string tableName, CancellationToken cancellationToken = default)
        {
            var startTime = DateTime.UtcNow;
            var result = new SyncResult { TableName = tableName, Timestamp = startTime };

            OnSyncStatusChanged(SyncStatus.InProgress, tableName, 0, 100, $"Pushing changes for {tableName}");
            AddSyncLogEntry("PushChanges", tableName, SyncStatus.InProgress, $"Pushing changes for {tableName}");

            try
            {
                // Implementation depends on your local data storage strategy
                // For example, getting pending changes from a local database or offline store

                // This is a simplified example that assumes you have a way to get pending changes
                var pendingChanges = await GetPendingChangesAsync(tableName);

                if (pendingChanges.Count == 0)
                {
                    result.Success = true;
                    result.Message = $"No changes to push for {tableName}";
                    result.Duration = DateTime.UtcNow - startTime;

                    OnSyncStatusChanged(SyncStatus.Completed, tableName, 100, 100, result.Message);
                    AddSyncLogEntry("PushChanges", tableName, SyncStatus.Completed, result.Message, result.Duration, 0);

                    return result;
                }

                // Process changes in batches
                int totalItems = pendingChanges.Count;
                int processedItems = 0;
                int batchSize = Math.Min(_options.MaxBatchSize, totalItems);

                for (int i = 0; i < totalItems; i += batchSize)
                {
                    // Check if cancelled
                    cancellationToken.ThrowIfCancellationRequested();

                    // Get batch
                    var batch = pendingChanges.Skip(i).Take(batchSize).ToList();

                    // Send batch to server
                    var batchResult = await PushBatchAsync(tableName, batch, cancellationToken);

                    // Update result
                    result.ItemsProcessed += batchResult.ItemsProcessed;
                    result.ItemsCreated += batchResult.ItemsCreated;
                    result.ItemsUpdated += batchResult.ItemsUpdated;
                    result.ItemsDeleted += batchResult.ItemsDeleted;
                    result.Errors += batchResult.Errors;
                    result.ErrorMessages.AddRange(batchResult.ErrorMessages);

                    // Update progress
                    processedItems += batch.Count;
                    int progress = (int)((double)processedItems / totalItems * 100);
                    OnSyncStatusChanged(SyncStatus.InProgress, tableName, progress, 100, $"Pushing changes for {tableName}: {processedItems}/{totalItems}");
                }

                result.Success = result.Errors == 0;
                result.Duration = DateTime.UtcNow - startTime;
                result.Message = result.Success
                    ? $"Successfully pushed {result.ItemsProcessed} changes for {tableName} in {result.Duration.TotalSeconds:F2}s"
                    : $"Pushed changes for {tableName} with {result.Errors} errors in {result.Duration.TotalSeconds:F2}s";

                var status = result.Success ? SyncStatus.Completed : SyncStatus.Failed;
                OnSyncStatusChanged(status, tableName, 100, 100, result.Message);
                AddSyncLogEntry("PushChanges", tableName, status, result.Message, result.Duration, result.ItemsProcessed);

                return result;
            }
            catch (OperationCanceledException)
            {
                var duration = DateTime.UtcNow - startTime;
                var message = $"Push cancelled for {tableName}";

                OnSyncStatusChanged(SyncStatus.Cancelled, tableName, 0, 100, message);
                AddSyncLogEntry("PushChanges", tableName, SyncStatus.Cancelled, message, duration);

                return new SyncResult
                {
                    Success = false,
                    Message = message,
                    Duration = duration,
                    TableName = tableName,
                    Timestamp = startTime
                };
            }
            catch (Exception ex)
            {
                var duration = DateTime.UtcNow - startTime;
                var message = $"Push failed for {tableName}: {ex.Message}";

                _logger.LogError(ex, "Error during push of {TableName}", tableName);

                OnSyncStatusChanged(SyncStatus.Failed, tableName, 0, 100, message);
                AddSyncLogEntry("PushChanges", tableName, SyncStatus.Failed, message, duration);

                return new SyncResult
                {
                    Success = false,
                    Message = message,
                    Duration = duration,
                    TableName = tableName,
                    Timestamp = startTime,
                    ErrorMessages = { ex.Message }
                };
            }
        }

        public async Task<SyncResult> PullChangesAsync(string tableName, CancellationToken cancellationToken = default)
        {
            var startTime = DateTime.UtcNow;
            var result = new SyncResult { TableName = tableName, Timestamp = startTime };

            OnSyncStatusChanged(SyncStatus.InProgress, tableName, 0, 100, $"Pulling changes for {tableName}");
            AddSyncLogEntry("PullChanges", tableName, SyncStatus.InProgress, $"Pulling changes for {tableName}");

            try
            {
                // Get last sync timestamp for this table
                DateTime? lastSyncTime = GetLastSyncTimeForTable(tableName);

                // Get changes from server
                var changes = await GetChangesFromServerAsync(tableName, lastSyncTime, cancellationToken);

                if (changes.Count == 0)
                {
                    result.Success = true;
                    result.Message = $"No changes to pull for {tableName}";
                    result.Duration = DateTime.UtcNow - startTime;

                    OnSyncStatusChanged(SyncStatus.Completed, tableName, 100, 100, result.Message);
                    AddSyncLogEntry("PullChanges", tableName, SyncStatus.Completed, result.Message, result.Duration, 0);

                    return result;
                }

                // Process changes in batches
                int totalItems = changes.Count;
                int processedItems = 0;
                int batchSize = Math.Min(_options.MaxBatchSize, totalItems);

                for (int i = 0; i < totalItems; i += batchSize)
                {
                    // Check if cancelled
                    cancellationToken.ThrowIfCancellationRequested();

                    // Get batch
                    var batch = changes.Skip(i).Take(batchSize).ToList();

                    // Apply changes locally
                    var batchResult = await ApplyChangesLocallyAsync(tableName, batch, cancellationToken);

                    // Update result
                    result.ItemsProcessed += batchResult.ItemsProcessed;
                    result.ItemsCreated += batchResult.ItemsCreated;
                    result.ItemsUpdated += batchResult.ItemsUpdated;
                    result.ItemsDeleted += batchResult.ItemsDeleted;
                    result.Errors += batchResult.Errors;
                    result.ErrorMessages.AddRange(batchResult.ErrorMessages);

                    // Update progress
                    processedItems += batch.Count;
                    int progress = (int)((double)processedItems / totalItems * 100);
                    OnSyncStatusChanged(SyncStatus.InProgress, tableName, progress, 100, $"Pulling changes for {tableName}: {processedItems}/{totalItems}");
                }

                result.Success = result.Errors == 0;
                result.Duration = DateTime.UtcNow - startTime;
                result.Message = result.Success
                    ? $"Successfully pulled {result.ItemsProcessed} changes for {tableName} in {result.Duration.TotalSeconds:F2}s"
                    : $"Pulled changes for {tableName} with {result.Errors} errors in {result.Duration.TotalSeconds:F2}s";

                var status = result.Success ? SyncStatus.Completed : SyncStatus.Failed;
                OnSyncStatusChanged(status, tableName, 100, 100, result.Message);
                AddSyncLogEntry("PullChanges", tableName, status, result.Message, result.Duration, result.ItemsProcessed);

                return result;
            }
            catch (OperationCanceledException)
            {
                var duration = DateTime.UtcNow - startTime;
                var message = $"Pull cancelled for {tableName}";

                OnSyncStatusChanged(SyncStatus.Cancelled, tableName, 0, 100, message);
                AddSyncLogEntry("PullChanges", tableName, SyncStatus.Cancelled, message, duration);

                return new SyncResult
                {
                    Success = false,
                    Message = message,
                    Duration = duration,
                    TableName = tableName,
                    Timestamp = startTime
                };
            }
            catch (Exception ex)
            {
                var duration = DateTime.UtcNow - startTime;
                var message = $"Pull failed for {tableName}: {ex.Message}";

                _logger.LogError(ex, "Error during pull of {TableName}", tableName);

                OnSyncStatusChanged(SyncStatus.Failed, tableName, 0, 100, message);
                AddSyncLogEntry("PullChanges", tableName, SyncStatus.Failed, message, duration);

                return new SyncResult
                {
                    Success = false,
                    Message = message,
                    Duration = duration,
                    TableName = tableName,
                    Timestamp = startTime,
                    ErrorMessages = { ex.Message }
                };
            }
        }

        public void CancelSync()
        {
            if (_syncInfo.IsSyncing && _cancellationTokenSource != null)
            {
                _logger.LogInformation("Cancelling sync");
                _cancellationTokenSource.Cancel();
            }
        }

        public void ClearSyncLog()
        {
            lock (_syncLock)
            {
                _syncLog.Clear();
            }
        }

        public List<SyncLogEntry> GetSyncLog(int maxEntries = 100)
        {
            lock (_syncLock)
            {
                return _syncLog
                    .OrderByDescending(e => e.Timestamp)
                    .Take(maxEntries)
                    .ToList();
            }
        }

        protected virtual void OnSyncStatusChanged(SyncStatus status, string? tableName, int progress, int totalItems, string? message)
        {
            SyncStatusChanged?.Invoke(this, new SyncStatusChangedEventArgs(status, tableName, progress, totalItems, message));
        }

        private void AddSyncLogEntry(string operation, string? tableName, SyncStatus status, string? message, TimeSpan? duration = null, int? itemsProcessed = null)
        {
            var entry = new SyncLogEntry
            {
                Timestamp = DateTime.UtcNow,
                Operation = operation,
                TableName = tableName,
                Status = status,
                Message = message,
                Duration = duration,
                ItemsProcessed = itemsProcessed
            };

            lock (_syncLock)
            {
                _syncLog.Add(entry);

                // Limit log size
                if (_syncLog.Count > 1000)
                {
                    _syncLog.RemoveRange(0, _syncLog.Count - 1000);
                }
            }
        }

        private async Task AutoSyncAsync()
        {
            if (_syncInfo.IsSyncing)
            {
                return;
            }

            try
            {
                _logger.LogInformation("Starting automatic sync");
                await SyncAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during automatic sync");
            }
        }

        private DateTime? GetLastSyncTimeForTable(string tableName)
        {
            lock (_syncLock)
            {
                if (_syncInfo.TableResults.TryGetValue(tableName, out var result))
                {
                    return result.Timestamp;
                }

                return null;
            }
        }

        #region Data Access Methods

        // These methods should be implemented according to your specific data storage strategy
        // The implementations below are placeholders

        private async Task<List<SyncItem>> GetPendingChangesAsync(string tableName)
        {
            // Implementation depends on your local data storage strategy
            // For example, getting items with sync status = "Pending" from a local database

            // Placeholder implementation
            await Task.Delay(100);
            return new List<SyncItem>();
        }

        private async Task<SyncResult> PushBatchAsync(string tableName, List<SyncItem> batch, CancellationToken cancellationToken)
        {
            // Implementation depends on your API design
            // For example, sending a batch of changes to the server

            // Placeholder implementation
            await Task.Delay(500, cancellationToken);

            return new SyncResult
            {
                Success = true,
                ItemsProcessed = batch.Count,
                ItemsCreated = batch.Count(i => i.SyncAction == SyncAction.Create),
                ItemsUpdated = batch.Count(i => i.SyncAction == SyncAction.Update),
                ItemsDeleted = batch.Count(i => i.SyncAction == SyncAction.Delete)
            };
        }

        private async Task<List<SyncItem>> GetChangesFromServerAsync(string tableName, DateTime? lastSyncTime, CancellationToken cancellationToken)
        {
            // Implementation depends on your API design
            // For example, getting changes from the server since lastSyncTime

            try
            {
                var queryParams = new Dictionary<string, string>();

                if (lastSyncTime.HasValue)
                {
                    queryParams.Add("lastSyncTime", lastSyncTime.Value.ToString("o"));
                }

                var endpoint = $"api/sync/{tableName}";
                var result = await _apiClient.GetAsync<List<SyncItem>>(endpoint, queryParams);

                return result ?? new List<SyncItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting changes from server for {TableName}", tableName);
                throw;
            }
        }

        private async Task<SyncResult> ApplyChangesLocallyAsync(string tableName, List<SyncItem> changes, CancellationToken cancellationToken)
        {
            // Implementation depends on your local data storage strategy
            // For example, applying changes to a local database

            // Placeholder implementation
            await Task.Delay(500, cancellationToken);

            return new SyncResult
            {
                Success = true,
                ItemsProcessed = changes.Count,
                ItemsCreated = changes.Count(i => i.SyncAction == SyncAction.Create),
                ItemsUpdated = changes.Count(i => i.SyncAction == SyncAction.Update),
                ItemsDeleted = changes.Count(i => i.SyncAction == SyncAction.Delete)
            };
        }

        #endregion
    }

    public class SyncItem
    {
        public string Id { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public SyncAction SyncAction { get; set; }
        public string? Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string? RowVersion { get; set; }

        public T? GetData<T>() where T : class
        {
            if (string.IsNullOrEmpty(Data))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(Data);
            }
            catch
            {
                return null;
            }
        }
    }

    public enum SyncAction
    {
        Create,
        Update,
        Delete
    }
}