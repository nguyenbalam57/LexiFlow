namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho phản hồi API khi lấy danh sách nhóm từ vựng
    /// </summary>
    public class VocabularyGroupListDto
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
        /// Trạng thái hoạt động của nhóm
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Số lượng từ vựng trong nhóm
        /// </summary>
        public int VocabularyCount { get; set; }
    }
}
