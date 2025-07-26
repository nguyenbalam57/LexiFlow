namespace LexiFlow.API.DTOs.TechnicalTerm
{
    /// <summary>
    /// DTO cho tìm kiếm thuật ngữ kỹ thuật
    /// </summary>
    public class TechnicalTermSearchRequestDto
    {
        /// <summary>
        /// Từ khóa tìm kiếm
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Tìm trong lĩnh vực cụ thể
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Tìm trong lĩnh vực con
        /// </summary>
        public string SubField { get; set; }

        /// <summary>
        /// Tìm trong phòng ban
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Số lượng kết quả tối đa
        /// </summary>
        public int MaxResults { get; set; } = 20;
    }
}