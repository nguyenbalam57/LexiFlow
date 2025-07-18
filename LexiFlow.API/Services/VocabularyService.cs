using LexiFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.API.Services
{
    public class VocabularyService : IVocabularyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VocabularyService> _logger;

        public VocabularyService(ApplicationDbContext context, ILogger<VocabularyService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<PagedResult<Vocabulary>> GetVocabularyAsync(int userId, int page, int pageSize, DateTime? lastSync)
        {
            // Start with query for all vocabulary items created by the user or shared with everyone
            IQueryable<Vocabulary> query = _context.Vocabulary
                .Where(v => v.CreatedByUserID == userId);

            // Apply last sync filter if provided
            if (lastSync.HasValue)
            {
                query = query.Where(v => v.CreatedAt > lastSync.Value ||
                                         (v.UpdatedAt.HasValue && v.UpdatedAt.Value > lastSync.Value));
            }

            // Get total count
            var totalCount = await query.CountAsync();

            // Calculate pagination
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Apply pagination and get results
            var items = await query
                .OrderByDescending(v => v.UpdatedAt ?? v.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Vocabulary>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public async Task<Vocabulary> GetByIdAsync(int id)
        {
            return await _context.Vocabulary.FindAsync(id);
        }

        public async Task<Vocabulary> CreateAsync(Vocabulary vocabulary)
        {
            vocabulary.CreatedAt = DateTime.UtcNow;

            _context.Vocabulary.Add(vocabulary);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created vocabulary item {Id} by user {UserId}",
                vocabulary.VocabularyID, vocabulary.CreatedByUserID);

            return vocabulary;
        }

        public async Task<Vocabulary> UpdateAsync(Vocabulary vocabulary)
        {
            vocabulary.UpdatedAt = DateTime.UtcNow;

            _context.Entry(vocabulary).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated vocabulary item {Id} by user {UserId}",
                vocabulary.VocabularyID, vocabulary.UpdatedByUserID);

            return vocabulary;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vocabulary = await _context.Vocabulary.FindAsync(id);

            if (vocabulary == null)
            {
                _logger.LogWarning("Delete vocabulary failed: Item {Id} not found", id);
                return false;
            }

            _context.Vocabulary.Remove(vocabulary);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted vocabulary item {Id}", id);

            return true;
        }
    }
}
