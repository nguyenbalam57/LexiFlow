using LexiFlow.API.DTOs.WordList;
using LexiFlow.Models;

namespace LexiFlow.API.Services
{
    public interface IPersonalWordListService
    {
        Task<IEnumerable<PersonalWordList>> GetAllAsync(int userId);

        Task<PersonalWordList> GetByIdAsync(int listId, int userId);

        Task<PersonalWordList> CreateAsync(CreatePersonalWordListDto dto, int userId);

        Task<PersonalWordList> UpdateAsync(int listId, UpdatePersonalWordListDto dto, int userId);

        Task<bool> DeleteAsync(int listId, int userId);

        Task<IEnumerable<PersonalWordListItem>> GetListItemsAsync(int listId, int userId);

        Task<PersonalWordListItem> AddItemAsync(int listId, AddWordToListDto dto, int userId);

        Task<IEnumerable<PersonalWordListItem>> AddItemsAsync(int listId, AddMultipleWordsToListDto dto, int userId);

        Task<bool> RemoveItemAsync(int listId, int itemId, int userId);

        Task<bool> RemoveItemByVocabularyIdAsync(int listId, int vocabularyId, int userId);

        Task<bool> ClearListAsync(int listId, int userId);

        Task<bool> IsVocabularyInListAsync(int listId, int vocabularyId, int userId);

        Task<IEnumerable<PersonalWordList>> GetListsContainingVocabularyAsync(int vocabularyId, int userId);
    }
}
