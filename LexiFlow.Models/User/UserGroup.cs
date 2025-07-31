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

namespace LexiFlow.Models.User
{
    /// <summary>
    /// Liên kết giữa người dùng và nhóm
    /// </summary>
    [Index(nameof(UserId), nameof(GroupId), IsUnique = true, Name = "IX_UserGroup_User_Group")]
    public class UserGroup : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserGroupId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int GroupId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        // Cải tiến: Vai trò trong nhóm
        [StringLength(50)]
        public string MemberRole { get; set; } // Member, Admin, Moderator

        public bool IsAdmin { get; set; } = false;

        // Cải tiến: Trạng thái thành viên
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Pending, Suspended

        public DateTime? ExpiryDate { get; set; }

        // Cải tiến: Quản lý thành viên
        public int? InvitedBy { get; set; }

        [StringLength(255)]
        public string JoinReason { get; set; }

        // Cải tiến: Quyền trong nhóm
        public string CustomPermissions { get; set; }

        public bool CanInvite { get; set; } = false;

        public bool CanModifyContent { get; set; } = false;

        // Cải tiến: Thống kê hoạt động
        public DateTime? LastActivity { get; set; }

        [StringLength(255)]
        public string ActivitySummary { get; set; }

        public int ContributionPoints { get; set; } = 0;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        [ForeignKey("InvitedBy")]
        public virtual User InvitedByUser { get; set; }
    }
}
