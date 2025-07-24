using LexiFlow.Models;

namespace LexiFlow.Core.Models.Responses
{
    /// <summary>
    /// Phản hồi danh sách từ vựng
    /// </summary>
    public class VocabularyListResponse
    {
        /// <summary>
        /// Trạng thái thành công
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Danh sách từ vựng
        /// </summary>
        public List<Vocabulary> Data { get; set; }

        /// <summary>
        /// Thông tin phân trang
        /// </summary>
        public Pagination Pagination { get; set; }

        /// <summary>
        /// Thời gian đồng bộ
        /// </summary>
        public System.DateTime LastSync { get; set; }
    }

    /// <summary>
    /// Thông tin phân trang
    /// </summary>
    public class Pagination
    {
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
    }
}
