using LexiFlow.API.DTOs.Vocabulary;
using LexiFlow.API.DTOs.Common;

namespace LexiFlow.API.Services.Vocabulary
{
    /// <summary>
    /// Interface cho Vocabulary Service
    /// </summary>
    public interface IVocabularyService
    {
        /// <summary>
        /// L?y danh sách t? v?ng có phân trang
        /// </summary>
        Task<PaginatedResultDto<VocabularyDto>> GetVocabulariesAsync(
            int page = 1, 
            int pageSize = 10, 
            string? searchTerm = null,
            int? categoryId = null,
            string? level = null,
            string? partOfSpeech = null,
            bool? isActive = true);

        /// <summary>
        /// L?y chi ti?t t? v?ng theo ID
        /// </summary>
        Task<VocabularyDto?> GetVocabularyByIdAsync(int id);

        /// <summary>
        /// L?y t? v?ng theo term
        /// </summary>
        Task<VocabularyDto?> GetVocabularyByTermAsync(string term, string languageCode = "ja");

        /// <summary>
        /// T?o t? v?ng m?i
        /// </summary>
        Task<VocabularyDto> CreateVocabularyAsync(CreateVocabularyDto createDto, int createdBy);

        /// <summary>
        /// C?p nh?t t? v?ng
        /// </summary>
        Task<VocabularyDto?> UpdateVocabularyAsync(int id, UpdateVocabularyDto updateDto, int modifiedBy);

        /// <summary>
        /// Xóa t? v?ng (soft delete)
        /// </summary>
        Task<bool> DeleteVocabularyAsync(int id, int deletedBy);

        /// <summary>
        /// L?y t? v?ng theo category
        /// </summary>
        Task<IEnumerable<VocabularyDto>> GetVocabulariesByCategoryAsync(int categoryId);

        /// <summary>
        /// L?y t? v?ng theo level
        /// </summary>
        Task<IEnumerable<VocabularyDto>> GetVocabulariesByLevelAsync(string level);

        /// <summary>
        /// L?y t? v?ng ng?u nhiên
        /// </summary>
        Task<IEnumerable<VocabularyDto>> GetRandomVocabulariesAsync(int count = 10, string? level = null);

        /// <summary>
        /// L?y t? v?ng ph? bi?n nh?t
        /// </summary>
        Task<IEnumerable<VocabularyDto>> GetMostCommonVocabulariesAsync(int count = 10);

        /// <summary>
        /// L?y t? v?ng m?i nh?t
        /// </summary>
        Task<IEnumerable<VocabularyDto>> GetRecentVocabulariesAsync(int count = 10);

        /// <summary>
        /// Ki?m tra t? v?ng ?ã t?n t?i
        /// </summary>
        Task<bool> VocabularyExistsAsync(string term, string languageCode = "ja", int? excludeId = null);

        /// <summary>
        /// L?y th?ng kê t? v?ng
        /// </summary>
        Task<VocabularyStatisticsDto> GetVocabularyStatisticsAsync();
    }

    /// <summary>
    /// DTO cho th?ng kê t? v?ng
    /// </summary>
    public class VocabularyStatisticsDto
    {
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public Dictionary<string, int> CountByLevel { get; set; } = new();
        public Dictionary<string, int> CountByCategory { get; set; } = new();
        public Dictionary<string, int> CountByWordType { get; set; } = new();
        public DateTime? LastUpdated { get; set; }
    }
}