using LexiFlow.API.Models;
using System.Text.Json;

namespace LexiFlow.API.Services
{
    public class SyncService : ISyncService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SyncService> _logger;

        public SyncService(ApplicationDbContext context, ILogger<SyncService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SyncResult> SyncVocabularyAsync(int userId, DateTime? lastSync, List<PendingSyncItem> pendingItems)
        {
            var result = new SyncResult
            {
                Success = true,
                SyncedAt = DateTime.UtcNow
            };

            try
            {
                // Process pending uploads from client
                if (pendingItems != null && pendingItems.Count > 0)
                {
                    foreach (var item in pendingItems)
                    {
                        try
                        {
                            await ProcessSyncItemAsync(userId, item);
                            result.ItemsUploaded++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing sync item {ItemId} for user {UserId}", item.Id, userId);
                        }
                    }
                }

                // Download changes from server
                var changedItems = await GetChangedItemsAsync(userId, lastSync);
                result.ItemsDownloaded = changedItems.Count;

                // Update last sync timestamp
                await UpdateLastSyncTimestampAsync(userId, "Vocabulary", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                _logger.LogError(ex, "Sync operation failed for user {UserId}", userId);
            }

            return result;
        }

        public async Task<DateTime> GetLastSyncTimestampAsync(int userId, string tableName)
        {
            var syncMetadata = await _context.SyncMetadata
                .Where(sm => sm.UserID == userId && sm.TableName == tableName)
                .FirstOrDefaultAsync();

            return syncMetadata?.LastSyncTimestamp ?? DateTime.MinValue;
        }

        private async Task ProcessSyncItemAsync(int userId, PendingSyncItem item)
        {
            if (item.TableName.ToLower() != "vocabulary")
            {
                _logger.LogWarning("Unsupported table name in sync item: {TableName}", item.TableName);
                return;
            }

            switch (item.Operation.ToUpper())
            {
                case "INSERT":
                    await ProcessInsertAsync(userId, item);
                    break;

                case "UPDATE":
                    await ProcessUpdateAsync(userId, item);
                    break;

                case "DELETE":
                    await ProcessDeleteAsync(userId, item);
                    break;

                default:
                    _logger.LogWarning("Unknown operation in sync item: {Operation}", item.Operation);
                    break;
            }
        }

        private async Task ProcessInsertAsync(int userId, PendingSyncItem item)
        {
            var vocabulary = JsonSerializer.Deserialize<Vocabulary>(item.Data);

            if (vocabulary == null)
            {
                _logger.LogWarning("Failed to deserialize vocabulary data for INSERT operation");
                return;
            }

            // Ensure user ID is set correctly
            vocabulary.CreatedByUserID = userId;
            vocabulary.CreatedAt = DateTime.UtcNow;
            vocabulary.VocabularyID = 0; // Ensure auto-increment works

            _context.Vocabulary.Add(vocabulary);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Inserted vocabulary item {Id} from sync for user {UserId}",
                vocabulary.VocabularyID, userId);
        }

        private async Task ProcessUpdateAsync(int userId, PendingSyncItem item)
        {
            if (!item.RecordId.HasValue)
            {
                _logger.LogWarning("Missing RecordId for UPDATE operation");
                return;
            }

            var existingVocabulary = await _context.Vocabulary
                .Where(v => v.VocabularyID == item.RecordId.Value)
                .FirstOrDefaultAsync();

            if (existingVocabulary == null)
            {
                _logger.LogWarning("Vocabulary item {Id} not found for UPDATE operation", item.RecordId.Value);
                return;
            }

            // Verify ownership
            if (existingVocabulary.CreatedByUserID != userId)
            {
                _logger.LogWarning("User {UserId} attempted to update vocabulary item {Id} they don't own",
                    userId, item.RecordId.Value);
                return;
            }

            var updatedVocabulary = JsonSerializer.Deserialize<Vocabulary>(item.Data);

            if (updatedVocabulary == null)
            {
                _logger.LogWarning("Failed to deserialize vocabulary data for UPDATE operation");
                return;
            }

            // Update fields
            existingVocabulary.Japanese = updatedVocabulary.Japanese;
            existingVocabulary.Kana = updatedVocabulary.Kana;
            existingVocabulary.Romaji = updatedVocabulary.Romaji;
            existingVocabulary.Vietnamese = updatedVocabulary.Vietnamese;
            existingVocabulary.English = updatedVocabulary.English;
            existingVocabulary.Example = updatedVocabulary.Example;
            existingVocabulary.Notes = updatedVocabulary.Notes;
            existingVocabulary.Level = updatedVocabulary.Level;
            existingVocabulary.UpdatedByUserID = userId;
            existingVocabulary.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated vocabulary item {Id} from sync for user {UserId}",
                existingVocabulary.VocabularyID, userId);
        }

        private async Task ProcessDeleteAsync(int userId, PendingSyncItem item)
        {
            if (!item.RecordId.HasValue)
            {
                _logger.LogWarning("Missing RecordId for DELETE operation");
                return;
            }

            var existingVocabulary = await _context.Vocabulary
                .Where(v => v.VocabularyID == item.RecordId.Value)
                .FirstOrDefaultAsync();

            if (existingVocabulary == null)
            {
                _logger.LogWarning("Vocabulary item {Id} not found for DELETE operation", item.RecordId.Value);
                return;
            }

            // Verify ownership
            if (existingVocabulary.CreatedByUserID != userId)
            {
                _logger.LogWarning("User {UserId} attempted to delete vocabulary item {Id} they don't own",
                    userId, item.RecordId.Value);
                return;
            }

            _context.Vocabulary.Remove(existingVocabulary);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted vocabulary item {Id} from sync for user {UserId}",
                existingVocabulary.VocabularyID, userId);
        }

        private async Task<List<Vocabulary>> GetChangedItemsAsync(int userId, DateTime? lastSync)
        {
            if (!lastSync.HasValue)
            {
                // If no last sync timestamp, return all vocabulary items for the user
                return await _context.Vocabulary
                    .Where(v => v.CreatedByUserID == userId)
                    .ToListAsync();
            }

            // Return only items that have been created or updated since the last sync
            return await _context.Vocabulary
                .Where(v => v.CreatedByUserID == userId &&
                           (v.CreatedAt > lastSync.Value ||
                            (v.UpdatedAt.HasValue && v.UpdatedAt.Value > lastSync.Value)))
                .ToListAsync();
        }

        private async Task UpdateLastSyncTimestampAsync(int userId, string tableName, DateTime timestamp)
        {
            var syncMetadata = await _context.SyncMetadata
                .Where(sm => sm.UserID == userId && sm.TableName == tableName)
                .FirstOrDefaultAsync();

            if (syncMetadata == null)
            {
                // Create new record
                syncMetadata = new SyncMetadata
                {
                    UserID = userId,
                    TableName = tableName,
                    LastSyncTimestamp = timestamp,
                    LastSyncVersion = 1
                };
                _context.SyncMetadata.Add(syncMetadata);
            }
            else
            {
                // Update existing record
                syncMetadata.LastSyncTimestamp = timestamp;
                syncMetadata.LastSyncVersion++;
            }

            await _context.SaveChangesAsync();
        }
    }
}
