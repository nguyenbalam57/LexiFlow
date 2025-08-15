using LexiFlow.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Media
{
    /// <summary>
    /// Lịch sử xử lý media
    /// </summary>
    public class MediaProcessingHistory : BaseEntity
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

        public int? ProcessedBy { get; set; }

        // Navigation properties
        [ForeignKey("MediaId")]
        public virtual MediaFile MediaFile { get; set; }

        [ForeignKey("ProcessedBy")]
        public virtual User.User ProcessedByUser { get; set; }
    }
}
