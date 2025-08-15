using LexiFlow.Models.Learning.Vocabulary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LexiFlow.AdminDashboard.Services
{
    /// <summary>
    /// Interface for Vocabulary Management Service
    /// </summary>
    public interface IVocabularyManagementService
    {
        // Vocabulary CRUD Operations - Using int id to match Vocabulary.Id
        Task<List<Vocabulary>> GetVocabulariesAsync(int page = 1, int pageSize = 50, string searchTerm = "");
        Task<Vocabulary> GetVocabularyByIdAsync(int id);  // Changed from vocabularyId to id
        Task<Vocabulary> CreateVocabularyAsync(CreateVocabularyRequest request);
        Task<Vocabulary> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request);  // Changed parameter name
        Task<bool> DeleteVocabularyAsync(int id, bool softDelete = true);  // Changed parameter name
        Task<List<Vocabulary>> GetVocabulariesByCategoryAsync(int categoryId);

        // Category Management
        Task<List<Category>> GetCategoriesAsync();
        Task<Category> CreateCategoryAsync(CreateCategoryRequest request);
        Task<Category> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest request);
        Task<bool> DeleteCategoryAsync(int categoryId, bool softDelete = true);

        // Search and Filter
        Task<List<Vocabulary>> SearchVocabulariesAsync(VocabularySearchFilter filter);
        Task<int> GetVocabularyCountAsync(string searchTerm = "");

        // Bulk Operations - Using List<int> to match Vocabulary.Id
        Task<List<Vocabulary>> ImportVocabulariesAsync(List<CreateVocabularyRequest> vocabularies);
        Task<byte[]> ExportVocabulariesAsync(List<int> vocabularyIds = null);
        Task<bool> BulkUpdateCategoryAsync(List<int> vocabularyIds, int categoryId);
        Task<bool> BulkDeleteAsync(List<int> vocabularyIds, bool softDelete = true);

        // Advanced Features
        Task<List<Vocabulary>> GetRandomVocabulariesAsync(int count, string level = null);
        Task<List<Vocabulary>> GetRecentVocabulariesAsync(int count = 10);
        Task<VocabularyStatistics> GetVocabularyStatisticsAsync();
        Task<List<Vocabulary>> GetRelatedVocabulariesAsync(int vocabularyId);

        // Audio Management
        Task<bool> UploadAudioFileAsync(int vocabularyId, byte[] audioData, string fileName);
        Task<byte[]> GetAudioFileAsync(int vocabularyId);
        Task<bool> DeleteAudioFileAsync(int vocabularyId);
    }

    /// <summary>
    /// Request models for Vocabulary Management
    /// </summary>
    public class CreateVocabularyRequest
    {
        public string Word { get; set; }  // Maps to Term
        public string Hiragana { get; set; }  // Maps to Reading
        public string Katakana { get; set; }  // Maps to AlternativeReadings
        public string Romaji { get; set; }    // Maps to MetadataJson
        public string Meaning { get; set; }   // Maps to Translation.Text
        public string PartOfSpeech { get; set; }  // Maps directly
        public string JLPTLevel { get; set; }  // Maps to Level
        public int? CategoryId { get; set; }   // Maps directly
        public string ExampleSentence { get; set; }  // Maps to MetadataJson
        public string ExampleMeaning { get; set; }   // Maps to MetadataJson
        public string Notes { get; set; }      // Maps to MetadataJson
        public List<string> Tags { get; set; } = new List<string>(); // Maps to Tags (JSON)
        public int DifficultyLevel { get; set; } = 1; // Maps directly
        public bool IsActive { get; set; } = true;    // Maps to Status
    }

    public class UpdateVocabularyRequest
    {
        public string Word { get; set; } = string.Empty;  // Maps to Term
        public string? Hiragana { get; set; }  // Maps to Reading
        public string? Katakana { get; set; }  // Maps to AlternativeReadings
        public string? Romaji { get; set; }    // Maps to MetadataJson
        public string Meaning { get; set; } = string.Empty;   // Maps to Translation.Text
        public string? PartOfSpeech { get; set; }  // Maps directly
        public string JLPTLevel { get; set; } = "N5";  // Maps to Level
        public int? CategoryId { get; set; }   // Maps directly
        public string? ExampleSentence { get; set; }  // Maps to MetadataJson
        public string? ExampleMeaning { get; set; }   // Maps to MetadataJson
        public string? Notes { get; set; } = string.Empty;      // Maps to MetadataJson
        public List<string> Tags { get; set; } = new List<string>(); // Maps to Tags (JSON)
        public int DifficultyLevel { get; set; } = 1; // Maps directly
        public bool IsActive { get; set; } = true;    // Maps to Status
    }

    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Level { get; set; } = "General";
        public string CategoryType { get; set; } = "Vocabulary";
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateCategoryRequest : CreateCategoryRequest
    {
        // Inherits all properties from CreateCategoryRequest
    }

    public class VocabularySearchFilter
    {
        public string? SearchTerm { get; set; }
        public List<int>? CategoryIds { get; set; }
        public List<string>? JLPTLevels { get; set; }
        public List<string>? PartsOfSpeech { get; set; }
        public int? MinDifficulty { get; set; }
        public int? MaxDifficulty { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    public class VocabularyStatistics
    {
        public int TotalVocabularies { get; set; }
        public int ActiveVocabularies { get; set; }
        public int InactiveVocabularies { get; set; }
        public int VocabulariesWithAudio { get; set; }
        public int VocabulariesCreatedThisMonth { get; set; }
        public Dictionary<string, int> VocabulariesByJLPTLevel { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> VocabulariesByCategory { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> VocabulariesByPartOfSpeech { get; set; } = new Dictionary<string, int>();
        public Dictionary<int, int> VocabulariesByDifficulty { get; set; } = new Dictionary<int, int>();
    }
}