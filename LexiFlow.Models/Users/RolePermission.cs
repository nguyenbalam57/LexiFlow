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
    /// Quan hệ giữa vai trò và quyền trong hệ thống
    /// </summary>
    [Index(nameof(RoleId), nameof(PermissionId), IsUnique = true, Name = "IX_RolePermission_RolePermission")]
    public class RolePermission : BaseEntity
    {
        /// <summary>
        /// Khóa chính của quan hệ vai trò - quyền (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolePermissionId { get; set; }

        /// <summary>
        /// ID của vai trò.
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// ID của quyền.
        /// </summary>
        [Required]
        public int PermissionId { get; set; }

        /// <summary>
        /// Phạm vi áp dụng quyền (ví dụ: Toàn hệ thống, theo phòng ban, theo dự án...).
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Nếu true, quyền này có thể override (ghi đè) quyền khác.
        /// </summary>
        public bool IsOverride { get; set; } = false;

        /// <summary>
        /// ID của người đã cấp quyền này.
        /// </summary>
        public int? GrantedBy { get; set; }

        // =================== Navigation properties ===================

        /// <summary>
        /// Vai trò được gán quyền.
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        /// <summary>
        /// Quyền được gán cho vai trò.
        /// </summary>
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        /// <summary>
        /// Người đã cấp quyền cho vai trò này.
        /// </summary>
        [ForeignKey("GrantedBy")]
        public virtual User GrantedByUser { get; set; }
    }
}
