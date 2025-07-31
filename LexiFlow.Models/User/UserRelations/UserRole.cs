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

namespace LexiFlow.Models.User.UserRelations
{
    /// <summary>
    /// Liên kết người dùng với vai trò
    /// </summary>
    [Index(nameof(UserId), nameof(RoleId), IsUnique = true, Name = "IX_UserRole_UserRole")]
    public class UserRole : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserRoleId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        // Cải tiến: Hạn chế thời gian
        public DateTime? ExpiresAt { get; set; }

        // Cải tiến: Người cấp vai trò
        public int? AssignedBy { get; set; }

        // Cải tiến: Phạm vi giới hạn
        [StringLength(255)]
        public string Scope { get; set; }

        // Cải tiến: Trạng thái kích hoạt
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("AssignedBy")]
        public virtual User AssignedByUser { get; set; }
    }
}
