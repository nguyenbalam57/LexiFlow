using LexiFlow.AdminDashboard.Services;
using LexiFlow.Models.Learning.Vocabulary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LexiFlow.Infrastructure.Data;
using System.Text;
using System.IO;

namespace LexiFlow.AdminDashboard.Services.Implementation
{
    /// <summary>
    /// Vocabulary Management Service Implementation
    /// </summary>
    public class VocabularyManagementService : IVocabularyManagementService
    {
        private readonly LexiFlowContext _context;
        private readonly ILogger<VocabularyManagementService> _logger;
        private readonly IApiClient _apiClient;

        public VocabularyManagementService(
            LexiFlowContext context,
            ILogger<VocabularyManagementService> logger,
            IApiClient apiClient)
        {
            _context = context;
            _logger = logger;
            _apiClient = apiClient;
        }

        #region Vocabulary CRUD Operations

        public async Task<List<Vocabulary>> GetVocabulariesAsync(int page = 1, int pageSize = 50, string searchTerm = "")
        {
            try
            {
                var query = _context.Vocabularies
                    .Include(v => v.Category)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(v => 
                        v.Term.ToLower().Contains(searchTerm) ||
                        v.Reading.ToLower().Contains(searchTerm) ||
                        v.AlternativeReadings.ToLower().Contains(searchTerm) ||
                        v.Translations.Any(t => t.Text.ToLower().Contains(searchTerm)));
                }

                return await query
                    .OrderBy(v => v.Term)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies");
                throw;
            }
        }

        public async Task<Vocabulary> GetVocabularyByIdAsync(int id)
        {
            try
            {
                return await _context.Vocabularies
                    .Include(v => v.Category)
                    .Include(v => v.Translations)
                    .FirstOrDefaultAsync(v => v.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary by ID {Id}", id);
                throw;
            }
        }

        public async Task<Vocabulary> CreateVocabularyAsync(CreateVocabularyRequest request)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                // Check if vocabulary already exists
                var existingVocabulary = await _context.Vocabularies
                    .FirstOrDefaultAsync(v => v.Term == request.Word);

                if (existingVocabulary != null)
                {
                    throw new InvalidOperationException("Vocabulary word already exists");
                }

                // Create vocabulary
                var vocabulary = new Vocabulary
                {
                    Term = request.Word,
                    Reading = request.Hiragana,
                    AlternativeReadings = request.Katakana,
                    // Note: Romaji mapping would need custom field or metadata
                    Translations = new List<Translation>
                    {
                        new Translation
                        {
                            LanguageCode = "en",
                            Text = request.Meaning,
                            CreatedBy = 1, // TODO: Get from current user
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    },
                    PartOfSpeech = request.PartOfSpeech,
                    Level = request.JLPTLevel,
                    CategoryId = request.CategoryId,
                    DifficultyLevel = request.DifficultyLevel,
                    Status = request.IsActive ? "Active" : "Inactive",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Handle additional fields in metadata
                var metadata = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(request.Romaji))
                    metadata["romaji"] = request.Romaji;
                if (!string.IsNullOrEmpty(request.ExampleSentence))
                    metadata["exampleSentence"] = request.ExampleSentence;
                if (!string.IsNullOrEmpty(request.ExampleMeaning))
                    metadata["exampleMeaning"] = request.ExampleMeaning;
                if (!string.IsNullOrEmpty(request.Notes))
                    metadata["notes"] = request.Notes;

                if (metadata.Any())
                    vocabulary.MetadataJson = System.Text.Json.JsonSerializer.Serialize(metadata);

                // Handle tags
                if (request.Tags?.Any() == true)
                    vocabulary.Tags = System.Text.Json.JsonSerializer.Serialize(request.Tags);

                _context.Vocabularies.Add(vocabulary);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Vocabulary created successfully: {Word}", request.Word);

                // Return vocabulary with includes
                return await GetVocabularyByIdAsync(vocabulary.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary: {Word}", request.Word);
                throw;
            }
        }

        public async Task<Vocabulary> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request)
        {
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                var vocabulary = await _context.Vocabularies
                    .Include(v => v.Translations)
                    .FirstOrDefaultAsync(v => v.Id == id);
                if (vocabulary == null)
                {
                    throw new ArgumentException("Vocabulary not found");
                }

                // Check if word conflicts with other vocabularies
                var existingVocabulary = await _context.Vocabularies
                    .FirstOrDefaultAsync(v => v.Id != id && v.Term == request.Word);

                if (existingVocabulary != null)
                {
                    throw new InvalidOperationException("Vocabulary word already exists");
                }

                // Update vocabulary
                vocabulary.Term = request.Word;
                vocabulary.Reading = request.Hiragana;
                vocabulary.AlternativeReadings = request.Katakana;
                vocabulary.PartOfSpeech = request.PartOfSpeech;
                vocabulary.Level = request.JLPTLevel;
                vocabulary.CategoryId = request.CategoryId;
                vocabulary.DifficultyLevel = request.DifficultyLevel;
                vocabulary.Status = request.IsActive ? "Active" : "Inactive";
                vocabulary.UpdatedAt = DateTime.UtcNow;

                // Update metadata
                var metadata = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(request.Romaji))
                    metadata["romaji"] = request.Romaji;
                if (!string.IsNullOrEmpty(request.ExampleSentence))
                    metadata["exampleSentence"] = request.ExampleSentence;
                if (!string.IsNullOrEmpty(request.ExampleMeaning))
                    metadata["exampleMeaning"] = request.ExampleMeaning;
                if (!string.IsNullOrEmpty(request.Notes))
                    metadata["notes"] = request.Notes;

                if (metadata.Any())
                    vocabulary.MetadataJson = System.Text.Json.JsonSerializer.Serialize(metadata);

                // Update tags
                if (request.Tags?.Any() == true)
                    vocabulary.Tags = System.Text.Json.JsonSerializer.Serialize(request.Tags);

                // Update translations
                var translation = vocabulary.Translations?.FirstOrDefault();
                if (translation != null)
                {
                    translation.Text = request.Meaning;
                    translation.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    vocabulary.Translations = new List<Translation>
                    {
                        new Translation
                        {
                            LanguageCode = "en",
                            Text = request.Meaning,
                            CreatedBy = 1, // TODO: Get from current user
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    };
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Vocabulary updated successfully: {VocabularyId}", id);

                // Return updated vocabulary with includes
                return await GetVocabularyByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary: {VocabularyId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteVocabularyAsync(int id, bool softDelete = true)
        {
            try
            {
                var vocabulary = await _context.Vocabularies.FindAsync(id);
                if (vocabulary == null)
                {
                    return false;
                }

                if (softDelete)
                {
                    vocabulary.Status = "Inactive";
                    vocabulary.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    _context.Vocabularies.Remove(vocabulary);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Vocabulary deleted (soft: {SoftDelete}): {VocabularyId}", softDelete, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary: {VocabularyId}", id);
                throw;
            }
        }

        public async Task<List<Vocabulary>> GetVocabulariesByCategoryAsync(int categoryId)
        {
            try
            {
                return await _context.Vocabularies
                    .Include(v => v.Category)
                    .Where(v => v.CategoryId == categoryId)
                    .OrderBy(v => v.Term)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies by category: {CategoryId}", categoryId);
                throw;
            }
        }

        #endregion

        #region Category Management

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                return await _context.Categories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.CategoryName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                throw;
            }
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryRequest request)
        {
            try
            {
                // Check if category name already exists
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryName == request.CategoryName);

                if (existingCategory != null)
                {
                    throw new InvalidOperationException("Category name already exists");
                }

                var category = new Category
                {
                    CategoryName = request.CategoryName,
                    Description = request.Description,
                    Level = request.Level,
                    CategoryType = request.CategoryType,
                    ParentCategoryId = request.ParentCategoryId,
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Category created successfully: {CategoryName}", request.CategoryName);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category: {CategoryName}", request.CategoryName);
                throw;
            }
        }

        public async Task<Category> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest request)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    throw new ArgumentException("Category not found");
                }

                // Check if category name conflicts
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId != categoryId && c.CategoryName == request.CategoryName);

                if (existingCategory != null)
                {
                    throw new InvalidOperationException("Category name already exists");
                }

                category.CategoryName = request.CategoryName;
                category.Description = request.Description;
                category.Level = request.Level;
                category.CategoryType = request.CategoryType;
                category.ParentCategoryId = request.ParentCategoryId;
                category.IsActive = request.IsActive;
                category.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Category updated successfully: {CategoryId}", categoryId);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category: {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId, bool softDelete = true)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    return false;
                }

                // Check if category has vocabularies
                var hasVocabularies = await _context.Vocabularies
                    .AnyAsync(v => v.CategoryId == categoryId);

                if (hasVocabularies && !softDelete)
                {
                    throw new InvalidOperationException("Cannot hard delete category with vocabularies");
                }

                if (softDelete)
                {
                    category.IsActive = false;
                    category.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    _context.Categories.Remove(category);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Category deleted (soft: {SoftDelete}): {CategoryId}", softDelete, categoryId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category: {CategoryId}", categoryId);
                throw;
            }
        }

        #endregion

        #region Search and Filter

        public async Task<List<Vocabulary>> SearchVocabulariesAsync(VocabularySearchFilter filter)
        {
            try
            {
                var query = _context.Vocabularies
                    .Include(v => v.Category)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    var searchTerm = filter.SearchTerm.ToLower();
                    query = query.Where(v =>
                        v.Term.ToLower().Contains(searchTerm) ||
                        v.Reading.ToLower().Contains(searchTerm) ||
                        v.AlternativeReadings.ToLower().Contains(searchTerm) ||
                        v.Translations.Any(t => t.Text.ToLower().Contains(searchTerm)));
                }

                if (filter.CategoryIds?.Any() == true)
                {
                    query = query.Where(v => filter.CategoryIds.Contains(v.CategoryId ?? 0));
                }

                if (filter.JLPTLevels?.Any() == true)
                {
                    query = query.Where(v => filter.JLPTLevels.Contains(v.Level));
                }

                if (filter.PartsOfSpeech?.Any() == true)
                {
                    query = query.Where(v => filter.PartsOfSpeech.Contains(v.PartOfSpeech));
                }

                if (filter.MinDifficulty.HasValue)
                {
                    query = query.Where(v => v.DifficultyLevel >= filter.MinDifficulty.Value);
                }

                if (filter.MaxDifficulty.HasValue)
                {
                    query = query.Where(v => v.DifficultyLevel <= filter.MaxDifficulty.Value);
                }

                if (filter.IsActive.HasValue)
                {
                    var status = filter.IsActive.Value ? "Active" : "Inactive";
                    query = query.Where(v => v.Status == status);
                }

                if (filter.CreatedFrom.HasValue)
                {
                    query = query.Where(v => v.CreatedAt >= filter.CreatedFrom.Value);
                }

                if (filter.CreatedTo.HasValue)
                {
                    query = query.Where(v => v.CreatedAt <= filter.CreatedTo.Value);
                }

                // Apply sorting
                query = filter.SortBy?.ToLower() switch
                {
                    "meaning" => filter.SortDescending ? 
                        query.OrderByDescending(v => v.Translations.FirstOrDefault().Text) : 
                        query.OrderBy(v => v.Translations.FirstOrDefault().Text),
                    "jlptlevel" => filter.SortDescending ? query.OrderByDescending(v => v.Level) : query.OrderBy(v => v.Level),
                    "difficulty" => filter.SortDescending ? query.OrderByDescending(v => v.DifficultyLevel) : query.OrderBy(v => v.DifficultyLevel),
                    "createdat" => filter.SortDescending ? query.OrderByDescending(v => v.CreatedAt) : query.OrderBy(v => v.CreatedAt),
                    _ => filter.SortDescending ? query.OrderByDescending(v => v.Term) : query.OrderBy(v => v.Term)
                };

                // Apply pagination
                return await query
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching vocabularies");
                throw;
            }
        }

        public async Task<int> GetVocabularyCountAsync(string searchTerm = "")
        {
            try
            {
                var query = _context.Vocabularies.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(v =>
                        v.Term.ToLower().Contains(searchTerm) ||
                        v.Translations.Any(t => t.Text.ToLower().Contains(searchTerm)));
                }

                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary count");
                throw;
            }
        }

        #endregion

        #region Bulk Operations

        public async Task<List<Vocabulary>> ImportVocabulariesAsync(List<CreateVocabularyRequest> vocabularies)
        {
            try
            {
                var createdVocabularies = new List<Vocabulary>();

                foreach (var vocabularyRequest in vocabularies)
                {
                    try
                    {
                        var vocabulary = await CreateVocabularyAsync(vocabularyRequest);
                        createdVocabularies.Add(vocabulary);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to import vocabulary: {Word}", vocabularyRequest.Word);
                        // Continue with other vocabularies
                    }
                }

                _logger.LogInformation("Bulk import completed: {CreatedCount}/{TotalCount} vocabularies", 
                    createdVocabularies.Count, vocabularies.Count);

                return createdVocabularies;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during bulk vocabulary import");
                throw;
            }
        }

        public async Task<byte[]> ExportVocabulariesAsync(List<int> vocabularyIds = null)
        {
            try
            {
                var query = _context.Vocabularies
                    .Include(v => v.Category)
                    .AsQueryable();

                if (vocabularyIds?.Any() == true)
                {
                    query = query.Where(v => vocabularyIds.Contains(v.Id));
                }

                var vocabularies = await query.ToListAsync();

                // Create CSV content
                var csv = new StringBuilder();
                
                // Headers
                csv.AppendLine("Word,Hiragana,Katakana,Romaji,Meaning,PartOfSpeech,JLPTLevel,Category,ExampleSentence,ExampleMeaning,Notes,Tags,DifficultyLevel,IsActive,CreatedDate");

                // Data
                foreach (var vocabulary in vocabularies)
                {
                    var meaning = vocabulary.Translations?.FirstOrDefault()?.Text ?? "";
                    var metadata = GetMetadataValues(vocabulary.MetadataJson);
                    var tags = GetTagsFromJson(vocabulary.Tags);
                    var isActive = vocabulary.Status == "Active" ? "Yes" : "No";
                    
                    csv.AppendLine($"{vocabulary.Term},{vocabulary.Reading},{vocabulary.AlternativeReadings},{metadata.GetValueOrDefault("romaji", "")},{meaning},{vocabulary.PartOfSpeech},{vocabulary.Level},{vocabulary.Category?.CategoryName},{metadata.GetValueOrDefault("exampleSentence", "")},{metadata.GetValueOrDefault("exampleMeaning", "")},{metadata.GetValueOrDefault("notes", "")},{string.Join(";", tags)},{vocabulary.DifficultyLevel},{isActive},{vocabulary.CreatedAt:yyyy-MM-dd}");
                }

                return Encoding.UTF8.GetBytes(csv.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting vocabularies");
                throw;
            }
        }

        private Dictionary<string, string> GetMetadataValues(string metadataJson)
        {
            try
            {
                if (string.IsNullOrEmpty(metadataJson))
                    return new Dictionary<string, string>();
                
                var metadata = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(metadataJson);
                return metadata.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? "");
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        private List<string> GetTagsFromJson(string tagsJson)
        {
            try
            {
                if (string.IsNullOrEmpty(tagsJson))
                    return new List<string>();
                
                return System.Text.Json.JsonSerializer.Deserialize<List<string>>(tagsJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<bool> BulkUpdateCategoryAsync(List<int> vocabularyIds, int categoryId)
        {
            try
            {
                var vocabularies = await _context.Vocabularies
                    .Where(v => vocabularyIds.Contains(v.Id))
                    .ToListAsync();

                foreach (var vocabulary in vocabularies)
                {
                    vocabulary.CategoryId = categoryId;
                    vocabulary.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Bulk category update completed: {Count} vocabularies", vocabularies.Count);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during bulk category update");
                throw;
            }
        }

        public async Task<bool> BulkDeleteAsync(List<int> vocabularyIds, bool softDelete = true)
        {
            try
            {
                var vocabularies = await _context.Vocabularies
                    .Where(v => vocabularyIds.Contains(v.Id))
                    .ToListAsync();

                if (softDelete)
                {
                    foreach (var vocabulary in vocabularies)
                    {
                        vocabulary.Status = "Inactive";
                        vocabulary.UpdatedAt = DateTime.UtcNow;
                    }
                }
                else
                {
                    _context.Vocabularies.RemoveRange(vocabularies);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Bulk delete completed (soft: {SoftDelete}): {Count} vocabularies", 
                    softDelete, vocabularies.Count);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during bulk delete");
                throw;
            }
        }

        #endregion

        #region Advanced Features

        public async Task<List<Vocabulary>> GetRandomVocabulariesAsync(int count, string level = null)
        {
            try
            {
                var query = _context.Vocabularies
                    .Include(v => v.Category)
                    .Where(v => v.Status == "Active")
                    .AsQueryable();

                if (!string.IsNullOrEmpty(level))
                {
                    query = query.Where(v => v.Level == level);
                }

                return await query
                    .OrderBy(v => Guid.NewGuid())
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random vocabularies");
                throw;
            }
        }

        public async Task<List<Vocabulary>> GetRecentVocabulariesAsync(int count = 10)
        {
            try
            {
                return await _context.Vocabularies
                    .Include(v => v.Category)
                    .Where(v => v.Status == "Active")
                    .OrderByDescending(v => v.CreatedAt)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent vocabularies");
                throw;
            }
        }

        public async Task<VocabularyStatistics> GetVocabularyStatisticsAsync()
        {
            try
            {
                var totalVocabularies = await _context.Vocabularies.CountAsync();
                var activeVocabularies = await _context.Vocabularies.CountAsync(v => v.Status == "Active");
                var vocabulariesCreatedThisMonth = await _context.Vocabularies
                    .CountAsync(v => v.CreatedAt >= DateTime.UtcNow.AddDays(-30));

                var vocabulariesByJLPTLevel = await _context.Vocabularies
                    .Where(v => v.Status == "Active")
                    .GroupBy(v => v.Level)
                    .Select(g => new { Level = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Level ?? "Unknown", x => x.Count);

                var vocabulariesByCategory = await _context.Vocabularies
                    .Where(v => v.Status == "Active")
                    .Include(v => v.Category)
                    .GroupBy(v => v.Category.CategoryName)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Category ?? "Uncategorized", x => x.Count);

                var vocabulariesByPartOfSpeech = await _context.Vocabularies
                    .Where(v => v.Status == "Active")
                    .GroupBy(v => v.PartOfSpeech)
                    .Select(g => new { PartOfSpeech = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.PartOfSpeech ?? "Unknown", x => x.Count);

                var vocabulariesByDifficulty = await _context.Vocabularies
                    .Where(v => v.Status == "Active")
                    .GroupBy(v => v.DifficultyLevel)
                    .Select(g => new { Difficulty = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Difficulty, x => x.Count);

                // Calculate vocabularies with audio from metadata
                var vocabulariesWithAudio = await _context.Vocabularies
                    .Where(v => v.Status == "Active" && v.MetadataJson.Contains("hasAudio"))
                    .CountAsync();

                return new VocabularyStatistics
                {
                    TotalVocabularies = totalVocabularies,
                    ActiveVocabularies = activeVocabularies,
                    InactiveVocabularies = totalVocabularies - activeVocabularies,
                    VocabulariesWithAudio = vocabulariesWithAudio,
                    VocabulariesCreatedThisMonth = vocabulariesCreatedThisMonth,
                    VocabulariesByJLPTLevel = vocabulariesByJLPTLevel,
                    VocabulariesByCategory = vocabulariesByCategory,
                    VocabulariesByPartOfSpeech = vocabulariesByPartOfSpeech,
                    VocabulariesByDifficulty = vocabulariesByDifficulty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary statistics");
                throw;
            }
        }

        public async Task<List<Vocabulary>> GetRelatedVocabulariesAsync(int vocabularyId)
        {
            try
            {
                var vocabulary = await _context.Vocabularies.FindAsync(vocabularyId);
                if (vocabulary == null)
                {
                    return new List<Vocabulary>();
                }

                // Find related vocabularies by category, JLPT level, or part of speech
                return await _context.Vocabularies
                    .Include(v => v.Category)
                    .Where(v => v.Id != vocabularyId && v.Status == "Active" && 
                        (v.CategoryId == vocabulary.CategoryId ||
                         v.Level == vocabulary.Level ||
                         v.PartOfSpeech == vocabulary.PartOfSpeech))
                    .OrderBy(v => v.Term)
                    .Take(10)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting related vocabularies for {VocabularyId}", vocabularyId);
                throw;
            }
        }

        #endregion

        #region Audio Management

        public async Task<bool> UploadAudioFileAsync(int vocabularyId, byte[] audioData, string fileName)
        {
            try
            {
                var vocabulary = await _context.Vocabularies.FindAsync(vocabularyId);
                if (vocabulary == null)
                {
                    return false;
                }

                // For now, we'll just update the metadata to indicate audio was uploaded
                // In a full implementation, you would save the audio file and create a MediaFile record
                var metadata = string.IsNullOrEmpty(vocabulary.MetadataJson) ? "{}" : vocabulary.MetadataJson;
                var metadataObj = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(metadata);
                metadataObj["hasAudio"] = true;
                metadataObj["audioFileName"] = fileName;
                metadataObj["audioUploadedAt"] = DateTime.UtcNow.ToString("O");
                
                vocabulary.MetadataJson = System.Text.Json.JsonSerializer.Serialize(metadataObj);
                vocabulary.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Audio file uploaded for vocabulary: {VocabularyId}", vocabularyId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading audio file for vocabulary: {VocabularyId}", vocabularyId);
                throw;
            }
        }

        public async Task<byte[]> GetAudioFileAsync(int vocabularyId)
        {
            try
            {
                // Placeholder implementation - in production would retrieve from file storage
                return new byte[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting audio file for vocabulary: {VocabularyId}", vocabularyId);
                throw;
            }
        }

        public async Task<bool> DeleteAudioFileAsync(int vocabularyId)
        {
            try
            {
                var vocabulary = await _context.Vocabularies.FindAsync(vocabularyId);
                if (vocabulary == null)
                {
                    return false;
                }

                // Remove audio metadata
                if (!string.IsNullOrEmpty(vocabulary.MetadataJson))
                {
                    var metadataObj = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(vocabulary.MetadataJson);
                    metadataObj.Remove("hasAudio");
                    metadataObj.Remove("audioFileName");
                    metadataObj.Remove("audioUploadedAt");
                    vocabulary.MetadataJson = System.Text.Json.JsonSerializer.Serialize(metadataObj);
                }

                vocabulary.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Audio file deleted for vocabulary: {VocabularyId}", vocabularyId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting audio file for vocabulary: {VocabularyId}", vocabularyId);
                throw;
            }
        }

        #endregion
    }
}