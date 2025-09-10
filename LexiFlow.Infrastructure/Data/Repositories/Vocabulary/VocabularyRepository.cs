using LexiFlow.Infrastructure.Data.Repositories.Base;
using LexiFlow.Infrastructure.Data.Repositories.Vocabulary;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LexiFlow.Infrastructure.Data.Repositories.Vocabulary
{
    /// <summary>
    /// Repository implementation cho Vocabulary
    /// </summary>
    public class VocabularyRepository : BaseRepository<Models.Learning.Vocabulary.Vocabulary>, IVocabularyRepository
    {
        public VocabularyRepository(LexiFlowContext context) : base(context)
        {
        }

        /// <summary>
        /// Override GetAll ?? ch? l?y active vocabularies
        /// </summary>
        public override IQueryable<Models.Learning.Vocabulary.Vocabulary> GetAll()
        {
            return _dbSet.Where(v => v.IsActive && !v.IsDeleted);
        }

        /// <summary>
        /// Tìm t? v?ng theo term và language code
        /// </summary>
        public async Task<Models.Learning.Vocabulary.Vocabulary?> GetByTermAsync(string term, string languageCode = "ja")
        {
            return await _dbSet
                .Include(v => v.Category)
                .Include(v => v.Definitions)
                .Include(v => v.Examples)
                .FirstOrDefaultAsync(v => v.Term == term && v.LanguageCode == languageCode && v.IsActive && !v.IsDeleted);
        }

        /// <summary>
        /// L?y t? v?ng theo category
        /// </summary>
        public async Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Include(v => v.Category)
                .Where(v => v.CategoryId == categoryId && v.IsActive && !v.IsDeleted)
                .OrderBy(v => v.Term)
                .ToListAsync();
        }

        /// <summary>
        /// L?y t? v?ng theo level
        /// </summary>
        public async Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetByLevelAsync(string level)
        {
            return await _dbSet
                .Include(v => v.Category)
                .Where(v => v.Level == level && v.IsActive && !v.IsDeleted)
                .OrderBy(v => v.Term)
                .ToListAsync();
        }

        /// <summary>
        /// Tìm ki?m t? v?ng v?i nhi?u tiêu chí
        /// </summary>
        public async Task<PagedResult<Models.Learning.Vocabulary.Vocabulary>> SearchAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? level = null,
            string? partOfSpeech = null,
            bool? isActive = true,
            int page = 1,
            int pageSize = 10)
        {
            var query = _dbSet.Include(v => v.Category).AsQueryable();

            // Apply filters
            if (isActive.HasValue)
            {
                query = query.Where(v => v.IsActive == isActive.Value);
            }

            query = query.Where(v => !v.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => 
                    v.Term.Contains(searchTerm) ||
                    (v.Reading != null && v.Reading.Contains(searchTerm)) ||
                    (v.AlternativeReadings != null && v.AlternativeReadings.Contains(searchTerm)) ||
                    (v.Definitions != null && v.Definitions.Any(d => d.Text.Contains(searchTerm))));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(v => v.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(level))
            {
                query = query.Where(v => v.Level == level);
            }

            if (!string.IsNullOrEmpty(partOfSpeech))
            {
                query = query.Where(v => v.PartOfSpeech == partOfSpeech);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(v => v.Term)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Models.Learning.Vocabulary.Vocabulary>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }

        /// <summary>
        /// L?y t? v?ng ng?u nhiên
        /// </summary>
        public async Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetRandomAsync(int count = 10, string? level = null)
        {
            var query = _dbSet.Where(v => v.IsActive && !v.IsDeleted);

            if (!string.IsNullOrEmpty(level))
            {
                query = query.Where(v => v.Level == level);
            }

            // Simple random selection using GUID (not the most efficient but works)
            return await query
                .OrderBy(v => Guid.NewGuid())
                .Take(count)
                .Include(v => v.Category)
                .ToListAsync();
        }

        /// <summary>
        /// L?y t? v?ng ph? bi?n nh?t
        /// </summary>
        public async Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetMostCommonAsync(int count = 10)
        {
            return await _dbSet
                .Where(v => v.IsActive && !v.IsDeleted)
                .OrderByDescending(v => v.FrequencyRank)
                .ThenBy(v => v.Term)
                .Take(count)
                .Include(v => v.Category)
                .ToListAsync();
        }

        /// <summary>
        /// L?y t? v?ng m?i ???c thêm
        /// </summary>
        public async Task<IEnumerable<Models.Learning.Vocabulary.Vocabulary>> GetRecentlyAddedAsync(int count = 10)
        {
            return await _dbSet
                .Where(v => v.IsActive && !v.IsDeleted)
                .OrderByDescending(v => v.CreatedAt)
                .Take(count)
                .Include(v => v.Category)
                .ToListAsync();
        }

        /// <summary>
        /// Ki?m tra t? ?ã t?n t?i ch?a
        /// </summary>
        public async Task<bool> ExistsByTermAsync(string term, string languageCode = "ja", int? excludeId = null)
        {
            var query = _dbSet.Where(v => v.Term == term && v.LanguageCode == languageCode && !v.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(v => v.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        /// <summary>
        /// L?y th?ng kê t? v?ng
        /// </summary>
        public async Task<VocabularyStatistics> GetStatisticsAsync()
        {
            var totalCount = await _dbSet.CountAsync(v => !v.IsDeleted);
            var activeCount = await _dbSet.CountAsync(v => v.IsActive && !v.IsDeleted);
            var inactiveCount = totalCount - activeCount;

            var countByLevel = await _dbSet
                .Where(v => !v.IsDeleted)
                .GroupBy(v => v.Level)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            var countByCategory = await _dbSet
                .Include(v => v.Category)
                .Where(v => !v.IsDeleted && v.Category != null)
                .GroupBy(v => v.Category!.CategoryName)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            var countByPartOfSpeech = await _dbSet
                .Where(v => !v.IsDeleted && !string.IsNullOrEmpty(v.PartOfSpeech))
                .GroupBy(v => v.PartOfSpeech)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            var lastUpdated = await _dbSet
                .Where(v => !v.IsDeleted)
                .MaxAsync(v => (DateTime?)v.UpdatedAt);

            return new VocabularyStatistics
            {
                TotalCount = totalCount,
                ActiveCount = activeCount,
                InactiveCount = inactiveCount,
                CountByLevel = countByLevel,
                CountByCategory = countByCategory,
                CountByWordType = countByPartOfSpeech,
                LastUpdated = lastUpdated
            };
        }

        /// <summary>
        /// Override GetByIdAsync ?? include related data
        /// </summary>
        public override async Task<Models.Learning.Vocabulary.Vocabulary?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(v => v.Category)
                .Include(v => v.Definitions)
                .Include(v => v.Examples)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
        }
    }
}