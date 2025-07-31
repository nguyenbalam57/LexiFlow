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

namespace LexiFlow.Models.Progress
{
    /// <summary>
    /// Mục trong danh sách từ vựng cá nhân
    /// </summary>
    [Index(nameof(ListId), nameof(VocabularyId), IsUnique = true, Name = "IX_PersonalWordListItem_List_Vocab")]
    public class PersonalWordListItem : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        [Required]
        public int ListId { get; set; }

        [Required]
        public int VocabularyId { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Cải tiến: Vị trí và trạng thái
        public int DisplayOrder { get; set; } = 0; // Thứ tự hiển thị

        public bool IsFavorite { get; set; } = false; // Mục yêu thích

        // Cải tiến: Thông tin cá nhân
        [StringLength(255)]
        public string PersonalNote { get; set; } // Ghi chú cá nhân

        [StringLength(255)]
        public string PersonalExample { get; set; } // Ví dụ cá nhân

        // Cải tiến: Thống kê học tập
        public int? StudyCount { get; set; } = 0; // Số lần học

        public int? CorrectCount { get; set; } = 0; // Số lần đúng

        public DateTime? LastStudied { get; set; } // Thời điểm học gần nhất

        public int? MasteryLevel { get; set; } = 0; // Mức độ thành thạo

        // Cải tiến: Tags và phân loại
        [StringLength(255)]
        public string PersonalTags { get; set; } // Tags cá nhân

        [StringLength(50)]
        public string Difficulty { get; set; } // Mức độ khó cá nhân

        // Navigation properties
        [ForeignKey("ListId")]
        public virtual PersonalWordList List { get; set; }

        [ForeignKey("VocabularyId")]
        public virtual Learning.Vocabulary.Vocabulary Vocabulary { get; set; }
    }
}
