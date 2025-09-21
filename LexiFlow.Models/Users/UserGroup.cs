using LexiFlow.Models.Cores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Users
{
    /// <summary>
    /// Liên kết giữa người dùng và nhóm
    /// </summary>
    [Index(nameof(UserId), nameof(GroupId), IsUnique = true, Name = "IX_UserGroup_User_Group")]
    public class UserGroup : BaseEntity
    {
        /// <summary>
        /// Khóa chính của quan hệ User - Group (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserGroupId { get; set; }

        /// <summary>
        /// ID của người dùng.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// ID của nhóm.
        /// </summary>
        [Required]
        public int GroupId { get; set; }

        /// <summary>
        /// Thời điểm người dùng tham gia nhóm.
        /// </summary>
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Vai trò của thành viên trong nhóm (Member, Admin, Moderator...).
        /// </summary>
        [StringLength(50)]
        public string MemberRole { get; set; }

        /// <summary>
        /// Đánh dấu người dùng có phải admin trong nhóm hay không.
        /// </summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        /// Trạng thái của thành viên trong nhóm (Active, Pending, Suspended...).
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Ngày hết hạn thành viên (nếu có).
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// ID của người đã mời thành viên này tham gia nhóm.
        /// </summary>
        public int? InvitedBy { get; set; }

        /// <summary>
        /// Lý do hoặc ghi chú khi tham gia nhóm.
        /// </summary>
        [StringLength(255)]
        public string JoinReason { get; set; }

        /// <summary>
        /// Các quyền tùy chỉnh riêng cho thành viên này trong nhóm.
        /// </summary>
        public string CustomPermissions { get; set; }

        /// <summary>
        /// Cho phép thành viên này mời người khác tham gia nhóm.
        /// </summary>
        public bool CanInvite { get; set; } = false;

        /// <summary>
        /// Cho phép thành viên chỉnh sửa nội dung trong nhóm.
        /// </summary>
        public bool CanModifyContent { get; set; } = false;

        /// <summary>
        /// Thời điểm hoạt động gần nhất của thành viên trong nhóm.
        /// </summary>
        public DateTime? LastActivity { get; set; }

        /// <summary>
        /// Tóm tắt hoạt động gần đây của thành viên trong nhóm.
        /// </summary>
        [StringLength(255)]
        public string ActivitySummary { get; set; }

        /// <summary>
        /// Điểm đóng góp của thành viên trong nhóm.
        /// </summary>
        public int ContributionPoints { get; set; } = 0;

        // =================== Navigation properties ===================

        /// <summary>
        /// Người dùng trong quan hệ này.
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Nhóm mà người dùng tham gia.
        /// </summary>
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        /// <summary>
        /// Người đã mời thành viên này vào nhóm (nếu có).
        /// </summary>
        [ForeignKey("InvitedBy")]
        public virtual User InvitedByUser { get; set; }
    }
}

