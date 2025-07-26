using LexiFlow.API.Models.DTOs;
using LexiFlow.API.Models.Responses;
using LexiFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly ISyncService _syncService;
        private readonly ILogger<SyncController> _logger;

        public SyncController(
            ISyncService syncService,
            ILogger<SyncController> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        [HttpGet("{entityType}")]
        public async Task<ActionResult<List<SyncItemDto>>> GetChanges(
            string entityType,
            [FromQuery] DateTime? lastSyncTime = null)
        {
            try
            {
                var userId = GetUserId();
                var changes = await _syncService.GetChangesAsync(entityType, lastSyncTime, userId);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"Changes retrieved successfully for {entityType}",
                    Data = new
                    {
                        Changes = changes,
                        LastSyncTime = DateTime.UtcNow
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting changes for {EntityType}", entityType);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"An error occurred while retrieving changes for {entityType}"
                });
            }
        }

        [HttpPost("{entityType}")]
        public async Task<IActionResult> ApplyChanges(string entityType, [FromBody] SyncBatchDto batch)
        {
            try
            {
                if (batch.EntityType.ToLowerInvariant() != entityType.ToLowerInvariant())
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = $"Entity type mismatch: URL says '{entityType}' but body says '{batch.EntityType}'"
                    });
                }

                var userId = GetUserId();
                var (created, updated, deleted, errors) = await _syncService.ApplyChangesAsync(batch, userId);

                if (errors.Count > 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Some changes could not be applied",
                        Data = new
                        {
                            Created = created,
                            Updated = updated,
                            Deleted = deleted,
                            Errors = errors,
                            LastSyncTime = DateTime.UtcNow
                        }
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = $"Changes applied successfully for {entityType}",
                    Data = new
                    {
                        Created = created,
                        Updated = updated,
                        Deleted = deleted,
                        LastSyncTime = DateTime.UtcNow
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying changes for {EntityType}", entityType);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"An error occurred while applying changes for {entityType}"
                });
            }
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("Invalid user ID in token");
        }
    }
}