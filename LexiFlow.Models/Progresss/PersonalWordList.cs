using LexiFlow.Models.Cores;
using LexiFlow.Models.Medias;
using LexiFlow.Models.Users;
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
    /// Danh sách từ vựng cá nhân của người dùng
    /// </summary>
    [Index(nameof(UserId), nameof(ListName), IsUnique = true, Name = "IX_PersonalWordList_User_Name")]
    public class PersonalWordList : BaseEntity
    {
        /// <summary>
        /// ID danh sách từ vựng cá nhân
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListId { get; set; }

        /// <summary>
        /// Tên danh sách từ vựng cá nhân
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ListName { get; set; }

        /// <summary>
        /// Mô tả danh sách từ vựng cá nhân
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// ID người dùng sở hữu danh sách từ vựng cá nhân
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Loại danh sách từ vựng (Study, Favorite, Custom)
        /// </summary>
        [StringLength(50)]
        public string ListType { get; set; } // Study, Favorite, Custom

        /// <summary>
        /// Mã màu cho danh sách từ vựng cá nhân
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; } // Mã màu

        /// <summary>
        /// ID phương tiện liên quan đến danh sách từ vựng (nếu có)
        /// </summary>
        public int? MediaId { get; set; }

        /// <summary>
        /// Phương pháp sắp xếp danh sách từ vựng cá nhân
        /// </summary>
        public string SortingMethod { get; set; } // Custom, Alphabetical, Date

        /// <summary>
        /// Trạng thái yêu thích của danh sách từ vựng cá nhân
        /// </summary>
        public bool IsFavorite { get; set; } = false; // Danh sách yêu thích

        /// <summary>
        /// Trạng thái ẩn của danh sách từ vựng cá nhân
        /// </summary>
        public bool IncludeInReview { get; set; } = true; // Bao gồm trong ôn tập

        /// <summary>
        /// Trạng thái chia sẻ của danh sách từ vựng cá nhân
        /// </summary>
        public bool IsShared { get; set; } = false; // Chia sẻ với người khác

        /// <summary>
        /// Danh sách người dùng được chia sẻ (danh sách ID người dùng, phân tách bằng dấu phẩy)
        /// </summary>
        public string SharedWith { get; set; } // Danh sách người dùng được chia sẻ

        /// <summary>
        /// Số lượng mục trong danh sách từ vựng cá nhân
        /// </summary>
        public int? ItemCount { get; set; } // Số lượng mục

        /// <summary>
        /// Thời điểm học gần nhất
        /// </summary>
        public DateTime? LastStudied { get; set; } // Thời điểm học gần nhất

        /// <summary>
        /// Tỷ lệ thông thạo của danh sách từ vựng cá nhân
        /// </summary>
        public float? MasteryPercentage { get; set; } // Tỷ lệ thông thạo

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Phương tiện liên quan đến danh sách từ vựng (nếu có)
        /// </summary>
        [ForeignKey("MediaId")]
        public virtual MediaFile Media { get; set; }

        public virtual ICollection<PersonalWordListItem> Items { get; set; }
    }
}
