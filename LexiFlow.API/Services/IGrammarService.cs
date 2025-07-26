using LexiFlow.API.DTOs.Grammar;
using LexiFlow.Models;

namespace LexiFlow.API.Services
{
    public interface IGrammarService
    {
        Task<IEnumerable<Grammar>> GetAllAsync(string jlptLevel = null, int? categoryId = null);

        Task<Grammar> GetByIdAsync(int id);

        Task<Grammar> CreateAsync(CreateGrammarDto dto, int userId);

        Task<Grammar> UpdateAsync(int id, UpdateGrammarDto dto, int userId);

        Task<bool> DeleteAsync(int id, int userId);

        Task<IEnumerable<Grammar>> SearchAsync(GrammarSearchRequestDto searchRequest);

        Task<GrammarExample> AddExampleAsync(int grammarId, CreateGrammarExampleDto dto, int userId);

        Task<bool> DeleteExampleAsync(int exampleId, int userId);

        Task<UserGrammarProgress> GetUserProgressAsync(int userId, int grammarId);

        Task<IEnumerable<UserGrammarProgress>> GetUserProgressListAsync(int userId, string jlptLevel = null);

        Task<UserGrammarProgress> UpdateProgressAsync(int userId, UpdateGrammarProgressDto dto);

        Task<IEnumerable<Grammar>> GetReviewListAsync(int userId, int count = 20, bool includeNew = true);
    }
}
