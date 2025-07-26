namespace LexiFlow.API.DTOs.Common
{
    /// <summary>
    /// DTO cho tùy chọn select
    /// </summary>
    public class SelectOptionDto
    {
        /// <summary>
        /// Giá trị
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Nhãn hiển thị
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Nhóm (nếu có)
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// Bị vô hiệu hóa
        /// </summary>
        public bool Disabled { get; set; }
    }
}
