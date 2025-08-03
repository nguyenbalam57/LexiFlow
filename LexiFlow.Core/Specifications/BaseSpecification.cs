using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Specifications
{
    /// <summary>
    /// Base specification pattern implementation for defining query specifications / Triển khai mẫu đặc tả cơ sở để xác định đặc tả truy vấn
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public abstract class BaseSpecification<T>
    {
        /// <summary>
        /// Criteria for the specification
        /// </summary>
        public Expression<Func<T, bool>> Criteria { get; private set; }

        /// <summary>
        /// Includes for eager loading related entities
        /// </summary>
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// String-based include paths for eager loading related entities
        /// </summary>
        public List<string> IncludeStrings { get; } = new List<string>();

        /// <summary>
        /// Order by expression
        /// </summary>
        public Expression<Func<T, object>> OrderBy { get; private set; }

        /// <summary>
        /// Order by descending expression
        /// </summary>
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        /// <summary>
        /// Group by expression
        /// </summary>
        public Expression<Func<T, object>> GroupBy { get; private set; }

        /// <summary>
        /// Number of items to skip
        /// </summary>
        public int Skip { get; private set; }

        /// <summary>
        /// Number of items to take
        /// </summary>
        public int Take { get; private set; }

        /// <summary>
        /// Whether paging is enabled
        /// </summary>
        public bool IsPagingEnabled { get; private set; }

        /// <summary>
        /// Whether tracking is disabled
        /// </summary>
        public bool IsTrackingDisabled { get; private set; }

        /// <summary>
        /// Constructor with criteria
        /// </summary>
        /// <param name="criteria">Expression defining the filter criteria</param>
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected BaseSpecification()
        {
            Criteria = _ => true; // Default to selecting all
        }

        /// <summary>
        /// Add include for eager loading
        /// </summary>
        /// <param name="includeExpression">Expression defining the include</param>
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Add string-based include path
        /// </summary>
        /// <param name="includeString">Include path</param>
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        /// <summary>
        /// Apply paging
        /// </summary>
        /// <param name="skip">Number of items to skip</param>
        /// <param name="take">Number of items to take</param>
        protected virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        /// <summary>
        /// Apply order by
        /// </summary>
        /// <param name="orderByExpression">Order by expression</param>
        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        /// <summary>
        /// Apply order by descending
        /// </summary>
        /// <param name="orderByDescendingExpression">Order by descending expression</param>
        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        /// <summary>
        /// Apply group by
        /// </summary>
        /// <param name="groupByExpression">Group by expression</param>
        protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }

        /// <summary>
        /// Disable tracking for query performance
        /// </summary>
        protected virtual void DisableTracking()
        {
            IsTrackingDisabled = true;
        }
    }
}
