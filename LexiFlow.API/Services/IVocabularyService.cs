using LexiFlow.API.DTOs;
using LexiFlow.API.DTOs.Vocabulary;
using LexiFlow.Models;

namespace LexiFlow.API.Services
{
    public interface IVocabularyService
    {
        Task<(IEnumerable<Vocabulary> Items, int TotalCount, int Page, int PageSize, int TotalPages)> GetVocabularyAsync(
            int userId, int page = 1, int pageSize = 50, DateTime? lastSync = null);

        Task<Vocabulary?> GetByIdAsync(int id);

        Task<Vocabulary?> CreateAsync(CreateVocabularyDto dto, int userId);

        Task<Vocabulary?> UpdateAsync(int id, UpdateVocabularyDto dto, int userId);

        Task<bool> DeleteAsync(int id, int userId);

        Task<IEnumerable<Vocabulary>> SearchAsync(string query, string languageCode = "all", int maxResults = 20);
    }
}
