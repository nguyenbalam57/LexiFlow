using LexiFlow.API.DTOs.Category;

namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho thống kê về nhóm từ vựng
    /// </summary>
    public class VocabularyGroupStatsDto
    {
        /// <summary>
        /// Tổng số nhóm từ vựng
        /// </summary>
        public int TotalGroups { get; set; }

        /// <summary>
        /// Số nhóm từ vựng đang hoạt động
        /// </summary>
        public int ActiveGroups { get; set; }

        /// <summary>
        /// Thống kê số nhóm theo danh mục
        /// </summary>
        public List<CategoryStatsDto> GroupsByCategory { get; set; } = new List<CategoryStatsDto>();

        /// <summary>
        /// Top nhóm từ vựng theo số lượng từ
        /// </summary>
        public List<GroupVocabularyCountDto> TopGroupsByVocabularyCount { get; set; } = new List<GroupVocabularyCountDto>();

        /// <summary>
        /// Các nhóm mới cập nhật gần đây
        /// </summary>
        public List<RecentlyUpdatedGroupDto> RecentlyUpdatedGroups { get; set; } = new List<RecentlyUpdatedGroupDto>();
    }
}
