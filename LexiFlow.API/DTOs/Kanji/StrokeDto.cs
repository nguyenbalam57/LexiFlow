namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO cho nét viết kanji
    /// </summary>
    public class StrokeDto
    {
        /// <summary>
        /// Thứ tự nét
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Dữ liệu đường path
        /// </summary>
        public string PathData { get; set; }

        /// <summary>
        /// Loại nét
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Hướng
        /// </summary>
        public string Direction { get; set; }
    }
}
