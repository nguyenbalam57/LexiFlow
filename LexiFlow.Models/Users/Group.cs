using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LexiFlow.Models.Cores;
using LexiFlow.Models.Notification;
using LexiFlow.Models.Users.UserRelations;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.Models.Users
{
    /// <summary>
    /// Nhóm người dùng trong hệ thống
    /// </summary>
    [Index(nameof(GroupName), IsUnique = true, Name = "IX_Group_Name")]
    public class Group : AuditableEntity
    {
        /// <summary>
        /// Khóa chính của nhóm (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        /// <summary>
        /// Tên nhóm (bắt buộc, tối đa 100 ký tự, duy nhất).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về nhóm.
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Đường dẫn đến biểu tượng đại diện của nhóm.
        /// </summary>
        [StringLength(255)]
        public string IconPath { get; set; }

        /// <summary>
        /// ID của người dùng đã tạo nhóm.
        /// </summary>
        public int? CreatedByUserId { get; set; }

        /// <summary>
        /// Loại nhóm (Department, Team, Role, Custom...).
        /// </summary>
        [StringLength(50)]
        public string GroupType { get; set; }

        /// <summary>
        /// Mã màu (dùng để hiển thị trực quan, tối đa 20 ký tự).
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; }

        /// <summary>
        /// Thứ tự hiển thị của nhóm trong danh sách.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Giới hạn số thành viên tối đa của nhóm.
        /// </summary>
        public int? MaxMembers { get; set; }

        /// <summary>
        /// Chính sách tham gia nhóm (Open, Approval, Invitation).
        /// </summary>
        [StringLength(50)]
        public string JoinPolicy { get; set; }

        /// <summary>
        /// Cho phép người dùng tự tham gia nhóm hay không.
        /// </summary>
        public bool AllowSelfJoin { get; set; } = false;

        /// <summary>
        /// ID của nhóm cha (nếu nhóm nằm trong hệ thống phân cấp).
        /// </summary>
        public int? ParentGroupId { get; set; }

        /// <summary>
        /// Nếu true, thành viên nhóm này cũng tự động là thành viên của nhóm cha.
        /// </summary>
        public bool IsNested { get; set; } = false;

        /// <summary>
        /// Các quyền mặc định được gán cho nhóm.
        /// </summary>
        public string DefaultPermissions { get; set; }

        /// <summary>
        /// Xác định nhóm hệ thống (không thể chỉnh sửa/xóa).
        /// </summary>
        public bool IsSystemGroup { get; set; } = false;

        /// <summary>
        /// Cho biết nhóm có được đồng bộ từ nguồn ngoài hay không.
        /// </summary>
        public bool IsSynchronized { get; set; } = false;

        /// <summary>
        /// Nguồn đồng bộ (LDAP, AD, hệ thống ngoài...).
        /// </summary>
        [StringLength(255)]
        public string SyncSource { get; set; }

        /// <summary>
        /// Ngày hết hạn của nhóm (nếu có).
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Số lượng thành viên hiện tại của nhóm.
        /// </summary>
        public int MemberCount { get; set; } = 0;

        /// <summary>
        /// Thời điểm hoạt động gần nhất trong nhóm.
        /// </summary>
        public DateTime? LastActivityDate { get; set; }

        // =================== Navigation properties ===================

        /// <summary>
        /// Người đã tạo nhóm.
        /// </summary>
        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        /// <summary>
        /// Nhóm cha (nếu có).
        /// </summary>
        [ForeignKey("ParentGroupId")]
        public virtual Group ParentGroup { get; set; }

        /// <summary>
        /// Danh sách các nhóm con trực thuộc.
        /// </summary>
        public virtual ICollection<Group> ChildGroups { get; set; }

        /// <summary>
        /// Danh sách quan hệ giữa người dùng và nhóm.
        /// </summary>
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        /// <summary>
        /// Danh sách quyền trực tiếp gán cho nhóm.
        /// </summary>
        public virtual ICollection<GroupPermission> GroupPermissions { get; set; }

        /// <summary>
        /// Danh sách thông báo gửi tới nhóm.
        /// </summary>
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }

        /// <summary>
        /// Danh sách thành viên tham gia lịch biểu có liên quan đến nhóm.
        /// </summary>
        public virtual ICollection<ScheduleItemParticipant> ScheduleItemParticipants { get; set; }
    }
}

