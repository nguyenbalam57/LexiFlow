using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LexiFlow.Models.Core;
using LexiFlow.Models.Notification;
using Microsoft.EntityFrameworkCore;

namespace LexiFlow.Models.User
{
    /// <summary>
    /// Nhóm người dùng trong hệ thống
    /// </summary>
    [Index(nameof(GroupName), IsUnique = true, Name = "IX_Group_Name")]
    public class Group : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        public int? CreatedByUserId { get; set; }

        // Cải tiến: Phân loại và hiển thị
        [StringLength(50)]
        public string GroupType { get; set; } // Department, Team, Role, Custom

        [StringLength(20)]
        public string ColorCode { get; set; }

        public int DisplayOrder { get; set; } = 0;

        // Cải tiến: Quản lý thành viên
        public int? MaxMembers { get; set; }

        [StringLength(50)]
        public string JoinPolicy { get; set; } // Open, Approval, Invitation

        public bool AllowSelfJoin { get; set; } = false;

        // Cải tiến: Phân cấp và quản lý
        public int? ParentGroupId { get; set; }

        public bool IsNested { get; set; } = false; // Thành viên tự động thuộc nhóm cha

        // Cải tiến: Quyền và thông tin hệ thống
        public string DefaultPermissions { get; set; }

        public bool IsSystemGroup { get; set; } = false;

        public bool IsSynchronized { get; set; } = false; // Đồng bộ với nguồn ngoài

        [StringLength(255)]
        public string SyncSource { get; set; }

        // Cải tiến: Thời gian hoạt động
        public DateTime? ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;

        // IActivatable implementation
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        // Cải tiến: Thống kê
        public int MemberCount { get; set; } = 0;

        public DateTime? LastActivityDate { get; set; }

        // Navigation properties
        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("ParentGroupId")]
        public virtual Group ParentGroup { get; set; }

        public virtual ICollection<Group> ChildGroups { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<GroupPermission> GroupPermissions { get; set; }
        public virtual ICollection<PermissionGroupMapping> PermissionGroupMappings { get; set; }
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }
        public virtual ICollection<Scheduling.ScheduleItemParticipant> ScheduleItemParticipants { get; set; }
    }

}
