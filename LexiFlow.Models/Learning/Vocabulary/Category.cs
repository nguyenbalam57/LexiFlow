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

namespace LexiFlow.Models.Learning.Vocabulary
{
    /// <summary>
    /// Danh mục từ vựng, tổ chức theo cấu trúc cây
    /// </summary>
    [Index(nameof(CategoryName), Name = "IX_Category_Name")]
    public class Category : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Level { get; set; }

        public int? DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public int? ParentCategoryId { get; set; }

        // Cải tiến: Thuộc tính hiển thị
        [StringLength(255)]
        public string IconPath { get; set; }

        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Thông tin phân loại
        [StringLength(50)]
        public string CategoryType { get; set; } // Grammar, Vocabulary, Kanji, etc.

        [StringLength(100)]
        public string Tags { get; set; }

        // Cải tiến: Giới hạn truy cập
        public bool IsRestricted { get; set; } = false;
        public string AllowedRoles { get; set; }
        public string AllowedUserIds { get; set; }

        // Cải tiến: Thống kê
        public int? ItemCount { get; set; } = 0;
        public DateTime? LastUpdatedContent { get; set; }

        // Navigation properties
        [ForeignKey("ParentCategoryId")]
        public virtual Category ParentCategory { get; set; }

        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<Vocabulary> Vocabularies { get; set; }
        public virtual ICollection<VocabularyGroup> VocabularyGroups { get; set; }
        public virtual ICollection<Practice.TestResult> TestResults { get; set; }
    }
}
