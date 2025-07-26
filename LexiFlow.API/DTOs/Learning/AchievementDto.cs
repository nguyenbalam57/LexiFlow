namespace LexiFlow.API.DTOs.Learning
{
    /// <summary>
    /// DTO cho thành tích học tập
    /// </summary>
    public class AchievementDto
    {
        /// <summary>
        /// ID thành tích
        /// </summary>
        public int AchievementID { get; set; }

        /// <summary>
        /// Tên thành tích
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// URL hình ảnh
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Đã đạt được
        /// </summary>
        public bool IsUnlocked { get; set; }

        /// <summary>
        /// Thời gian đạt được
        /// </summary>
        public DateTime? UnlockedAt { get; set; }

        /// <summary>
        /// Tiến độ
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Mục tiêu cần đạt
        /// </summary>
        public int Goal { get; set; }
    }
}
