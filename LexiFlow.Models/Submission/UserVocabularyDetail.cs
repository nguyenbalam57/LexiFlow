using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Submission
{
    /// <summary>
    /// Chi tiết từ vựng trong bài nộp
    /// </summary>
    [Index(nameof(SubmissionId), nameof(VocabularyId), Name = "IX_UserVocabularyDetail_Submission_Vocab")]
    public class UserVocabularyDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserVocabularyDetaillId { get; set; }

        [Required]
        public int SubmissionId { get; set; }

        public int? VocabularyId { get; set; }

        [StringLength(100)]
        public string Word { get; set; }

        [StringLength(100)]
        public string Reading { get; set; }

        public string Meaning { get; set; }

        [StringLength(500)]
        public string Example { get; set; }

        // Cải tiến: Thông tin từ vựng
        [StringLength(255)]
        public string Translation { get; set; } // Bản dịch

        [StringLength(10)]
        public string LanguageCode { get; set; } = "vi"; // Ngôn ngữ bản dịch

        [StringLength(50)]
        public string WordType { get; set; } // Loại từ: Noun, Verb, Adjective

        [StringLength(20)]
        public string JLPTLevel { get; set; } // Cấp độ JLPT

        // Cải tiến: Đánh giá và phản hồi
        [StringLength(50)]
        public string ReviewStatus { get; set; } // Approved, Rejected, Pending

        public string ReviewComments { get; set; } // Nhận xét đánh giá

        [Range(1, 5)]
        public int? Accuracy { get; set; } // Độ chính xác (1-5)

        // Cải tiến: Liên kết và tham khảo
        public string Sources { get; set; } // Nguồn tham khảo

        public string RelatedWords { get; set; } // Từ liên quan

        // Cải tiến: Thông tin bổ sung
        public string UserNotes { get; set; } // Ghi chú của người dùng

        public string Tags { get; set; } // Thẻ

        // Cải tiến: Media và đính kèm
        [StringLength(255)]
        public string ImageUrl { get; set; } // Hình ảnh

        [StringLength(255)]
        public string AudioUrl { get; set; } // Âm thanh

        // Navigation properties
        [ForeignKey("SubmissionId")]
        public virtual UserVocabularySubmission Submission { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Learning.Vocabulary.Vocabulary Vocabulary { get; set; }

        public virtual ICollection<Media.MediaFile> MediaFiles { get; set; }
    }
}
