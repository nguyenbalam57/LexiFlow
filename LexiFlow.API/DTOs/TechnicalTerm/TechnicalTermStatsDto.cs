using System.Collections.Generic;

namespace LexiFlow.API.DTOs.TechnicalTerm
{
    /// <summary>
    /// DTO cho thống kê thuật ngữ kỹ thuật
    /// </summary>
    public class TechnicalTermStatsDto
    {
        /// <summary>
        /// Tổng số thuật ngữ
        /// </summary>
        public int TotalTerms { get; set; }

        /// <summary>
        /// Số thuật ngữ theo lĩnh vực
        /// </summary>
        public Dictionary<string, int> TermsByField { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Số thuật ngữ theo phòng ban
        /// </summary>
        public Dictionary<string, int> TermsByDepartment { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Thuật ngữ được sử dụng nhiều nhất
        /// </summary>
        public List<MostUsedTermDto> MostUsedTerms { get; set; } = new List<MostUsedTermDto>();
    }

    /// <summary>
    /// DTO cho thuật ngữ được sử dụng nhiều nhất
    /// </summary>
    public class MostUsedTermDto
    {
        /// <summary>
        /// ID thuật ngữ
        /// </summary>
        public int TermID { get; set; }

        /// <summary>
        /// Thuật ngữ tiếng Nhật
        /// </summary>
        public string Japanese { get; set; }

        /// <summary>
        /// Nghĩa tiếng Việt
        /// </summary>
        public string Vietnamese { get; set; }

        /// <summary>
        /// Tần suất sử dụng
        /// </summary>
        public int UsageCount { get; set; }
    }
}