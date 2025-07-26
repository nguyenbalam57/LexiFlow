using LexiFlow.API.DTOs.Kanji;
using LexiFlow.Models;

namespace LexiFlow.API.Services
{
    public interface IKanjiService
    {
        Task<IEnumerable<Kanji>> GetAllAsync(string jlptLevel = null, int? grade = null);

        Task<Kanji> GetByIdAsync(int id);

        Task<Kanji> GetByCharacterAsync(string character);

        Task<Kanji> CreateAsync(CreateKanjiDto dto, int userId);

        Task<Kanji> UpdateAsync(int id, UpdateKanjiDto dto, int userId);

        Task<bool> DeleteAsync(int id, int userId);

        Task<IEnumerable<Kanji>> SearchAsync(KanjiSearchRequestDto searchRequest);

        Task<UserKanjiProgress> GetUserProgressAsync(int userId, int kanjiId);

        Task<IEnumerable<UserKanjiProgress>> GetUserProgressListAsync(int userId, string jlptLevel = null, int? grade = null);

        Task<UserKanjiProgress> UpdateProgressAsync(int userId, UpdateKanjiProgressDto dto);

        Task<IEnumerable<Kanji>> GetReviewListAsync(int userId, int count = 20, bool includeNew = true);

        Task<IEnumerable<KanjiComponentDto>> GetComponentsAsync(int kanjiId);

        Task<IEnumerable<VocabularyReferenceDto>> GetRelatedVocabularyAsync(int kanjiId);
    }
}
