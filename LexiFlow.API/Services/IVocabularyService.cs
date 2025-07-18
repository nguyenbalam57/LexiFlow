namespace LexiFlow.API.Services
{
    public interface IVocabularyService
    {
        Task<PagedResult<Vocabulary>> GetVocabularyAsync(int userId, int page, int pageSize, DateTime? lastSync);
        Task<Vocabulary> GetByIdAsync(int id);
        Task<Vocabulary> CreateAsync(Vocabulary vocabulary);
        Task<Vocabulary> UpdateAsync(Vocabulary vocabulary);
        Task<bool> DeleteAsync(int id);
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
