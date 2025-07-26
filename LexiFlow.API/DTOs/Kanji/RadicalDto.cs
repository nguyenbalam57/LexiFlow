namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO cho bộ thủ kanji
    /// </summary>
    public class RadicalDto
    {
        /// <summary>
        /// Ký tự bộ thủ
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// Tên bộ thủ
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Nghĩa bộ thủ
        /// </summary>
        public string Meaning { get; set; }

        /// <summary>
        /// Số nét
        /// </summary>
        public int StrokeCount { get; set; }

        /// <summary>
        /// Số lượng kanji sử dụng bộ thủ này
        /// </summary>
        public int KanjiCount { get; set; }
    }
}
