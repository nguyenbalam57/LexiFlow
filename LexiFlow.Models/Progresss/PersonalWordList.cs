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
    /// Danh sách từ vựng cá nhân của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(ListName), IsUnique = true, Name = "IX_PersonalWordList_User_Name")]
    public class PersonalWordList : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListId { get; set; }

        [Required]
        [StringLength(100)]
        public string ListName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }

        // Cải tiến: Phân loại và hiển thị
        [StringLength(50)]
        public string ListType { get; set; } // Study, Favorite, Custom

        [StringLength(20)]
        public string ColorCode { get; set; } // Mã màu

        [StringLength(255)]
        public string IconPath { get; set; } // Icon

        // Cải tiến: Sắp xếp và hiển thị
        public int DisplayOrder { get; set; } = 0; // Thứ tự hiển thị

        public string SortingMethod { get; set; } // Custom, Alphabetical, Date

        // Cải tiến: Học tập và chia sẻ
        public bool IsFavorite { get; set; } = false; // Danh sách yêu thích

        public bool IncludeInReview { get; set; } = true; // Bao gồm trong ôn tập

        public bool IsShared { get; set; } = false; // Chia sẻ với người khác

        public string SharedWith { get; set; } // Danh sách người dùng được chia sẻ

        // Cải tiến: Thống kê
        public int? ItemCount { get; set; } // Số lượng mục

        public DateTime? LastStudied { get; set; } // Thời điểm học gần nhất

        public float? MasteryPercentage { get; set; } // Tỷ lệ thông thạo

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        public virtual ICollection<PersonalWordListItem> Items { get; set; }
    }
}
