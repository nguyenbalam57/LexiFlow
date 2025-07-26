using LexiFlow.API.Models.DTOs;

namespace LexiFlow.API.Services
{
    public interface ISyncService
    {
        Task<IEnumerable<SyncItemDto>> GetChangesAsync(string entityType, DateTime? lastSyncTime, int userId);
        Task<(int Created, int Updated, int Deleted, List<string> Errors)> ApplyChangesAsync(SyncBatchDto batch, int userId);
    }
}