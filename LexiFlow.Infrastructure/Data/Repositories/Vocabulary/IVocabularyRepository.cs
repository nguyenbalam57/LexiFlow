using LexiFlow.Infrastructure.Data.Repositories.Base;
using LexiFlow.Models.Learning.Vocabulary;

namespace LexiFlow.Infrastructure.Data.Repositories.Vocabulary
{
    /// <summary>
    /// Interface cho Vocabulary Repository
    /// </summary>
    public interface IVocabularyRepository : IRepository<Models.Learning.Vocabulary.Vocabulary>
    {
        /// <summary>
        /// Tìm t? v?ng theo term và language code
        /// </summary>
        Task<Models.Learning.Vocabulary.Vocabulary?> GetByTermAsync(string term, string languageCode = "ja");
        
        /// <summary>
        /// L?y t? v?ng theo category
        /// </summary>
        Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetByCategoryAsync(int categoryId);
        
        /// <summary>
        /// L?y t? v?ng theo level
        /// </summary>
        Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetByLevelAsync(string level);
        
        /// <summary>
        /// Tìm ki?m t? v?ng v?i nhi?u tiêu chí
        /// </summary>
        Task<PagedResult<Models.Learning.Vocabulary.Vocabulary>> SearchAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? level = null,
            string? partOfSpeech = null,
            bool? isActive = true,
            int page = 1,
            int pageSize = 10);
        
        /// <summary>
        /// L?y t? v?ng ng?u nhiên
        /// </summary>
        Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetRandomAsync(int count = 10, string? level = null);
        
        /// <summary>
        /// L?y t? v?ng ph? bi?n nh?t
        /// </summary>
        Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetMostCommonAsync(int count = 10);
        
        /// <summary>
        /// L?y t? v?ng m?i ???c thêm
        /// </summary>
        Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetRecentlyAddedAsync(int count = 10);
        
        /// <summary>
        /// Ki?m tra t? ?ã t?n t?i ch?a
        /// </summary>
        Task<bool> ExistsByTermAsync(string term, string languageCode = "ja", int? excludeId = null);
        
        /// <summary>
        /// L?y th?ng kê t? v?ng
        /// </summary>
        Task<VocabularyStatistics> GetStatisticsAsync();
    }
    
    /// <summary>
    /// Statistics for vocabulary
    /// </summary>
    public class VocabularyStatistics
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