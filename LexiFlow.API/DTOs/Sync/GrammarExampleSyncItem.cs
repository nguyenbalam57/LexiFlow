using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.Sync
{
    /// <summary>
    /// Ví dụ ngữ pháp để đồng bộ
    /// </summary>
    public class GrammarExampleSyncItem
    {
        /// <summary>
        /// ID ví dụ ngữ pháp (có thể là ID tạm thời từ client nếu âm)
        /// </summary>
        public int ExampleID { get; set; }

        /// <summary>
        /// ID ngữ pháp liên quan
        /// </summary>
        public int GrammarID { get; set; }

        /// <summary>
        /// Câu ví dụ tiếng Nhật
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Japanese { get; set; }

        /// <summary>
        /// Nghĩa của câu ví dụ
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Meaning { get; set; }

        /// <summary>
        /// Thời gian tạo trên client
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật trên client
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// ID tham chiếu trên client (sử dụng cho các mục mới)
        /// </summary>
        public string ClientReferenceId { get; set; }
    }
}
