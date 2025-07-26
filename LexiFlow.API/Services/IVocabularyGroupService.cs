using LexiFlow.API.Data;
using LexiFlow.API.Data.Repositories;
using LexiFlow.API.Models;
using LexiFlow.API.Models.DTOs;
using LexiFlow.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LexiFlow.API.Services
{
    /// <summary>
    /// Service for managing vocabulary groups
    /// </summary>
    public interface IVocabularyGroupService
    {
        /// <summary>
        /// Get all vocabulary groups with optional filtering
        /// </summary>
        Task<List<VocabularyGroup>> GetAllAsync(bool includeInactive = false, int? categoryId = null);

        /// <summary>
        /// Get a vocabulary group by ID
        /// </summary>
        Task<VocabularyGroup?> GetByIdAsync(int id);

        /// <summary>
        /// Create a new vocabulary group
        /// </summary>
        Task<VocabularyGroup?> CreateAsync(CreateVocabularyGroupDto dto, int userId);

        /// <summary>
        /// Update an existing vocabulary group
        /// </summary>
        Task<VocabularyGroup?> UpdateAsync(int id, UpdateVocabularyGroupDto dto, int userId);

        /// <summary>
        /// Delete a vocabulary group
        /// </summary>
        Task<bool> DeleteAsync(int id, int userId);

        /// <summary>
        /// Get vocabularies by group ID with pagination
        /// </summary>
        Task<PagedResult<Vocabulary>> GetVocabulariesByGroupAsync(int groupId, int page, int pageSize);
    }

    /// <summary>
    /// Implementation of IVocabularyGroupService
    /// </summary>
    public class VocabularyGroupService : IVocabularyGroupService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<VocabularyGroupService> _logger;

        public VocabularyGroupService(ApplicationDbContext dbContext, ILogger<VocabularyGroupService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<VocabularyGroup>> GetAllAsync(bool includeInactive = false, int? categoryId = null)
        {
            var query = _dbContext.VocabularyGroups.AsQueryable();

            // Apply filters
            if (!includeInactive)
            {
                query = query.Where(g => g.IsActive);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(g => g.CategoryID == categoryId);
            }

            // Include related entities and order results
            return await query
                .Include(g => g.Category)
                .OrderBy(g => g.GroupName)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<VocabularyGroup?> GetByIdAsync(int id)
        {
            return await _dbContext.VocabularyGroups
                .Include(g => g.Category)
                .FirstOrDefaultAsync(g => g.GroupID == id);
        }

        /// <inheritdoc />
        public async Task<VocabularyGroup?> CreateAsync(CreateVocabularyGroupDto dto, int userId)
        {
            var group = new VocabularyGroup
            {
                GroupName = dto.GroupName,
                Description = dto.Description,
                CategoryID = dto.CategoryId,
                CreatedByUserID = userId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.VocabularyGroups.Add(group);
            await _dbContext.SaveChangesAsync();

            return group;
        }

        /// <inheritdoc />
        public async Task<VocabularyGroup?> UpdateAsync(int id, UpdateVocabularyGroupDto dto, int userId)
        {
            var group = await _dbContext.VocabularyGroups.FindAsync(id);

            if (group == null)
            {
                return null;
            }

            // Check if RowVersion matches
            if (!string.IsNullOrEmpty(dto.RowVersionString))
            {
                byte[] rowVersion = Convert.FromBase64String(dto.RowVersionString);
                _dbContext.Entry(group).OriginalValues["RowVersion"] = rowVersion;
            }

            // Update properties
            group.GroupName = dto.GroupName;
            group.Description = dto.Description;
            group.CategoryID = dto.CategoryId;
            group.IsActive = dto.IsActive;
            group.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _dbContext.SaveChangesAsync();
                return group;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflict
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var group = await _dbContext.VocabularyGroups.FindAsync(id);

            if (group == null)
            {
                return false;
            }

            // Check if the group has associated vocabularies
            var hasVocabularies = await _dbContext.Vocabulary.AnyAsync(v => v.GroupID == id);

            if (hasVocabularies)
            {
                // Soft delete by marking as inactive
                group.IsActive = false;
                group.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                // Hard delete if no associated vocabularies
                _dbContext.VocabularyGroups.Remove(group);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<PagedResult<Vocabulary>> GetVocabulariesByGroupAsync(int groupId, int page, int pageSize)
        {
            // Check if group exists
            var groupExists = await _dbContext.VocabularyGroups.AnyAsync(g => g.GroupID == groupId);
            if (!groupExists)
            {
                return null;
            }

            // Calculate pagination
            var skip = (page - 1) * pageSize;

            // Get total count
            var totalCount = await _dbContext.Vocabulary.CountAsync(v => v.GroupID == groupId);

            // Get paginated results
            var items = await _dbContext.Vocabulary
                .Where(v => v.GroupID == groupId)
                .OrderBy(v => v.Japanese)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Vocabulary>
            {
                Items = items,
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}