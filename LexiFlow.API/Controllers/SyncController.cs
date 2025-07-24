// LexiFlow.API/Controllers/SyncController.cs (Continuing from the existing file)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LexiFlow.Core.Interfaces;
using LexiFlow.Core.Models;
using LexiFlow.Core.Models.Requests;
using LexiFlow.Core.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LexiFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SyncController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SyncController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public SyncController(
            IUnitOfWork unitOfWork,
            ILogger<SyncController> logger,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        [HttpPost]
        public async Task<ActionResult<SyncResponse>> SyncData([FromBody] SyncRequest request)
        {
            if (request == null)
            {
                return BadRequest(new SyncResponse
                {
                    Success = false,
                    Message = "Invalid request data"
                });
            }

            try
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                if (currentUserId <= 0)
                {
                    return Unauthorized(new SyncResponse
                    {
                        Success = false,
                        Message = "User not authenticated"
                    });
                }

                _logger.LogInformation("Starting sync for user {UserId}, device {DeviceId}", currentUserId, request.DeviceId);

                // Begin transaction
                await _unitOfWork.BeginTransactionAsync();

                // Process client changes
                var result = new SyncResult
                {
                    SyncedAt = DateTime.UtcNow,
                    Success = true
                };

                // Apply client changes
                try
                {
                    // Process vocabulary changes
                    var vocabResults = await ApplyVocabularyChangesAsync(request.ModifiedItems, request.DeletedItemIds, currentUserId);
                    result.TotalUpdated = vocabResults.created + vocabResults.updated;
                    result.TotalDeleted = vocabResults.deleted;

                    if (vocabResults.errors > 0)
                    {
                        _logger.LogWarning("Encountered {ErrorCount} errors while processing vocabulary changes", vocabResults.errors);
                    }

                    // Get server changes since last sync
                    if (request.LastSyncTime.HasValue)
                    {
                        var updatedItems = await GetUpdatedVocabularyItemsAsync(request.LastSyncTime.Value, currentUserId);
                        result.UpdatedItems = updatedItems;
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    result.Message = $"Synchronized successfully. Updated: {result.TotalUpdated}, Deleted: {result.TotalDeleted}, Received: {result.UpdatedItems.Count}";
                    _logger.LogInformation("Sync completed successfully for user {UserId}", currentUserId);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    _logger.LogError(ex, "Error during sync operation for user {UserId}", currentUserId);
                    result.Success = false;
                    result.Message = "Error during synchronization: " + ex.Message;
                }

                return Ok(new SyncResponse
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception during sync operation");
                return StatusCode(500, new SyncResponse
                {
                    Success = false,
                    Message = "An unexpected error occurred during synchronization"
                });
            }
        }

        #region Helper Methods

        private async Task<(int created, int updated, int deleted, int errors)> ApplyVocabularyChangesAsync(
            List<Vocabulary> modifiedItems,
            List<int> deletedItemIds,
            int currentUserId)
        {
            int created = 0, updated = 0, deleted = 0, errors = 0;

            // Process modified items
            foreach (var item in modifiedItems)
            {
                try
                {
                    // Check if item exists
                    var existingItem = await _unitOfWork.VocabularyItems.GetByIdAsync(item.Id);

                    if (existingItem == null)
                    {
                        // Create new item
                        item.CreatedByUserId = currentUserId;
                        item.CreatedAt = DateTime.UtcNow;
                        item.UpdatedByUserId = currentUserId;
                        item.UpdatedAt = DateTime.UtcNow;

                        await _unitOfWork.VocabularyItems.AddAsync(item, currentUserId);
                        created++;
                    }
                    else
                    {
                        // Update existing item
                        // Preserve creation info
                        item.CreatedAt = existingItem.CreatedAt;
                        item.CreatedByUserId = existingItem.CreatedByUserId;

                        // Update modification info
                        item.UpdatedByUserId = currentUserId;
                        item.UpdatedAt = DateTime.UtcNow;

                        await _unitOfWork.VocabularyItems.UpdateAsync(item, currentUserId);
                        updated++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing vocabulary item {Id}", item.Id);
                    errors++;
                }
            }

            // Process deleted items
            foreach (var itemId in deletedItemIds)
            {
                try
                {
                    await _unitOfWork.VocabularyItems.DeleteAsync(itemId, currentUserId);
                    deleted++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting vocabulary item {Id}", itemId);
                    errors++;
                }
            }

            return (created, updated, deleted, errors);
        }

        private async Task<List<Vocabulary>> GetUpdatedVocabularyItemsAsync(DateTime lastSyncTime, int userId)
        {
            // Get items updated since last sync
            var updatedItems = await _unitOfWork.VocabularyItems.GetAsync(
                v => v.UpdatedAt > lastSyncTime &&
                     (v.IsPublic || v.CreatedByUserId == userId || v.UpdatedByUserId == userId));

            return updatedItems.ToList();
        }

        private async Task<(int created, int updated, int deleted, int errors)> ApplyCategoryChangesAsync(List<SyncItemDto> changes, int currentUserId)
        {
            // Implementation for category changes
            int created = 0, updated = 0, deleted = 0, errors = 0;

            foreach (var change in changes)
            {
                try
                {
                    switch (change.ChangeType)
                    {
                        case ChangeType.Create:
                        case ChangeType.Update:
                            var category = System.Text.Json.JsonSerializer.Deserialize<Category>(change.Data);
                            var existingCategory = await _unitOfWork.Categories.GetByIdAsync(category.Id);

                            if (existingCategory == null)
                            {
                                category.CreatedByUserId = currentUserId;
                                category.CreatedAt = DateTime.UtcNow;
                                category.UpdatedByUserId = currentUserId;
                                category.UpdatedAt = DateTime.UtcNow;

                                await _unitOfWork.Categories.AddAsync(category, currentUserId);
                                created++;
                            }
                            else
                            {
                                category.CreatedAt = existingCategory.CreatedAt;
                                category.CreatedByUserId = existingCategory.CreatedByUserId;
                                category.UpdatedByUserId = currentUserId;
                                category.UpdatedAt = DateTime.UtcNow;

                                await _unitOfWork.Categories.UpdateAsync(category, currentUserId);
                                updated++;
                            }
                            break;

                        case ChangeType.Delete:
                            await _unitOfWork.Categories.DeleteAsync(int.Parse(change.Id), currentUserId);
                            deleted++;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error applying category change {Id}", change.Id);
                    errors++;
                }
            }

            return (created, updated, deleted, errors);
        }

        private async Task<List<Category>> GetUpdatedCategoriesAsync(DateTime lastSyncTime, int userId)
        {
            // Get categories updated since last sync
            var updatedCategories = await _unitOfWork.Categories.GetAsync(
                c => c.UpdatedAt > lastSyncTime &&
                     (c.IsPublic || c.CreatedByUserId == userId || c.UpdatedByUserId == userId));

            return updatedCategories.ToList();
        }

        #endregion
    }
}