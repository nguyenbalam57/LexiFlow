namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho phản hồi API khi lấy thông tin nhóm từ vựng chi tiết
    /// </summary>
    public class VocabularyGroupDetailDto
    {
        /// <summary>
        /// ID của nhóm từ vựng
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// Tên nhóm từ vựng
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả về nhóm từ vựng
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ID của danh mục mà nhóm thuộc về
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// Tên của danh mục mà nhóm thuộc về
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// ID của người dùng đã tạo nhóm
        /// </summary>
        public int? CreatedByUserID { get; set; }

        /// <summary>
        /// Tên của người dùng đã tạo nhóm
        /// </summary>
        public string? CreatedByUserName { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của nhóm
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Số lượng từ vựng trong nhóm
        /// </summary>
        public int VocabularyCount { get; set; }

        /// <summary>
        /// Thời gian tạo nhóm
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật nhóm
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Chuỗi phiên bản hàng (dùng cho kiểm soát đồng thời)
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;
    }
}
