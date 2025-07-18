using System.Net.Http.Headers;
using System.Text.Json;

namespace LexiFlow.API.Services
{
    public class BasicSyncService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalDatabase _localDb;
        private readonly ILogger<BasicSyncService> _logger;
        private readonly string _apiUrl;

        public BasicSyncService(HttpClient httpClient, LocalDatabase localDb,
            ILogger<BasicSyncService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _localDb = localDb;
            _logger = logger;
            _apiUrl = configuration["ApiUrl"];
        }

        public async Task<SyncResult> SyncAsync(string authToken)
        {
            var result = new SyncResult();

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken);

                // Step 1: Upload local changes
                result.UploadResult = await UploadLocalChangesAsync();

                // Step 2: Download server changes
                result.DownloadResult = await DownloadServerChangesAsync();

                // Step 3: Update sync metadata
                await UpdateSyncMetadataAsync();

                result.Success = true;
                result.SyncedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sync failed");
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private async Task<UploadResult> UploadLocalChangesAsync()
        {
            var result = new UploadResult();

            // Get pending items
            var pendingItems = await _localDb.GetPendingSyncItemsAsync();

            foreach (var item in pendingItems)
            {
                try
                {
                    var response = await UploadItemAsync(item);

                    if (response.IsSuccessStatusCode)
                    {
                        await _localDb.MarkSyncedAsync(item.Id);
                        result.SuccessCount++;
                    }
                    else
                    {
                        await _localDb.UpdateSyncErrorAsync(item.Id, response.ReasonPhrase);
                        result.FailedCount++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to sync item {item.Id}");
                    await _localDb.UpdateSyncErrorAsync(item.Id, ex.Message);
                    result.FailedCount++;
                }
            }

            return result;
        }

        private async Task<HttpResponseMessage> UploadItemAsync(PendingSyncItem item)
        {
            return item.Operation switch
            {
                "INSERT" => await _httpClient.PostAsJsonAsync($"{_apiUrl}/api/v1/vocabulary", item.Data),
                "UPDATE" => await _httpClient.PutAsJsonAsync($"{_apiUrl}/api/v1/vocabulary/{item.RecordId}", item.Data),
                "DELETE" => await _httpClient.DeleteAsync($"{_apiUrl}/api/v1/vocabulary/{item.RecordId}"),
                _ => throw new InvalidOperationException($"Unknown operation: {item.Operation}")
            };
        }

        private async Task<DownloadResult> DownloadServerChangesAsync()
        {
            var result = new DownloadResult();

            // Get last sync timestamp
            var lastSync = await _localDb.GetLastSyncTimestampAsync("Vocabulary");

            // Download changes
            var response = await _httpClient.GetAsync(
                $"{_apiUrl}/api/v1/vocabulary?lastSync={lastSync:yyyy-MM-ddTHH:mm:ss}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<VocabularyListResponse>(content);

                foreach (var vocab in data.Data)
                {
                    await _localDb.SaveServerVocabularyAsync(vocab);
                    result.RecordsDownloaded++;
                }
            }

            return result;
        }

        private async Task UpdateSyncMetadataAsync()
        {
            await _localDb.UpdateSyncMetadataAsync("Vocabulary", DateTime.UtcNow);
        }
    }

    // Models
    public class SyncResult
    {
        public bool Success { get; set; }
        public DateTime SyncedAt { get; set; }
        public UploadResult UploadResult { get; set; }
        public DownloadResult DownloadResult { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class UploadResult
    {
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
    }

    public class DownloadResult
    {
        public int RecordsDownloaded { get; set; }
    }
}
