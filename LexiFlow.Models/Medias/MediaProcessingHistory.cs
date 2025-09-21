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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryId { get; set; }

        [Required]
        public int MediaId { get; set; }

        [Required]
        [StringLength(50)]
        public string Operation { get; set; } // "Upload", "Process", "CreateVariant", "Delete"

        [StringLength(255)]
        public string Details { get; set; }

        public bool IsSuccess { get; set; }

        [StringLength(1000)]
        public string ErrorMessage { get; set; }

        // Navigation properties
        [ForeignKey("MediaId")]
        public virtual MediaFile MediaFile { get; set; }
    }
}
