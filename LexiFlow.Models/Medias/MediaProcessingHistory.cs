using LexiFlow.Models.Cores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models.Medias
{
    /// <summary>
    /// Lịch sử xử lý media
    /// </summary>
    public class MediaProcessingHistory : AuditableEntity
    {
        /// <summary>
        /// Id lịch sử (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryId { get; set; }

        /// <summary>
        /// Id media mà lịch sử này thuộc về
        /// </summary>
        [Required]
        public int MediaId { get; set; }

        /// <summary>
        /// Loại thao tác trên media
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Operation { get; set; } // "Upload", "Process", "CreateVariant", "Delete"

        /// <summary>
        /// Chi tiết về thao tác
        /// </summary>
        [StringLength(255)]
        public string Details { get; set; }

        /// <summary>
        /// Kết quả của thao tác
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Thông báo lỗi nếu có
        /// </summary>
        public string ErrorMessage { get; set; }

        // Navigation properties

        /// <summary>
        /// Liên kết đến media
        /// </summary>
        [ForeignKey("MediaId")]
        public virtual MediaFile MediaFile { get; set; }
    }
}
