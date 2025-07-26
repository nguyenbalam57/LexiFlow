namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// DTO cho chuỗi thứ tự viết kanji
    /// </summary>
    public class KanjiStrokeOrderDto
    {
        /// <summary>
        /// ID kanji
        /// </summary>
        public int KanjiID { get; set; }

        /// <summary>
        /// Ký tự kanji
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// URL ảnh thứ tự viết
        /// </summary>
        public string StrokeOrderImageUrl { get; set; }

        /// <summary>
        /// Dữ liệu SVG thứ tự viết
        /// </summary>
        public string StrokeOrderSvgData { get; set; }

        /// <summary>
        /// Thứ tự các nét
        /// </summary>
        public List<StrokeDto> Strokes { get; set; } = new List<StrokeDto>();
    }

}
