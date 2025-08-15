using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LexiFlow.API.DTOs.Common;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.Controllers
{
    /// <summary>
    /// Controller ??ng b? d? li?u gi?a client và server
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SyncController : ControllerBase
    {
        private readonly ILogger<SyncController> _logger;

        public SyncController(ILogger<SyncController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// ??ng b? toàn b? d? li?u
        /// </summary>
        [HttpPost("full")]
        public async Task<ActionResult<FullSyncResponseDto>> FullSync([FromBody] FullSyncRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var response = new FullSyncResponseDto
                {
                    SyncId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    SyncTime = DateTime.UtcNow,
                    Status = "Success",
                    VocabularySync = new SyncResultDto
                    {
                        UpdatedItems = 15,
                        DeletedItems = 2,
                        ConflictCount = 0,
                        LastSyncTime = request.LastSyncTime
                    },
                    KanjiSync = new SyncResultDto
                    {
                        UpdatedItems = 8,
                        DeletedItems = 1,
                        ConflictCount = 0,
                        LastSyncTime = request.LastSyncTime
                    },
                    GrammarSync = new SyncResultDto
                    {
                        UpdatedItems = 5,
                        DeletedItems = 0,
                        ConflictCount = 0,
                        LastSyncTime = request.LastSyncTime
                    },
                    ProgressSync = new SyncResultDto
                    {
                        UpdatedItems = 25,
                        DeletedItems = 0,
                        ConflictCount = 1,
                        LastSyncTime = request.LastSyncTime
                    },
                    StudyPlanSync = new SyncResultDto
                    {
                        UpdatedItems = 3,
                        DeletedItems = 0,
                        ConflictCount = 0,
                        LastSyncTime = request.LastSyncTime
                    },
                    Conflicts = new List<SyncConflictDto>
                    {
                        new SyncConflictDto
                        {
                            ConflictId = 1,
                            EntityType = "LearningProgress",
                            EntityId = 123,
                            ConflictType = "ModificationConflict",
                            ClientVersion = request.LastSyncTime ?? DateTime.UtcNow,
                            ServerVersion = DateTime.UtcNow.AddMinutes(-10),
                            ClientData = "{ \"score\": 85 }",
                            ServerData = "{ \"score\": 90 }",
                            ResolutionStrategy = "TakeServer"
                        }
                    },
                    TotalProcessedItems = 56,
                    ProcessingTimeMs = 1250
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing full sync");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// ??ng b? d? li?u incremental
        /// </summary>
        [HttpPost("incremental")]
        public async Task<ActionResult<IncrementalSyncResponseDto>> IncrementalSync([FromBody] IncrementalSyncRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var response = new IncrementalSyncResponseDto
                {
                    SyncId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    SyncTime = DateTime.UtcNow,
                    LastSyncTime = request.LastSyncTime,
                    HasMoreData = false,
                    Changes = new List<EntityChangeDto>
                    {
                        new EntityChangeDto
                        {
                            EntityType = "Vocabulary",
                            EntityId = 456,
                            ChangeType = "Update",
                            Timestamp = DateTime.UtcNow.AddMinutes(-30),
                            Data = "{ \"term\": \"?????\", \"meaning\": \"Hello\" }"
                        },
                        new EntityChangeDto
                        {
                            EntityType = "LearningProgress",
                            EntityId = 789,
                            ChangeType = "Create",
                            Timestamp = DateTime.UtcNow.AddMinutes(-15),
                            Data = "{ \"vocabularyId\": 456, \"score\": 95 }"
                        }
                    },
                    DeletedItems = new List<DeletedItemDto>
                    {
                        new DeletedItemDto
                        {
                            EntityType = "StudyTask",
                            EntityId = 321,
                            DeletedAt = DateTime.UtcNow.AddHours(-2),
                            Reason = "UserRequest"
                        }
                    },
                    Conflicts = new List<SyncConflictDto>(),
                    NextSyncToken = Guid.NewGuid().ToString()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing incremental sync");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Upload d? li?u t? client lên server
        /// </summary>
        [HttpPost("upload")]
        public async Task<ActionResult<UploadResponseDto>> UploadData([FromBody] UploadRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var response = new UploadResponseDto
                {
                    UploadId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    UploadTime = DateTime.UtcNow,
                    ProcessedItems = request.Changes.Count,
                    SuccessfulItems = request.Changes.Count - 1,
                    FailedItems = 1,
                    Results = new List<UploadResultDto>
                    {
                        new UploadResultDto
                        {
                            ClientId = "client-001",
                            EntityType = "StudySession",
                            Status = "Success",
                            ServerId = 1001,
                            Message = "Created successfully"
                        },
                        new UploadResultDto
                        {
                            ClientId = "client-002",
                            EntityType = "LearningProgress",
                            Status = "Failed",
                            Message = "Validation error: Invalid score value"
                        }
                    },
                    Conflicts = new List<SyncConflictDto>(),
                    NextSyncTime = DateTime.UtcNow
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading data");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gi?i quy?t xung ??t ??ng b?
        /// </summary>
        [HttpPost("resolve-conflicts")]
        public async Task<ActionResult<ConflictResolutionResponseDto>> ResolveConflicts([FromBody] ConflictResolutionRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                var response = new ConflictResolutionResponseDto
                {
                    ResolutionId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ResolvedAt = DateTime.UtcNow,
                    ResolvedConflicts = request.Resolutions.Count,
                    Results = request.Resolutions.Select(r => new ConflictResolutionResultDto
                    {
                        ConflictId = r.ConflictId,
                        Status = "Resolved",
                        AppliedStrategy = r.Strategy,
                        Message = "Conflict resolved successfully"
                    }).ToList()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resolving conflicts");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// L?y tr?ng thái ??ng b?
        /// </summary>
        [HttpGet("status")]
        public async Task<ActionResult<SyncStatusDto>> GetSyncStatus([FromQuery] string? deviceId = null)
        {
            try
            {
                var userId = GetCurrentUserId();

                var status = new SyncStatusDto
                {
                    UserId = userId,
                    DeviceId = deviceId ?? "default",
                    LastSyncTime = DateTime.UtcNow.AddHours(-2),
                    IsOnline = true,
                    SyncEnabled = true,
                    PendingChanges = 5,
                    PendingConflicts = 0,
                    LastSyncDuration = 1250,
                    SyncHistory = new List<SyncHistoryDto>
                    {
                        new SyncHistoryDto
                        {
                            SyncId = "sync-001",
                            SyncTime = DateTime.UtcNow.AddHours(-2),
                            SyncType = "Incremental",
                            Status = "Success",
                            ItemsProcessed = 15,
                            Duration = 850
                        },
                        new SyncHistoryDto
                        {
                            SyncId = "sync-002",
                            SyncTime = DateTime.UtcNow.AddHours(-6),
                            SyncType = "Full",
                            Status = "Success",
                            ItemsProcessed = 120,
                            Duration = 3200
                        }
                    },
                    Statistics = new SyncStatisticsDto
                    {
                        TotalSyncs = 45,
                        SuccessfulSyncs = 43,
                        FailedSyncs = 2,
                        TotalItemsSynced = 2500,
                        AverageSyncTime = 1150,
                        LastFailureReason = "Network timeout"
                    }
                };

                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting sync status");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// C?u hình ??ng b?
        /// </summary>
        [HttpPut("settings")]
        public async Task<ActionResult> UpdateSyncSettings([FromBody] SyncSettingsDto settings)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Update sync settings in database

                return Ok(new { Message = "Sync settings updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating sync settings");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Reset d? li?u ??ng b?
        /// </summary>
        [HttpPost("reset")]
        public async Task<ActionResult> ResetSync([FromBody] ResetSyncRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();

                // TODO: Reset sync data based on request

                return Ok(new
                {
                    Message = "Sync reset completed successfully",
                    ResetTime = DateTime.UtcNow,
                    ResetType = request.ResetType
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting sync");
                return StatusCode(500, "Internal server error");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 1;
        }
    }

    #region DTOs

    public class FullSyncRequestDto
    {
        public DateTime? LastSyncTime { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public string AppVersion { get; set; } = string.Empty;
        public bool ForceFullSync { get; set; }
        public List<string> EntityTypes { get; set; } = new List<string>();
    }

    public class FullSyncResponseDto
    {
        public string SyncId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime SyncTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public SyncResultDto VocabularySync { get; set; } = new SyncResultDto();
        public SyncResultDto KanjiSync { get; set; } = new SyncResultDto();
        public SyncResultDto GrammarSync { get; set; } = new SyncResultDto();
        public SyncResultDto ProgressSync { get; set; } = new SyncResultDto();
        public SyncResultDto StudyPlanSync { get; set; } = new SyncResultDto();
        public List<SyncConflictDto> Conflicts { get; set; } = new List<SyncConflictDto>();
        public int TotalProcessedItems { get; set; }
        public int ProcessingTimeMs { get; set; }
    }

    public class SyncResultDto
    {
        public int UpdatedItems { get; set; }
        public int DeletedItems { get; set; }
        public int ConflictCount { get; set; }
        public DateTime? LastSyncTime { get; set; }
    }

    public class IncrementalSyncRequestDto
    {
        [Required]
        public DateTime LastSyncTime { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public string? SyncToken { get; set; }
        public int? MaxItems { get; set; }
        public List<string> EntityTypes { get; set; } = new List<string>();
    }

    public class IncrementalSyncResponseDto
    {
        public string SyncId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime SyncTime { get; set; }
        public DateTime LastSyncTime { get; set; }
        public bool HasMoreData { get; set; }
        public List<EntityChangeDto> Changes { get; set; } = new List<EntityChangeDto>();
        public List<DeletedItemDto> DeletedItems { get; set; } = new List<DeletedItemDto>();
        public List<SyncConflictDto> Conflicts { get; set; } = new List<SyncConflictDto>();
        public string NextSyncToken { get; set; } = string.Empty;
    }

    public class EntityChangeDto
    {
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string ChangeType { get; set; } = string.Empty; // Create, Update, Delete
        public DateTime Timestamp { get; set; }
        public string Data { get; set; } = string.Empty;
        public string? Version { get; set; }
    }

    public class DeletedItemDto
    {
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public DateTime DeletedAt { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class SyncConflictDto
    {
        public int ConflictId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string ConflictType { get; set; } = string.Empty;
        public DateTime ClientVersion { get; set; }
        public DateTime ServerVersion { get; set; }
        public string ClientData { get; set; } = string.Empty;
        public string ServerData { get; set; } = string.Empty;
        public string ResolutionStrategy { get; set; } = string.Empty;
    }

    public class UploadRequestDto
    {
        [Required]
        public string DeviceId { get; set; } = string.Empty;
        public List<ClientChangeDto> Changes { get; set; } = new List<ClientChangeDto>();
        public DateTime UploadTime { get; set; }
    }

    public class ClientChangeDto
    {
        public string ClientId { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string ChangeType { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class UploadResponseDto
    {
        public string UploadId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime UploadTime { get; set; }
        public int ProcessedItems { get; set; }
        public int SuccessfulItems { get; set; }
        public int FailedItems { get; set; }
        public List<UploadResultDto> Results { get; set; } = new List<UploadResultDto>();
        public List<SyncConflictDto> Conflicts { get; set; } = new List<SyncConflictDto>();
        public DateTime NextSyncTime { get; set; }
    }

    public class UploadResultDto
    {
        public string ClientId { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? ServerId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ConflictResolutionRequestDto
    {
        public List<ConflictResolutionDto> Resolutions { get; set; } = new List<ConflictResolutionDto>();
    }

    public class ConflictResolutionDto
    {
        [Required]
        public int ConflictId { get; set; }
        [Required]
        public string Strategy { get; set; } = string.Empty; // TakeClient, TakeServer, Merge
        public string? MergedData { get; set; }
    }

    public class ConflictResolutionResponseDto
    {
        public string ResolutionId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime ResolvedAt { get; set; }
        public int ResolvedConflicts { get; set; }
        public List<ConflictResolutionResultDto> Results { get; set; } = new List<ConflictResolutionResultDto>();
    }

    public class ConflictResolutionResultDto
    {
        public int ConflictId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AppliedStrategy { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class SyncStatusDto
    {
        public int UserId { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public DateTime? LastSyncTime { get; set; }
        public bool IsOnline { get; set; }
        public bool SyncEnabled { get; set; }
        public int PendingChanges { get; set; }
        public int PendingConflicts { get; set; }
        public int LastSyncDuration { get; set; }
        public List<SyncHistoryDto> SyncHistory { get; set; } = new List<SyncHistoryDto>();
        public SyncStatisticsDto Statistics { get; set; } = new SyncStatisticsDto();
    }

    public class SyncHistoryDto
    {
        public string SyncId { get; set; } = string.Empty;
        public DateTime SyncTime { get; set; }
        public string SyncType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ItemsProcessed { get; set; }
        public int Duration { get; set; }
    }

    public class SyncStatisticsDto
    {
        public int TotalSyncs { get; set; }
        public int SuccessfulSyncs { get; set; }
        public int FailedSyncs { get; set; }
        public int TotalItemsSynced { get; set; }
        public int AverageSyncTime { get; set; }
        public string? LastFailureReason { get; set; }
    }

    public class SyncSettingsDto
    {
        public bool AutoSync { get; set; }
        public int SyncInterval { get; set; } // minutes
        public bool SyncOnWifiOnly { get; set; }
        public bool SyncInBackground { get; set; }
        public List<string> SyncEntityTypes { get; set; } = new List<string>();
        public string ConflictResolutionStrategy { get; set; } = string.Empty;
    }

    public class ResetSyncRequestDto
    {
        [Required]
        public string ResetType { get; set; } = string.Empty; // Full, Partial, ConflictsOnly
        public List<string>? EntityTypes { get; set; }
        public bool DeleteLocalData { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    #endregion
}