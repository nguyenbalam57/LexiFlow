using LexiFlow.API.Data;
using LexiFlow.API.Models.DTOs;
using LexiFlow.Core.Entities;
using LexiFlow.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LexiFlow.API.Services
{
    public class SyncService : ISyncService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SyncService> _logger;

        public SyncService(ApplicationDbContext dbContext, ILogger<SyncService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<SyncItemDto>> GetChangesAsync(string entityType, DateTime? lastSyncTime, int userId)
        {
            var changes = new List<SyncItemDto>();

            try
            {
                switch (entityType.ToLowerInvariant())
                {
                    case "vocabularyitems":
                        changes = await GetVocabularyChangesAsync(lastSyncTime, userId);
                        break;

                    case "categories":
                        changes = await GetCategoryChangesAsync(lastSyncTime, userId);
                        break;

                    // Add other entity types as needed
                    default:
                        _logger.LogWarning("Unsupported entity type for sync: {EntityType}", entityType);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting changes for {EntityType}", entityType);
            }

            return changes;
        }

        private async Task<List<SyncItemDto>> GetVocabularyChangesAsync(DateTime? lastSyncTime, int userId)
        {
            var changes = new List<SyncItemDto>();

            // Get items that have been created or updated since the last sync
            var query = _dbContext.VocabularyItems
                .Include(v => v.Definitions)
                .Include(v => v.Examples)
                .Include(v => v.Translations)
                .AsQueryable();

            if (lastSyncTime.HasValue)
            {
                query = query.Where(v =>
                    v.CreatedAt >= lastSyncTime ||
                    (v.ModifiedAt.HasValue && v.ModifiedAt >= lastSyncTime) ||
                    (v.DeletedAt.HasValue && v.DeletedAt >= lastSyncTime)
                );
            }

            var items = await query.ToListAsync();

            foreach (var item in items)
            {
                var action = item.IsDeleted ? "Delete" : (item.CreatedAt >= (lastSyncTime ?? DateTime.MinValue) ? "Create" : "Update");

                changes.Add(new SyncItemDto
                {
                    Id = item.Id,
                    EntityType = "VocabularyItem",
                    Action = action,
                    Data = action != "Delete" ? item : null,
                    Timestamp = item.ModifiedAt ?? item.CreatedAt,
                    RowVersionString = Convert.ToBase64String(item.RowVersion)
                });
            }

            return changes;
        }

        private async Task<List<SyncItemDto>> GetCategoryChangesAsync(DateTime? lastSyncTime, int userId)
        {
            var changes = new List<SyncItemDto>();

            // Get categories that have been created or updated since the last sync
            var query = _dbContext.Categories.AsQueryable();

            if (lastSyncTime.HasValue)
            {
                query = query.Where(c =>
                    c.CreatedAt >= lastSyncTime ||
                    (c.ModifiedAt.HasValue && c.ModifiedAt >= lastSyncTime) ||
                    (c.DeletedAt.HasValue && c.DeletedAt >= lastSyncTime)
                );
            }

            var categories = await query.ToListAsync();

            foreach (var category in categories)
            {
                var action = category.IsDeleted ? "Delete" : (category.CreatedAt >= (lastSyncTime ?? DateTime.MinValue) ? "Create" : "Update");

                changes.Add(new SyncItemDto
                {
                    Id = category.Id,
                    EntityType = "Category",
                    Action = action,
                    Data = action != "Delete" ? category : null,
                    Timestamp = category.ModifiedAt ?? category.CreatedAt,
                    RowVersionString = Convert.ToBase64String(category.RowVersion)
                });
            }

            return changes;
        }

        public async Task<(int Created, int Updated, int Deleted, List<string> Errors)> ApplyChangesAsync(SyncBatchDto batch, int userId)
        {
            int created = 0;
            int updated = 0;
            int deleted = 0;
            var errors = new List<string>();

            // Start a transaction to ensure all changes are applied atomically
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                switch (batch.EntityType.ToLowerInvariant())
                {
                    case "vocabularyitems":
                        (created, updated, deleted, errors) = await ApplyVocabularyChangesAsync(batch.Changes, userId);
                        break;

                    case "categories":
                        (created, updated, deleted, errors) = await ApplyCategoryChangesAsync(batch.Changes, userId);
                        break;

                    // Add other entity types as needed
                    default:
                        errors.Add($"Unsupported entity type for sync: {batch.EntityType}");
                        break;
                }

                if (errors.Count == 0)
                {
                    // Commit the transaction if there are no errors
                    await transaction.CommitAsync();
                }
                else
                {
                    // Rollback if there are errors
                    await transaction.RollbackAsync();
                }
            }
            catch (Exception ex)
            {
                // Rollback in case of exceptions
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error applying changes for {EntityType}", batch.EntityType);
                errors.Add($"Error applying changes: {ex.Message}");
            }

            return (created, updated, deleted, errors);
        }

        private async Task<(int Created, int Updated, int Deleted, List<string> Errors)> ApplyVocabularyChangesAsync(
            List<SyncItemDto> changes, int userId)
        {
            int created = 0;
            int updated = 0;
            int deleted = 0;
            var errors = new List<string>();

            foreach (var change in changes)
            {
                try
                {
                    switch (change.Action)
                    {
                        case "Create":
                            var newItem = JsonSerializer.Deserialize<VocabularyItem>(
                                JsonSerializer.Serialize(change.Data));

                            if (newItem != null)
                            {
                                newItem.CreatedBy = userId;
                                newItem.CreatedAt = DateTime.UtcNow;
                                _dbContext.VocabularyItems.Add(newItem);
                                await _dbContext.SaveChangesAsync();
                                created++;
                            }
                            break;

                        case "Update":
                            var updateItem = JsonSerializer.Deserialize<VocabularyItem>(
                                JsonSerializer.Serialize(change.Data));

                            if (updateItem != null)
                            {
                                var existingItem = await _dbContext.VocabularyItems
                                    .Include(v => v.Definitions)
                                    .Include(v => v.Examples)
                                    .Include(v => v.Translations)
                                    .FirstOrDefaultAsync(v => v.Id == change.Id);

                                if (existingItem != null)
                                {
                                    // Update basic properties
                                    existingItem.Term = updateItem.Term;
                                    existingItem.Reading = updateItem.Reading;
                                    existingItem.CategoryId = updateItem.CategoryId;
                                    existingItem.DifficultyLevel = updateItem.DifficultyLevel;
                                    existingItem.Notes = updateItem.Notes;
                                    existingItem.Tags = updateItem.Tags;
                                    existingItem.ModifiedBy = userId;
                                    existingItem.ModifiedAt = DateTime.UtcNow;

                                    // Update related collections
                                    UpdateVocabularyRelatedEntities(existingItem, updateItem, userId);

                                    await _dbContext.SaveChangesAsync();
                                    updated++;
                                }
                                else
                                {
                                    errors.Add($"Vocabulary item with ID {change.Id} not found for update");
                                }
                            }
                            break;

                        case "Delete":
                            var existingItemToDelete = await _dbContext.VocabularyItems.FindAsync(change.Id);
                            if (existingItemToDelete != null)
                            {
                                existingItemToDelete.IsDeleted = true;
                                existingItemToDelete.DeletedBy = userId;
                                existingItemToDelete.DeletedAt = DateTime.UtcNow;
                                await _dbContext.SaveChangesAsync();
                                deleted++;
                            }
                            else
                            {
                                errors.Add($"Vocabulary item with ID {change.Id} not found for deletion");
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error applying vocabulary change for item {Id}", change.Id);
                    errors.Add($"Error with vocabulary item {change.Id}: {ex.Message}");
                }
            }

            return (created, updated, deleted, errors);
        }

        private void UpdateVocabularyRelatedEntities(VocabularyItem existingItem, VocabularyItem updateItem, int userId)
        {
            // Update definitions
            _dbContext.Definitions.RemoveRange(existingItem.Definitions);
            existingItem.Definitions = updateItem.Definitions.Select(d => new Definition
            {
                VocabularyItemId = existingItem.Id,
                Text = d.Text,
                PartOfSpeech = d.PartOfSpeech,
                SortOrder = d.SortOrder,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            // Update examples
            _dbContext.Examples.RemoveRange(existingItem.Examples);
            existingItem.Examples = updateItem.Examples.Select(e => new Example
            {
                VocabularyItemId = existingItem.Id,
                Text = e.Text,
                Translation = e.Translation,
                DifficultyLevel = e.DifficultyLevel,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            // Update translations
            _dbContext.Translations.RemoveRange(existingItem.Translations);
            existingItem.Translations = updateItem.Translations.Select(t => new Translation
            {
                VocabularyItemId = existingItem.Id,
                Text = t.Text,
                LanguageCode = t.LanguageCode,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            }).ToList();
        }

        private async Task<(int Created, int Updated, int Deleted, List<string> Errors)> ApplyCategoryChangesAsync(
            List<SyncItemDto> changes, int userId)
        {
            int created = 0;
            int updated = 0;
            int deleted = 0;
            var errors = new List<string>();

            foreach (var change in changes)
            {
                try
                {
                    switch (change.Action)
                    {
                        case "Create":
                            var newCategory = JsonSerializer.Deserialize<Category>(
                                JsonSerializer.Serialize(change.Data));

                            if (newCategory != null)
                            {
                                newCategory.CreatedBy = userId;
                                newCategory.CreatedAt = DateTime.UtcNow;
                                _dbContext.Categories.Add(newCategory);
                                await _dbContext.SaveChangesAsync();
                                created++;
                            }
                            break;

                        case "Update":
                            var updateCategory = JsonSerializer.Deserialize<Category>(
                                JsonSerializer.Serialize(change.Data));

                            if (updateCategory != null)
                            {
                                var existingCategory = await _dbContext.Categories
                                    .FirstOrDefaultAsync(c => c.Id == change.Id);

                                if (existingCategory != null)
                                {
                                    existingCategory.Name = updateCategory.Name;
                                    existingCategory.Description = updateCategory.Description;
                                    existingCategory.ParentId = updateCategory.ParentId;
                                    existingCategory.Status = updateCategory.Status;
                                    existingCategory.SortOrder = updateCategory.SortOrder;
                                    existingCategory.ModifiedBy = userId;
                                    existingCategory.ModifiedAt = DateTime.UtcNow;

                                    await _dbContext.SaveChangesAsync();
                                    updated++;
                                }
                                else
                                {
                                    errors.Add($"Category with ID {change.Id} not found for update");
                                }
                            }
                            break;

                        case "Delete":
                            var existingCategoryToDelete = await _dbContext.Categories.FindAsync(change.Id);
                            if (existingCategoryToDelete != null)
                            {
                                existingCategoryToDelete.IsDeleted = true;
                                existingCategoryToDelete.DeletedBy = userId;
                                existingCategoryToDelete.DeletedAt = DateTime.UtcNow;
                                await _dbContext.SaveChangesAsync();
                                deleted++;
                            }
                            else
                            {
                                errors.Add($"Category with ID {change.Id} not found for deletion");
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error applying category change for item {Id}", change.Id);
                    errors.Add($"Error with category {change.Id}: {ex.Message}");
                }
            }

            return (created, updated, deleted, errors);
        }
    }
}