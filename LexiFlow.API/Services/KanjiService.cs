using LexiFlow.API.Data.UnitOfWork;
using LexiFlow.API.DTOs.Kanji;
using LexiFlow.API.DTOs.Vocabulary;
using LexiFlow.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LexiFlow.API.Services
{
    public partial class KanjiService : IKanjiService
    {
        #region Component Management

        /// <summary>
        /// Get all available Kanji components
        /// </summary>
        /// <returns>List of Kanji components</returns>
        public async Task<IEnumerable<KanjiComponent>> GetAllComponentsAsync()
        {
            try
            {
                var components = await _unitOfWork.KanjiComponents.GetAll()
                    .OrderBy(c => c.Type)
                    .ThenBy(c => c.Character)
                    .ToListAsync();

                return components;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Kanji components");
                throw;
            }
        }

        /// <summary>
        /// Get a Kanji component by ID
        /// </summary>
        /// <param name="componentId">Component ID</param>
        /// <returns>Kanji component or null if not found</returns>
        public async Task<KanjiComponent> GetComponentByIdAsync(int componentId)
        {
            try
            {
                var component = await _unitOfWork.KanjiComponents.GetByIdAsync(componentId);
                return component;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Kanji component with ID: {ComponentId}", componentId);
                throw;
            }
        }

        /// <summary>
        /// Create a new Kanji component
        /// </summary>
        /// <param name="dto">Component creation data</param>
        /// <param name="userId">ID of the user creating the component</param>
        /// <returns>Created Kanji component</returns>
        public async Task<KanjiComponent> CreateComponentAsync(CreateKanjiComponentDto dto, int userId)
        {
            try
            {
                // Check if component already exists
                var existingComponent = await _unitOfWork.KanjiComponents.GetAll()
                    .FirstOrDefaultAsync(c => c.Character == dto.Character && c.Type == dto.Type);

                if (existingComponent != null)
                {
                    throw new InvalidOperationException($"Component with character '{dto.Character}' and type '{dto.Type}' already exists");
                }

                // Create new component
                var component = new KanjiComponent
                {
                    Character = dto.Character,
                    Name = dto.Name,
                    Meaning = dto.Meaning,
                    Type = dto.Type,
                    StrokeCount = dto.StrokeCount,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.KanjiComponents.AddAsync(component);
                await _unitOfWork.SaveChangesAsync();

                return component;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Kanji component: {Character}", dto.Character);
                throw;
            }
        }

        /// <summary>
        /// Update an existing Kanji component
        /// </summary>
        /// <param name="id">Component ID</param>
        /// <param name="dto">Component update data</param>
        /// <param name="userId">ID of the user updating the component</param>
        /// <returns>Updated Kanji component or null if not found</returns>
        public async Task<KanjiComponent> UpdateComponentAsync(int id, UpdateKanjiComponentDto dto, int userId)
        {
            try
            {
                var component = await _unitOfWork.KanjiComponents.GetByIdAsync(id);
                if (component == null)
                {
                    return null;
                }

                // Validate concurrency token
                var rowVersion = Convert.FromBase64String(dto.RowVersionString);
                if (!rowVersion.SequenceEqual(component.RowVersion))
                {
                    throw new DbUpdateConcurrencyException("The component has been modified by another user");
                }

                // Update properties
                component.Character = dto.Character;
                component.Name = dto.Name;
                component.Meaning = dto.Meaning;
                component.Type = dto.Type;
                component.StrokeCount = dto.StrokeCount;

                _unitOfWork.KanjiComponents.Update(component);
                await _unitOfWork.SaveChangesAsync();

                return component;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Kanji component with ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Delete a Kanji component
        /// </summary>
        /// <param name="id">Component ID</param>
        /// <param name="userId">ID of the user deleting the component</param>
        /// <returns>True if deleted, false if not found</returns>
        public async Task<bool> DeleteComponentAsync(int id, int userId)
        {
            try
            {
                var component = await _unitOfWork.KanjiComponents.GetByIdAsync(id);
                if (component == null)
                {
                    return false;
                }

                // Check if component is used in any Kanji
                var componentMappings = await _unitOfWork.KanjiComponentMappings.GetAll()
                    .Where(m => m.ComponentID == id)
                    .ToListAsync();

                if (componentMappings.Any())
                {
                    throw new InvalidOperationException("Cannot delete component that is used in Kanji");
                }

                _unitOfWork.KanjiComponents.Remove(component);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Kanji component with ID: {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Add a component to a Kanji
        /// </summary>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="dto">Component mapping data</param>
        /// <param name="userId">ID of the user adding the component</param>
        /// <returns>Created component mapping</returns>
        public async Task<KanjiComponentMapping> AddComponentToKanjiAsync(int kanjiId, AddKanjiComponentDto dto, int userId)
        {
            try
            {
                // Check if Kanji exists
                var kanji = await _unitOfWork.Kanjis.GetByIdAsync(kanjiId);
                if (kanji == null)
                {
                    throw new ArgumentException($"Kanji with ID {kanjiId} not found");
                }

                // Check if component exists
                var component = await _unitOfWork.KanjiComponents.GetByIdAsync(dto.ComponentId);
                if (component == null)
                {
                    throw new ArgumentException($"Component with ID {dto.ComponentId} not found");
                }

                // Check if mapping already exists
                var existingMapping = await _unitOfWork.KanjiComponentMappings.GetAll()
                    .FirstOrDefaultAsync(m => m.KanjiID == kanjiId && m.ComponentID == dto.ComponentId);

                if (existingMapping != null)
                {
                    throw new InvalidOperationException($"Component already added to Kanji");
                }

                // Create new mapping
                var mapping = new KanjiComponentMapping
                {
                    KanjiID = kanjiId,
                    ComponentID = dto.ComponentId,
                    Position = dto.Position,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.KanjiComponentMappings.AddAsync(mapping);
                await _unitOfWork.SaveChangesAsync();

                // Load navigation properties
                mapping.Kanji = kanji;
                mapping.Component = component;

                return mapping;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding component to Kanji with ID: {KanjiId}", kanjiId);
                throw;
            }
        }

        /// <summary>
        /// Remove a component from a Kanji
        /// </summary>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="componentId">Component ID</param>
        /// <param name="userId">ID of the user removing the component</param>
        /// <returns>True if removed, false if not found</returns>
        public async Task<bool> RemoveComponentFromKanjiAsync(int kanjiId, int componentId, int userId)
        {
            try
            {
                var mapping = await _unitOfWork.KanjiComponentMappings.GetAll()
                    .FirstOrDefaultAsync(m => m.KanjiID == kanjiId && m.ComponentID == componentId);

                if (mapping == null)
                {
                    return false;
                }

                _unitOfWork.KanjiComponentMappings.Remove(mapping);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing component from Kanji with ID: {KanjiId}", kanjiId);
                throw;
            }
        }

        /// <summary>
        /// Update a component mapping in a Kanji
        /// </summary>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="componentId">Component ID</param>
        /// <param name="position">New position</param>
        /// <param name="userId">ID of the user updating the mapping</param>
        /// <returns>Updated mapping or null if not found</returns>
        public async Task<KanjiComponentMapping> UpdateComponentMappingAsync(int kanjiId, int componentId, string position, int userId)
        {
            try
            {
                var mapping = await _unitOfWork.KanjiComponentMappings.GetAll()
                    .FirstOrDefaultAsync(m => m.KanjiID == kanjiId && m.ComponentID == componentId);

                if (mapping == null)
                {
                    return null;
                }

                mapping.Position = position;
                mapping.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.KanjiComponentMappings.Update(mapping);
                await _unitOfWork.SaveChangesAsync();

                return mapping;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating component mapping for Kanji with ID: {KanjiId}", kanjiId);
                throw;
            }
        }

        /// <summary>
        /// Get all Kanji that contain a specific component
        /// </summary>
        /// <param name="componentId">Component ID</param>
        /// <returns>List of Kanji containing the component</returns>
        public async Task<IEnumerable<Kanji>> GetKanjiByComponentAsync(int componentId)
        {
            try
            {
                var kanji = await _unitOfWork.KanjiComponentMappings.GetAll()
                    .Where(m => m.ComponentID == componentId)
                    .Include(m => m.Kanji)
                    .ThenInclude(k => k.Meanings)
                    .Select(m => m.Kanji)
                    .ToListAsync();

                return kanji;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Kanji by component ID: {ComponentId}", componentId);
                throw;
            }
        }

        #endregion

        #region Vocabulary Management

        /// <summary>
        /// Get all vocabulary items that contain a specific Kanji
        /// </summary>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged list of vocabulary items</returns>
        public async Task<PagedResult<Vocabulary>> GetVocabularyByKanjiAsync(int kanjiId, int page = 1, int pageSize = 20)
        {
            try
            {
                // Get total count of vocabularies containing this kanji
                var totalCount = await _unitOfWork.KanjiVocabularies.GetAll()
                    .Where(kv => kv.KanjiID == kanjiId)
                    .CountAsync();

                // Get paged vocabulary items
                var vocabularyItems = await _unitOfWork.KanjiVocabularies.GetAll()
                    .Where(kv => kv.KanjiID == kanjiId)
                    .Include(kv => kv.Vocabulary)
                    .OrderBy(kv => kv.Vocabulary.JLPT) // Order by JLPT level
                    .ThenBy(kv => kv.Vocabulary.Japanese)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(kv => kv.Vocabulary)
                    .ToListAsync();

                // Create paged result
                var result = new PagedResult<Vocabulary>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = vocabularyItems
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vocabulary by Kanji ID: {KanjiId}", kanjiId);
                throw;
            }
        }

        /// <summary>
        /// Add a Kanji to a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <param name="dto">Kanji vocabulary mapping data</param>
        /// <param name="userId">ID of the user adding the Kanji</param>
        /// <returns>Created Kanji vocabulary mapping</returns>
        public async Task<KanjiVocabulary> AddKanjiToVocabularyAsync(int vocabularyId, AddKanjiToVocabularyDto dto, int userId)
        {
            try
            {
                // Check if vocabulary exists
                var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(vocabularyId);
                if (vocabulary == null)
                {
                    throw new ArgumentException($"Vocabulary with ID {vocabularyId} not found");
                }

                // Check if Kanji exists
                var kanji = await _unitOfWork.Kanjis.GetByIdAsync(dto.KanjiId);
                if (kanji == null)
                {
                    throw new ArgumentException($"Kanji with ID {dto.KanjiId} not found");
                }

                // Check if mapping already exists
                var existingMapping = await _unitOfWork.KanjiVocabularies.GetAll()
                    .FirstOrDefaultAsync(kv => kv.VocabularyID == vocabularyId && kv.KanjiID == dto.KanjiId);

                if (existingMapping != null)
                {
                    throw new InvalidOperationException($"Kanji already added to vocabulary");
                }

                // Validate position within Japanese text
                if (vocabulary.Japanese == null ||
                    dto.Position < 0 ||
                    dto.Position >= vocabulary.Japanese.Length ||
                    vocabulary.Japanese[dto.Position].ToString() != kanji.Character)
                {
                    throw new ArgumentException($"Invalid position for Kanji in vocabulary");
                }

                // Create new mapping
                var mapping = new KanjiVocabulary
                {
                    VocabularyID = vocabularyId,
                    KanjiID = dto.KanjiId,
                    Position = dto.Position,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.KanjiVocabularies.AddAsync(mapping);
                await _unitOfWork.SaveChangesAsync();

                // Load navigation properties
                mapping.Vocabulary = vocabulary;
                mapping.Kanji = kanji;

                return mapping;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Kanji to vocabulary with ID: {VocabularyId}", vocabularyId);
                throw;
            }
        }

        /// <summary>
        /// Remove a Kanji from a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="userId">ID of the user removing the Kanji</param>
        /// <returns>True if removed, false if not found</returns>
        public async Task<bool> RemoveKanjiFromVocabularyAsync(int vocabularyId, int kanjiId, int userId)
        {
            try
            {
                var mapping = await _unitOfWork.KanjiVocabularies.GetAll()
                    .FirstOrDefaultAsync(kv => kv.VocabularyID == vocabularyId && kv.KanjiID == kanjiId);

                if (mapping == null)
                {
                    return false;
                }

                _unitOfWork.KanjiVocabularies.Remove(mapping);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing Kanji from vocabulary with ID: {VocabularyId}", vocabularyId);
                throw;
            }
        }

        /// <summary>
        /// Update a Kanji position in a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="position">New position</param>
        /// <param name="userId">ID of the user updating the mapping</param>
        /// <returns>Updated mapping or null if not found</returns>
        public async Task<KanjiVocabulary> UpdateKanjiPositionInVocabularyAsync(int vocabularyId, int kanjiId, int position, int userId)
        {
            try
            {
                // Get the mapping
                var mapping = await _unitOfWork.KanjiVocabularies.GetAll()
                    .Include(kv => kv.Vocabulary)
                    .Include(kv => kv.Kanji)
                    .FirstOrDefaultAsync(kv => kv.VocabularyID == vocabularyId && kv.KanjiID == kanjiId);

                if (mapping == null)
                {
                    return null;
                }

                // Validate position within Japanese text
                if (mapping.Vocabulary.Japanese == null ||
                    position < 0 ||
                    position >= mapping.Vocabulary.Japanese.Length ||
                    mapping.Vocabulary.Japanese[position].ToString() != mapping.Kanji.Character)
                {
                    throw new ArgumentException($"Invalid position for Kanji in vocabulary");
                }

                mapping.Position = position;
                mapping.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.KanjiVocabularies.Update(mapping);
                await _unitOfWork.SaveChangesAsync();

                return mapping;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Kanji position in vocabulary with ID: {VocabularyId}", vocabularyId);
                throw;
            }
        }

        /// <summary>
        /// Automatically detect and add all Kanji in a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <param name="userId">ID of the user performing the operation</param>
        /// <returns>List of created Kanji vocabulary mappings</returns>
        public async Task<IEnumerable<KanjiVocabulary>> AutoDetectAndAddKanjiToVocabularyAsync(int vocabularyId, int userId)
        {
            try
            {
                // Get the vocabulary
                var vocabulary = await _unitOfWork.Vocabularies.GetByIdAsync(vocabularyId);
                if (vocabulary == null)
                {
                    throw new ArgumentException($"Vocabulary with ID {vocabularyId} not found");
                }

                if (string.IsNullOrEmpty(vocabulary.Japanese))
                {
                    throw new InvalidOperationException("Vocabulary has no Japanese text");
                }

                // Get all Kanji characters
                var allKanji = await _unitOfWork.Kanjis.GetAll()
                    .Select(k => new { k.KanjiID, k.Character })
                    .ToListAsync();

                // Find Kanji in vocabulary
                var newMappings = new List<KanjiVocabulary>();
                for (int i = 0; i < vocabulary.Japanese.Length; i++)
                {
                    var character = vocabulary.Japanese[i].ToString();
                    var kanji = allKanji.FirstOrDefault(k => k.Character == character);

                    if (kanji != null)
                    {
                        // Check if mapping already exists
                        var existingMapping = await _unitOfWork.KanjiVocabularies.GetAll()
                            .FirstOrDefaultAsync(kv => kv.VocabularyID == vocabularyId && kv.KanjiID == kanji.KanjiID);

                        if (existingMapping == null)
                        {
                            // Create new mapping
                            var mapping = new KanjiVocabulary
                            {
                                VocabularyID = vocabularyId,
                                KanjiID = kanji.KanjiID,
                                Position = i,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };

                            await _unitOfWork.KanjiVocabularies.AddAsync(mapping);
                            newMappings.Add(mapping);
                        }
                    }
                }

                if (newMappings.Any())
                {
                    await _unitOfWork.SaveChangesAsync();
                }

                return newMappings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error auto-detecting Kanji in vocabulary with ID: {VocabularyId}", vocabularyId);
                throw;
            }
        }

        /// <summary>
        /// Get all Kanji in a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <returns>List of Kanji with position information</returns>
        public async Task<IEnumerable<KanjiReferenceDto>> GetKanjiInVocabularyAsync(int vocabularyId)
        {
            try
            {
                var kanjiVocabularies = await _unitOfWork.KanjiVocabularies.GetAll()
                    .Where(kv => kv.VocabularyID == vocabularyId)
                    .Include(kv => kv.Kanji)
                    .ThenInclude(k => k.Meanings)
                    .OrderBy(kv => kv.Position)
                    .ToListAsync();

                var result = kanjiVocabularies.Select(kv => new KanjiReferenceDto
                {
                    KanjiID = kv.KanjiID,
                    Character = kv.Kanji.Character,
                    Onyomi = kv.Kanji.OnYomi,
                    Kunyomi = kv.Kanji.KunYomi,
                    Meaning = kv.Kanji.Meanings.FirstOrDefault()?.Meaning ?? "",
                    Position = kv.Position ?? 0
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Kanji in vocabulary with ID: {VocabularyId}", vocabularyId);
                throw;
            }
        }

        #endregion
    }
}