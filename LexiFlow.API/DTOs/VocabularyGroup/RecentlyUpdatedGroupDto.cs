namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho nhóm từ vựng mới cập nhật
    /// </summary>
    public class RecentlyUpdatedGroupDto
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
        /// Thời gian cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
