using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Commons;
using LexiFlow.Models.Medias;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Learning.Vocabularys
{
    /// <summary>
    /// Nhóm các từ vựng liên quan
    /// </summary>
    [Index(nameof(GroupName), Name = "IX_VocabularyGroup_Name")]
    public class VocabularyGroup : BaseLearning
    {
        /// <summary>
        /// Khóa chính của nhóm từ vựng (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VocabularyGroupId { get; set; }

        /// <summary>
        /// Tên nhóm từ vựng (bắt buộc, tối đa 100 ký tự).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về nhóm từ vựng.
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// ID danh mục cha (nếu có).
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// ID của tệp phương tiện đại diện cho nhóm từ vựng (nếu có).
        /// </summary>
        public int? MediaFileId { get; set; }

        /// <summary>
        /// Thuộc tính nhóm
        /// </summary>
        [StringLength(50)]
        public string GroupType { get; set; } // Topic, Lesson, Theme, etc.

        /// <summary>
        /// Mã màu (dùng để hiển thị trực quan, tối đa 20 ký tự).
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; }

        /// <summary>
        /// Phương pháp sắp xếp từ vựng trong nhóm
        /// </summary>
        public string SortingMethod { get; set; } // Alphabetical, Difficulty, Custom

        /// <summary>
        /// Học tập và thực hành
        /// </summary>
        public bool IncludeInPractice { get; set; } = true;

        /// <summary>
        /// Bao gồm trong các bài kiểm tra
        /// </summary>
        public bool IncludeInTests { get; set; } = true;

        /// <summary>
        /// Thời gian học đề xuất (phút)
        /// </summary>
        public int? SuggestedStudyTimeMinutes { get; set; }

        /// <summary>
        /// Giới hạn truy cập
        /// </summary>
        public bool IsPublic { get; set; } = true;

        /// <summary>
        /// Vai trò được phép truy cập nhóm này (danh sách phân tách bằng dấu phẩy)
        /// Bổ sung: Cho phép chỉ định vai trò được phép truy cập nhóm từ vựng
        /// </summary>
        public string AllowedRoles { get; set; }

        // Navigation properties
        /// <summary>
        /// Danh mục cha (nếu có).
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Tệp phương tiện đại diện cho nhóm từ vựng (nếu có).
        /// </summary>
        [ForeignKey("MediaFileId")]
        public virtual MediaFile Media { get; set; }

        /// <summary>
        /// Các từ vựng thuộc nhóm này.
        /// </summary>
        public virtual ICollection<Vocabulary> Vocabularies { get; set; }

        // Cải tiến: Relationships mới
        public virtual ICollection<GroupVocabularyRelation> VocabularyRelations { get; set; }
        public virtual ICollection<Progress.LearningSession> LearningSessions { get; set; }
    }
}
