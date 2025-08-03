using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Specifications.Vocabulary
{
    /// <summary>
    /// Specification for filtering vocabulary items
    /// </summary>
    public class VocabularyFilterSpecification : BaseSpecification<LexiFlow.Models.Learning.Vocabulary.Vocabulary>
    {
        /// <summary>
        /// Constructor with advanced filtering options
        /// </summary>
        public VocabularyFilterSpecification(
            string searchTerm = null,
            string difficultyLevel = null,
            string category = null,
            string tags = null,
            int? categoryId = null,
            bool includeDefinitions = true,
            bool includeExamples = true,
            bool includeTranslations = true,
            bool includeMedia = false,
            bool onlyActive = true,
            int skip = 0,
            int take = 50,
            string sortBy = "Term",
            bool sortDescending = false)
            : base(BuildCriteria(searchTerm, difficultyLevel, category, tags, categoryId, onlyActive))
        {
            // Apply includes based on parameters
            if (includeDefinitions)
                AddInclude(v => v.Definitions);

            if (includeExamples)
                AddInclude(v => v.Examples);

            if (includeTranslations)
                AddInclude(v => v.Translations);

            if (includeMedia)
                AddInclude(v => v.MediaFiles);

            // Always include category for reference
            AddInclude(v => v.Category);

            // Apply sorting
            ApplySorting(sortBy, sortDescending);

            // Apply paging
            if (take > 0)
                ApplyPaging(skip, take);
        }

        /// <summary>
        /// Build filter criteria based on parameters
        /// </summary>
        private static Expression<Func<LexiFlow.Models.Learning.Vocabulary.Vocabulary, bool>> BuildCriteria(
            string searchTerm,
            string difficultyLevel,
            string category,
            string tags,
            int? categoryId,
            bool onlyActive)
        {
            Expression<Func<LexiFlow.Models.Learning.Vocabulary.Vocabulary, bool>> criteria = v =>
                (!onlyActive || !v.IsDeleted);

            // Apply search term filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();
                criteria = criteria.And(v =>
                    v.Term.ToLower().Contains(searchTerm) ||
                    (v.Reading != null && v.Reading.ToLower().Contains(searchTerm)) ||
                    (v.Translations != null && v.Translations.Any(t => t.Text.ToLower().Contains(searchTerm))) ||
                    (v.Definitions != null && v.Definitions.Any(d => d.Text.ToLower().Contains(searchTerm))));
            }

            // Apply difficulty level filter
            if (!string.IsNullOrWhiteSpace(difficultyLevel) && int.TryParse(difficultyLevel, out int level))
            {
                criteria = criteria.And(v => v.DifficultyLevel == level);
            }

            // Apply category name filter
            if (!string.IsNullOrWhiteSpace(category))
            {
                category = category.Trim().ToLower();
                criteria = criteria.And(v => v.Category != null && v.Category.CategoryName.ToLower().Contains(category));
            }

            // Apply category ID filter
            if (categoryId.HasValue)
            {
                criteria = criteria.And(v => v.CategoryId == categoryId.Value);
            }

            // Apply tags filter
            if (!string.IsNullOrWhiteSpace(tags))
            {
                var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim().ToLower())
                    .ToArray();

                criteria = criteria.And(v =>
                    !string.IsNullOrWhiteSpace(v.Tags) &&
                    tagList.Any(tag => v.Tags.ToLower().Contains(tag)));
            }

            return criteria;
        }

        /// <summary>
        /// Apply sorting based on property name
        /// </summary>
        private void ApplySorting(string sortBy, bool sortDescending)
        {
            switch (sortBy.ToLower())
            {
                case "term":
                    if (sortDescending)
                        ApplyOrderByDescending(v => v.Term);
                    else
                        ApplyOrderBy(v => v.Term);
                    break;
                case "reading":
                    if (sortDescending)
                        ApplyOrderByDescending(v => v.Reading);
                    else
                        ApplyOrderBy(v => v.Reading);
                    break;
                case "difficulty":
                    if (sortDescending)
                        ApplyOrderByDescending(v => v.DifficultyLevel);
                    else
                        ApplyOrderBy(v => v.DifficultyLevel);
                    break;
                case "frequency":
                    if (sortDescending)
                        ApplyOrderByDescending(v => v.Frequency);
                    else
                        ApplyOrderBy(v => v.Frequency);
                    break;
                case "created":
                    if (sortDescending)
                        ApplyOrderByDescending(v => v.CreatedAt);
                    else
                        ApplyOrderBy(v => v.CreatedAt);
                    break;
                case "updated":
                    if (sortDescending)
                        ApplyOrderByDescending(v => v.UpdatedAt);
                    else
                        ApplyOrderBy(v => v.UpdatedAt);
                    break;
                default:
                    // Default sorting by term
                    if (sortDescending)
                        ApplyOrderByDescending(v => v.Term);
                    else
                        ApplyOrderBy(v => v.Term);
                    break;
            }
        }
    }

    /// <summary>
    /// Extension methods for expression combination
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combine expressions with AND operator
        /// </summary>
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// Combine expressions with OR operator
        /// </summary>
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.OrElse(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
