using LexiFlow.API.DTOs.Vocabulary;
using LexiFlow.API.DTOs.Common;
using LexiFlow.API.Services.Vocabulary;
using LexiFlow.Infrastructure.Data.Repositories.Vocabulary;
using LexiFlow.Infrastructure.Data;
using AutoMapper;

namespace LexiFlow.API.Services.Vocabulary
{
    /// <summary>
    /// Service implementation cho Vocabulary
    /// </summary>
    public class VocabularyService : IVocabularyService
    {
        private readonly IVocabularyRepository _vocabularyRepository;
        private readonly LexiFlowContext _context;
        private readonly ILogger<VocabularyService> _logger;

        public VocabularyService(
            IVocabularyRepository vocabularyRepository, 
            LexiFlowContext context,
            ILogger<VocabularyService> logger)
        {
            _vocabularyRepository = vocabularyRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<PaginatedResultDto<VocabularyDto>> GetVocabulariesAsync(
            int page = 1, 
            int pageSize = 10, 
            string? searchTerm = null,
            int? categoryId = null,
            string? level = null,
            string? partOfSpeech = null,
            bool? isActive = true)
        {
            try
            {
                var result = await _vocabularyRepository.SearchAsync(
                    searchTerm, categoryId, level, partOfSpeech, isActive, page, pageSize);

                var vocabularyDtos = result.Items.Select(MapToDto).ToList();

                return new PaginatedResultDto<VocabularyDto>
                {
                    Data = vocabularyDtos,
                    TotalCount = result.TotalCount,
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies with search parameters");
                throw;
            }
        }

        public async Task<VocabularyDto?> GetVocabularyByIdAsync(int id)
        {
            try
            {
                var vocabulary = await _vocabularyRepository.GetByIdAsync(id);
                return vocabulary != null ? MapToDto(vocabulary) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary by id {VocabularyId}", id);
                throw;
            }
        }

        public async Task<VocabularyDto?> GetVocabularyByTermAsync(string term, string languageCode = "ja")
        {
            try
            {
                var vocabulary = await _vocabularyRepository.GetByTermAsync(term, languageCode);
                return vocabulary != null ? MapToDto(vocabulary) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary by term {Term}", term);
                throw;
            }
        }

        public async Task<VocabularyDto> CreateVocabularyAsync(CreateVocabularyDto createDto, int createdBy)
        {
            try
            {
                // Ki?m tra t? ?ã t?n t?i ch?a
                var exists = await _vocabularyRepository.ExistsByTermAsync(createDto.Word);
                if (exists)
                {
                    throw new InvalidOperationException($"Vocabulary with term '{createDto.Word}' already exists");
                }

                var vocabulary = new Models.Learning.Vocabulary.Vocabulary
                {
                    Term = createDto.Word,
                    Reading = createDto.Hiragana, // Map Hiragana to Reading
                    AlternativeReadings = createDto.Katakana, // Map Katakana to Alternative
                    Level = createDto.Level,
                    CategoryId = createDto.CategoryId,
                    PartOfSpeech = createDto.WordType, // Map WordType to PartOfSpeech
                    DifficultyLevel = createDto.Difficulty,
                    FrequencyRank = createDto.Frequency,
                    IpaNotation = createDto.IpaNotation,
                    IsCommon = createDto.IsCommon,
                    UsageNotes = createDto.Notes,
                    IsActive = true,
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    LanguageCode = "ja", // Default to Japanese
                    Status = "Active"
                };

                // Thêm definitions t? meaning
                if (!string.IsNullOrEmpty(createDto.Meaning))
                {
                    vocabulary.Definitions = new List<Models.Learning.Vocabulary.Definition>
                    {
                        new Models.Learning.Vocabulary.Definition
                        {
                            Text = createDto.Meaning, // Property name is Text
                            LanguageCode = "vi", // Vietnamese by default
                            CreatedBy = createdBy,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            IsActive = true
                        }
                    };
                }

                // Thêm examples n?u có
                if (!string.IsNullOrEmpty(createDto.Example))
                {
                    vocabulary.Examples = new List<Models.Learning.Vocabulary.Example>
                    {
                        new Models.Learning.Vocabulary.Example
                        {
                            Text = createDto.Example, // Property name is Text
                            Translation = createDto.ExampleMeaning,
                            CreatedBy = createdBy,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            IsActive = true
                        }
                    };
                }

                await _vocabularyRepository.AddAsync(vocabulary);
                await _context.SaveChangesAsync();

                // Reload with includes
                var createdVocabulary = await _vocabularyRepository.GetByIdAsync(vocabulary.Id);
                return MapToDto(createdVocabulary!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vocabulary");
                throw;
            }
        }

        public async Task<VocabularyDto?> UpdateVocabularyAsync(int id, UpdateVocabularyDto updateDto, int modifiedBy)
        {
            try
            {
                var vocabulary = await _vocabularyRepository.GetByIdAsync(id);
                if (vocabulary == null)
                {
                    return null;
                }

                // Ki?m tra term m?i có trung không
                if (!string.IsNullOrEmpty(updateDto.Word) && updateDto.Word != vocabulary.Term)
                {
                    var exists = await _vocabularyRepository.ExistsByTermAsync(updateDto.Word, "ja", id);
                    if (exists)
                    {
                        throw new InvalidOperationException($"Vocabulary with term '{updateDto.Word}' already exists");
                    }
                    vocabulary.Term = updateDto.Word;
                }

                // Update fields if provided
                if (!string.IsNullOrEmpty(updateDto.Hiragana)) vocabulary.Reading = updateDto.Hiragana;
                if (!string.IsNullOrEmpty(updateDto.Katakana)) vocabulary.AlternativeReadings = updateDto.Katakana;
                if (!string.IsNullOrEmpty(updateDto.Romaji)) vocabulary.IpaNotation = updateDto.Romaji; // Use IPA for Romaji
                if (!string.IsNullOrEmpty(updateDto.Level)) vocabulary.Level = updateDto.Level;
                if (updateDto.CategoryId.HasValue) vocabulary.CategoryId = updateDto.CategoryId.Value;
                if (!string.IsNullOrEmpty(updateDto.WordType)) vocabulary.PartOfSpeech = updateDto.WordType;
                if (updateDto.Difficulty.HasValue) vocabulary.DifficultyLevel = updateDto.Difficulty.Value;
                if (updateDto.Frequency.HasValue) vocabulary.FrequencyRank = updateDto.Frequency.Value;
                if (!string.IsNullOrEmpty(updateDto.IpaNotation)) vocabulary.IpaNotation = updateDto.IpaNotation;
                if (updateDto.IsCommon.HasValue) vocabulary.IsCommon = updateDto.IsCommon.Value;
                if (!string.IsNullOrEmpty(updateDto.Notes)) vocabulary.UsageNotes = updateDto.Notes;
                if (updateDto.IsActive.HasValue) vocabulary.IsActive = updateDto.IsActive.Value;

                vocabulary.ModifiedBy = modifiedBy;
                vocabulary.UpdatedAt = DateTime.UtcNow;

                _vocabularyRepository.Update(vocabulary);
                await _context.SaveChangesAsync();

                // Reload with includes
                var updatedVocabulary = await _vocabularyRepository.GetByIdAsync(id);
                return MapToDto(updatedVocabulary!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vocabulary {VocabularyId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteVocabularyAsync(int id, int deletedBy)
        {
            try
            {
                var vocabulary = await _vocabularyRepository.GetByIdAsync(id);
                if (vocabulary == null)
                {
                    return false;
                }

                vocabulary.IsDeleted = true;
                vocabulary.DeletedAt = DateTime.UtcNow;
                vocabulary.DeletedBy = deletedBy;

                _vocabularyRepository.Update(vocabulary);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vocabulary {VocabularyId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<VocabularyDto>> GetVocabulariesByCategoryAsync(int categoryId)
        {
            try
            {
                var vocabularies = await _vocabularyRepository.GetByCategoryAsync(categoryId);
                return vocabularies.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies by category {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<IEnumerable<VocabularyDto>> GetVocabulariesByLevelAsync(string level)
        {
            try
            {
                var vocabularies = await _vocabularyRepository.GetByLevelAsync(level);
                return vocabularies.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabularies by level {Level}", level);
                throw;
            }
        }

        public async Task<IEnumerable<VocabularyDto>> GetRandomVocabulariesAsync(int count = 10, string? level = null)
        {
            try
            {
                var vocabularies = await _vocabularyRepository.GetRandomAsync(count, level);
                return vocabularies.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting random vocabularies");
                throw;
            }
        }

        public async Task<IEnumerable<VocabularyDto>> GetMostCommonVocabulariesAsync(int count = 10)
        {
            try
            {
                var vocabularies = await _vocabularyRepository.GetMostCommonAsync(count);
                return vocabularies.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting most common vocabularies");
                throw;
            }
        }

        public async Task<IEnumerable<VocabularyDto>> GetRecentVocabulariesAsync(int count = 10)
        {
            try
            {
                var vocabularies = await _vocabularyRepository.GetRecentlyAddedAsync(count);
                return vocabularies.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent vocabularies");
                throw;
            }
        }

        public async Task<bool> VocabularyExistsAsync(string term, string languageCode = "ja", int? excludeId = null)
        {
            try
            {
                return await _vocabularyRepository.ExistsByTermAsync(term, languageCode, excludeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking vocabulary exists for term {Term}", term);
                throw;
            }
        }

        public async Task<VocabularyStatisticsDto> GetVocabularyStatisticsAsync()
        {
            try
            {
                var stats = await _vocabularyRepository.GetStatisticsAsync();
                
                return new VocabularyStatisticsDto
                {
                    TotalCount = stats.TotalCount,
                    ActiveCount = stats.ActiveCount,
                    InactiveCount = stats.InactiveCount,
                    CountByLevel = stats.CountByLevel,
                    CountByCategory = stats.CountByCategory,
                    CountByWordType = stats.CountByWordType,
                    LastUpdated = stats.LastUpdated
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary statistics");
                throw;
            }
        }

        /// <summary>
        /// Map Vocabulary entity to DTO
        /// </summary>
        private VocabularyDto MapToDto(Models.Learning.Vocabulary.Vocabulary vocabulary)
        {
            var dto = new VocabularyDto
            {
                VocabularyId = vocabulary.Id,
                Word = vocabulary.Term,
                Hiragana = vocabulary.Reading ?? "",
                Katakana = vocabulary.AlternativeReadings ?? "",
                Romaji = vocabulary.IpaNotation ?? "", // Using IPA for Romaji
                Level = vocabulary.Level,
                CategoryId = vocabulary.CategoryId ?? 0,
                CategoryName = vocabulary.Category?.CategoryName ?? "",
                WordType = vocabulary.PartOfSpeech ?? "",
                Difficulty = vocabulary.DifficultyLevel,
                Frequency = vocabulary.FrequencyRank,
                IpaNotation = vocabulary.IpaNotation ?? "",
                IsCommon = vocabulary.IsCommon,
                Notes = vocabulary.UsageNotes ?? "",
                IsActive = vocabulary.IsActive,
                CreatedAt = vocabulary.CreatedAt,
                UpdatedAt = vocabulary.UpdatedAt
            };

            // Map definitions - using Text property
            if (vocabulary.Definitions?.Any() == true)
            {
                dto.Meaning = string.Join("; ", vocabulary.Definitions.Select(d => d.Text));
            }

            // Map examples - using Text property
            if (vocabulary.Examples?.Any() == true)
            {
                var firstExample = vocabulary.Examples.First();
                dto.Example = firstExample.Text;
                dto.ExampleMeaning = firstExample.Translation ?? "";
            }

            return dto;
        }
    }
}