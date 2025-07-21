using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models
{
    /// <summary>
    /// Lớp kết quả phân trang
    /// </summary>
    public class PagedResult<T>
    {
        /// <summary>
        /// Danh sách các phần tử
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Số phần tử trên một trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số phần tử
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Có trang trước không
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Có trang sau không
        /// </summary>
        public bool HasNextPage => Page < TotalPages;
    }
}
