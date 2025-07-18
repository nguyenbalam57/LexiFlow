using LexiFlow.API.Models.DTOs;
using LexiFlow.API.Models.Responses;
using LexiFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _syncService;
        private readonly ILogger<SyncController> _logger;

        public SyncController(ISyncService syncService, ILogger<SyncController> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SyncData([FromBody] SyncRequestDto request)
        {
            var userId = GetUserId();

            try
            {
                // Convert DTOs to domain models
                var pendingItems = request.PendingItems?.Select(p => new PendingSyncItem
                {
                    TableName = p.TableName,
                    RecordId = p.RecordId,
                    Operation = p.Operation,
                    Data = p.Data
                }).ToList() ?? new List<PendingSyncItem>();

                var result = await _syncService.SyncVocabularyAsync(userId, request.LastSyncTimestamp, pendingItems);

                return Ok(new ApiResponse
                {
                    Success = result.Success,
                    Message = result.Success ? "Sync completed successfully" : "Sync completed with errors",
                    Data = new
                    {
                        syncedAt = result.SyncedAt,
                        itemsUploaded = result.ItemsUploaded,
                        itemsDownloaded = result.ItemsDownloaded,
                        errorMessage = result.ErrorMessage
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sync operation");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred during synchronization",
                    Data = new { errorMessage = ex.Message }
                });
            }
        }

        [HttpGet("last")]
        public async Task<IActionResult> GetLastSyncTimestamp([FromQuery] string tableName)
        {
            var userId = GetUserId();
            var lastSync = await _syncService.GetLastSyncTimestampAsync(userId, tableName);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = new { tableName, lastSync }
            });
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        }
    }
}
