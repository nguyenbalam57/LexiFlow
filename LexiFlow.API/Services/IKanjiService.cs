using LexiFlow.API.Data.Repositories;
using LexiFlow.API.DTOs.Kanji;
using LexiFlow.API.DTOs.Vocabulary;
using LexiFlow.Models;

namespace LexiFlow.API.Services
{
    /// <summary>
    /// Interface for kanji management service
    /// </summary>
    public partial interface IKanjiService
    {
        #region Component Management

        /// <summary>
        /// Get all available Kanji components
        /// </summary>
        /// <returns>List of Kanji components</returns>
        Task<IEnumerable<KanjiComponent>> GetAllComponentsAsync();

        /// <summary>
        /// Get a Kanji component by ID
        /// </summary>
        /// <param name="componentId">Component ID</param>
        /// <returns>Kanji component or null if not found</returns>
        Task<KanjiComponent> GetComponentByIdAsync(int componentId);

        /// <summary>
        /// Create a new Kanji component
        /// </summary>
        /// <param name="dto">Component creation data</param>
        /// <param name="userId">ID of the user creating the component</param>
        /// <returns>Created Kanji component</returns>
        Task<KanjiComponent> CreateComponentAsync(CreateKanjiComponentDto dto, int userId);

        /// <summary>
        /// Update an existing Kanji component
        /// </summary>
        /// <param name="id">Component ID</param>
        /// <param name="dto">Component update data</param>
        /// <param name="userId">ID of the user updating the component</param>
        /// <returns>Updated Kanji component or null if not found</returns>
        Task<KanjiComponent> UpdateComponentAsync(int id, UpdateKanjiComponentDto dto, int userId);

        /// <summary>
        /// Delete a Kanji component
        /// </summary>
        /// <param name="id">Component ID</param>
        /// <param name="userId">ID of the user deleting the component</param>
        /// <returns>True if deleted, false if not found</returns>
        Task<bool> DeleteComponentAsync(int id, int userId);

        /// <summary>
        /// Add a component to a Kanji
        /// </summary>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="dto">Component mapping data</param>
        /// <param name="userId">ID of the user adding the component</param>
        /// <returns>Created component mapping</returns>
        Task<KanjiComponentMapping> AddComponentToKanjiAsync(int kanjiId, AddKanjiComponentDto dto, int userId);

        /// <summary>
        /// Remove a component from a Kanji
        /// </summary>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="componentId">Component ID</param>
        /// <param name="userId">ID of the user removing the component</param>
        /// <returns>True if removed, false if not found</returns>
        Task<bool> RemoveComponentFromKanjiAsync(int kanjiId, int componentId, int userId);

        /// <summary>
        /// Update a component mapping in a Kanji
        /// </summary>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="componentId">Component ID</param>
        /// <param name="position">New position</param>
        /// <param name="userId">ID of the user updating the mapping</param>
        /// <returns>Updated mapping or null if not found</returns>
        Task<KanjiComponentMapping> UpdateComponentMappingAsync(int kanjiId, int componentId, string position, int userId);

        /// <summary>
        /// Get all Kanji that contain a specific component
        /// </summary>
        /// <param name="componentId">Component ID</param>
        /// <returns>List of Kanji containing the component</returns>
        Task<IEnumerable<Kanji>> GetKanjiByComponentAsync(int componentId);

        #endregion

        #region Vocabulary Management

        /// <summary>
        /// Get all vocabulary items that contain a specific Kanji
        /// </summary>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged list of vocabulary items</returns>
        Task<PagedResult<Vocabulary>> GetVocabularyByKanjiAsync(int kanjiId, int page = 1, int pageSize = 20);

        /// <summary>
        /// Add a Kanji to a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <param name="dto">Kanji vocabulary mapping data</param>
        /// <param name="userId">ID of the user adding the Kanji</param>
        /// <returns>Created Kanji vocabulary mapping</returns>
        Task<KanjiVocabulary> AddKanjiToVocabularyAsync(int vocabularyId, AddKanjiToVocabularyDto dto, int userId);

        /// <summary>
        /// Remove a Kanji from a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="userId">ID of the user removing the Kanji</param>
        /// <returns>True if removed, false if not found</returns>
        Task<bool> RemoveKanjiFromVocabularyAsync(int vocabularyId, int kanjiId, int userId);

        /// <summary>
        /// Update a Kanji position in a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <param name="kanjiId">Kanji ID</param>
        /// <param name="position">New position</param>
        /// <param name="userId">ID of the user updating the mapping</param>
        /// <returns>Updated mapping or null if not found</returns>
        Task<KanjiVocabulary> UpdateKanjiPositionInVocabularyAsync(int vocabularyId, int kanjiId, int position, int userId);

        /// <summary>
        /// Automatically detect and add all Kanji in a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <param name="userId">ID of the user performing the operation</param>
        /// <returns>List of created Kanji vocabulary mappings</returns>
        Task<IEnumerable<KanjiVocabulary>> AutoDetectAndAddKanjiToVocabularyAsync(int vocabularyId, int userId);

        /// <summary>
        /// Get all Kanji in a vocabulary item
        /// </summary>
        /// <param name="vocabularyId">Vocabulary ID</param>
        /// <returns>List of Kanji with position information</returns>
        Task<IEnumerable<KanjiReferenceDto>> GetKanjiInVocabularyAsync(int vocabularyId);

        #endregion
    }
}