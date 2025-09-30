using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Vocabularys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Progresss
{
    /// <summary>
    /// Mục trong danh sách từ vựng cá nhân
    /// </summary>
    [Index(nameof(ListId), nameof(VocabularyId), IsUnique = true, Name = "IX_PersonalWordListItem_List_Vocab")]
    public class PersonalWordListItem : BaseEntity
    {
        /// <summary>
        /// ID mục trong danh sách từ vựng cá nhân
        /// </summary>
        [Key]   
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        /// <summary>
        /// ID danh sách từ vựng cá nhân
        /// </summary>
        [Required]
        public int ListId { get; set; }

        /// <summary>
        /// ID từ vựng
        /// </summary>
        [Required]
        public int VocabularyId { get; set; }

        /// <summary>
        /// Ngày thêm từ vào danh sách
        /// </summary>
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Mục yêu thích
        /// </summary>
        public bool IsFavorite { get; set; } = false; // Mục yêu thích

        /// <summary>
        /// Ghi chú cá nhân về từ
        /// </summary>
        [StringLength(255)]
        public string PersonalNote { get; set; } // Ghi chú cá nhân

        /// <summary>
        /// Ví dụ cá nhân cho từ
        /// </summary>
        [StringLength(255)]
        public string PersonalExample { get; set; } // Ví dụ cá nhân

        /// <summary>
        /// 
        /// </summary>
        public int? StudyCount { get; set; } = 0; // Số lần học

        /// <summary>
        /// Số lần học đúng
        /// </summary>
        public int? CorrectCount { get; set; } = 0; // Số lần đúng

        /// <summary>
        /// Thời điểm học gần nhất
        /// </summary>
        public DateTime? LastStudied { get; set; } // Thời điểm học gần nhất

        /// <summary>
        /// Mức độ thành thạo cá nhân về từ
        /// </summary>
        public int? MasteryLevel { get; set; } = 0; // Mức độ thành thạo

        /// <summary>
        /// Tags cá nhân cho từ
        /// </summary>
        [StringLength(255)]
        public string PersonalTags { get; set; } // Tags cá nhân

        /// <summary>
        /// Mức độ khó cá nhân của từ
        /// </summary>
        [StringLength(50)]
        public string Difficulty { get; set; } // Mức độ khó cá nhân

        // Navigation properties

        /// <summary>
        /// Danh sách từ vựng cá nhân chứa mục này
        /// </summary>
        [ForeignKey("ListId")]
        public virtual PersonalWordList List { get; set; }

        /// <summary>
        /// Từ vựng liên quan đến mục này
        /// </summary>
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }
    }
}
