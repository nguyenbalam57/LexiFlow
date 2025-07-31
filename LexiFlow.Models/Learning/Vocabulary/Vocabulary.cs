using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using LexiFlow.Models.Core;
using LexiFlow.Models.Media;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.Models.Learning.Vocabulary
{
    /// <summary>
    /// Mô hình từ vựng đã tối ưu
    /// </summary>
    [Index(nameof(Term), nameof(LanguageCode), Name = "IX_Vocabulary_Term_Lang")]
    [Index(nameof(CategoryId), Name = "IX_Vocabulary_Category")]
    public class Vocabulary : AuditableEntity, ISoftDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Term { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";

        [StringLength(200)]
        public string Reading { get; set; }

        public int? CategoryId { get; set; }

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        // Cải tiến: Thuộc tính thêm
        [Range(1, 10)]
        public int Frequency { get; set; } = 5; // Tần suất sử dụng

        public string IpaNotation { get; set; } // Ký hiệu phiên âm quốc tế

        public bool IsCommon { get; set; } = false; // Có phải từ thông dụng không

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        public int? DeletedBy { get; set; }

        // Cải tiến: Lưu trữ JSON
        public string Tags { get; set; }

        // Cải tiến: Tìm kiếm toàn văn
        public string SearchVector { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Active";

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual User.User DeletedByUser { get; set; }

        public virtual ICollection<Definition> Definitions { get; set; }
        public virtual ICollection<Example> Examples { get; set; }
        public virtual ICollection<Translation> Translations { get; set; }
        public virtual ICollection<MediaFile> MediaFiles { get; set; }
        public virtual ICollection<LearningProgress> LearningProgresses { get; set; }

        // Cải tiến: Properties không lưu DB
        [NotMapped]
        public List<string> TagList
        {
            get => string.IsNullOrEmpty(Tags)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(Tags);
            set => Tags = JsonSerializer.Serialize(value);
        }
    }
}
