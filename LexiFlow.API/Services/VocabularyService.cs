using LexiFlow.API.Data;
using LexiFlow.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using LexiFlow.Models;

namespace LexiFlow.API.Services
{
    public class VocabularyService : IVocabularyService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<VocabularyService> _logger;

        public VocabularyService(ApplicationDbContext dbContext, ILogger<VocabularyService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<(IEnumerable<Vocabulary> Items, int TotalCount, int Page, int PageSize, int TotalPages)> GetVocabularyAsync(
            int userId, int page = 1, int pageSize = 50, DateTime? lastSync = null)
        {
            IQueryable<Vocabulary> query = _dbContext.VocabularyItems
                .Include(v => v.Definitions)
                .Include(v => v.Examples)
                .Include(v => v.Translations)
                .Include(v => v.Category)
                .AsQueryable();

            if (lastSync.HasValue)
            {
                query = query.Where(v => v.CreatedAt >= lastSync || v.ModifiedAt >= lastSync);
            }

            int totalCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query
                .OrderByDescending(v => v.ModifiedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount, page, pageSize, totalPages);
        }

        public async Task<VocabularyItem?> GetByIdAsync(int id)
        {
            return await _dbContext.VocabularyItems
                .Include(v => v.Definitions)
                .Include(v => v.Examples)
                .Include(v => v.Translations)
                .Include(v => v.Category)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<VocabularyItem?> CreateAsync(CreateVocabularyDto dto, int userId)
        {
            try
            {
                var vocabulary = new VocabularyItem
                {
                    Term = dto.Term,
                    LanguageCode = dto.LanguageCode,
                    Reading = dto.Reading,
                    CategoryId = dto.CategoryId,
                    DifficultyLevel = dto.DifficultyLevel,
                    Notes = dto.Notes,
                    Tags = dto.Tags,
                    Status = "Active",
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow
                };

                // Add definitions
                vocabulary.Definitions = dto.Definitions.Select(d => new Definition
                {
                    Text = d.Text,
                    PartOfSpeech = d.PartOfSpeech,
                    SortOrder = d.SortOrder,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                // Add examples if provided
                if (dto.Examples != null && dto.Examples.Any())
                {
                    vocabulary.Examples = dto.Examples.Select(e => new Example
                    {
                        Text = e.Text,
                        Translation = e.Translation,
                        DifficultyLevel = e.DifficultyLevel,
                        CreatedBy = userId,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();
                }

                // Add translations if provided
                if (dto.Translations != null && dto.Translations.Any())
                {
                    vocabulary.Translations = dto.Translations.Select(t => new Translation
                    {
                        Text = t.Text,
                        LanguageCode = t.LanguageCode,
                        CreatedBy = userId,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();
                }

                _dbContext.VocabularyItems.Add(vocabulary);
                await _dbContext.SaveChangesAsync();

                // Add user activity log
                await LogUserActivityAsync(userId, "Create", "Vocabulary", $"Created vocabulary item: {vocabulary.Term}");

                return vocabulary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary item {Term}", dto.Term);
                return null;
            }
        }

        public async Task<VocabularyItem?> UpdateAsync(int id, UpdateVocabularyDto dto, int userId)
        {
            try
            {
                var vocabulary = await _dbContext.VocabularyItems
                    .Include(v => v.Definitions)
                    .Include(v => v.Examples)
                    .Include(v => v.Translations)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (vocabulary == null)
                    return null;

                // Verify row version to prevent concurrency conflicts
                if (!string.IsNullOrEmpty(dto.RowVersionString))
                {
                    byte[] rowVersion = Convert.FromBase64String(dto.RowVersionString);
                    if (!vocabulary.RowVersion.SequenceEqual(rowVersion))
                    {
                        throw new DbUpdateConcurrencyException("The vocabulary item has been modified by another user.");
                    }
                }

                // Update basic properties
                vocabulary.Term = dto.Term;
                vocabulary.LanguageCode = dto.LanguageCode;
                vocabulary.Reading = dto.Reading;
                vocabulary.CategoryId = dto.CategoryId;
                vocabulary.DifficultyLevel = dto.DifficultyLevel;
                vocabulary.Notes = dto.Notes;
                vocabulary.Tags = dto.Tags;
                vocabulary.ModifiedBy = userId;
                vocabulary.ModifiedAt = DateTime.UtcNow;

                // Update definitions (remove existing and add new ones)
                _dbContext.Definitions.RemoveRange(vocabulary.Definitions);
                vocabulary.Definitions = dto.Definitions.Select(d => new Definition
                {
                    VocabularyItemId = vocabulary.Id,
                    Text = d.Text,
                    PartOfSpeech = d.PartOfSpeech,
                    SortOrder = d.SortOrder,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                // Update examples
                _dbContext.Examples.RemoveRange(vocabulary.Examples);
                if (dto.Examples != null && dto.Examples.Any())
                {
                    vocabulary.Examples = dto.Examples.Select(e => new Example
                    {
                        VocabularyItemId = vocabulary.Id,
                        Text = e.Text,
                        Translation = e.Translation,
                        DifficultyLevel = e.DifficultyLevel,
                        CreatedBy = userId,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();
                }

                // Update translations
                _dbContext.Translations.RemoveRange(vocabulary.Translations);
                if (dto.Translations != null && dto.Translations.Any())
                {
                    vocabulary.Translations = dto.Translations.Select(t => new Translation
                    {
                        VocabularyItemId = vocabulary.Id,
                        Text = t.Text,
                        LanguageCode = t.LanguageCode,
                        CreatedBy = userId,
                        CreatedAt = DateTime.UtcNow
                    }).ToList();
                }

                await _dbContext.SaveChangesAsync();

                // Add user activity log
                await LogUserActivityAsync(userId, "Update", "Vocabulary", $"Updated vocabulary item: {vocabulary.Term}");

                return vocabulary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary item {Id}", id);
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            try
            {
                var vocabulary = await _dbContext.VocabularyItems.FindAsync(id);
                if (vocabulary == null)
                    return false;

                // Perform soft delete
                vocabulary.IsDeleted = true;
                vocabulary.DeletedBy = userId;
                vocabulary.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                // Add user activity log
                await LogUserActivityAsync(userId, "Delete", "Vocabulary", $"Deleted vocabulary item: {vocabulary.Term}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary item {Id}", id);
                return false;
            }
        }

        public async Task<IEnumerable<VocabularyItem>> SearchAsync(string query, string languageCode = "all", int maxResults = 20)
        {
            try
            {
                IQueryable<VocabularyItem> vocabularyQuery = _dbContext.VocabularyItems
                    .Include(v => v.Definitions)
                    .Include(v => v.Translations)
                    .Where(v => !v.IsDeleted);

                // Apply language filter if specified
                if (languageCode != "all")
                {
                    vocabularyQuery = vocabularyQuery.Where(v => v.LanguageCode == languageCode);
                }

                // Apply search query
                if (!string.IsNullOrWhiteSpace(query))
                {
                    string normalizedQuery = query.Trim().ToLower();
                    vocabularyQuery = vocabularyQuery.Where(v =>
                        v.Term.ToLower().Contains(normalizedQuery) ||
                        v.Reading.ToLower().Contains(normalizedQuery) ||
                        v.Definitions.Any(d => d.Text.ToLower().Contains(normalizedQuery)) ||
                        v.Translations.Any(t => t.Text.ToLower().Contains(normalizedQuery))
                    );
                }

                return await vocabularyQuery
                    .OrderByDescending(v => v.Term.ToLower() == query.ToLower()) // Exact matches first
                    .ThenBy(v => v.Term)
                    .Take(maxResults)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching vocabulary with query {Query}", query);
                return new List<VocabularyItem>();
            }
        }

        private async Task LogUserActivityAsync(int userId, string action, string module, string details)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(userId);
                var activity = new UserActivity
                {
                    UserId = userId,
                    Username = user?.Username ?? "Unknown",
                    Action = action,
                    Module = module,
                    Details = details,
                    Timestamp = DateTime.UtcNow,
                    IpAddress = "API"
                };

                _dbContext.UserActivities.Add(activity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging user activity");
            }
        }
    }
}