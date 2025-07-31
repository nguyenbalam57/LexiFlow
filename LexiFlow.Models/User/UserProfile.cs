using LexiFlow.Models.Core;
using LexiFlow.Models.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.User
{
    /// <summary>
    /// Thông tin profile người dùng
    /// </summary>
    public class UserProfile : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [StringLength(100)]
        public string DepartmentName { get; set; }

        public int? DepartmentId { get; set; }

        // Cải tiến: Avatar và các thiết lập hiển thị
        public int? AvatarMediaId { get; set; }

        [StringLength(255)]
        public string PublicProfile { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }

        public bool ShowLearningStats { get; set; } = true;

        public bool ShowBadges { get; set; } = true;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("AvatarMediaId")]
        public virtual MediaFile AvatarMedia { get; set; }
    }
}
