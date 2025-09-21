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
    /// Phân nhóm quyền hạn trong hệ thống
    /// </summary>
    [Index(nameof(GroupName), IsUnique = true, Name = "IX_PermissionGroup_Name")]
    public class PermissionGroup : AuditableEntity
    {
        /// <summary>
        /// Khóa chính của nhóm quyền (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionGroupId { get; set; }

        /// <summary>
        /// Tên của nhóm quyền (bắt buộc, tối đa 100 ký tự, duy nhất).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về nhóm quyền.
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Khu vực (module) trong hệ thống mà nhóm quyền này áp dụng.
        /// </summary>
        [StringLength(50)]
        public string ModuleArea { get; set; }

        /// <summary>
        /// Thứ tự hiển thị của nhóm quyền trong danh sách.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Đường dẫn đến biểu tượng của nhóm quyền (nếu có).
        /// </summary>
        [StringLength(255)]
        public string IconPath { get; set; }

        /// <summary>
        /// Mã màu để hiển thị nhóm quyền (tối đa 20 ký tự).
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; }

        /// <summary>
        /// ID của nhóm quyền cha (nếu nhóm này nằm trong hệ thống phân cấp).
        /// </summary>
        public int? ParentGroupId { get; set; }

        /// <summary>
        /// Xác định nhóm quyền này có phải là nhóm hệ thống hay không.
        /// </summary>
        public bool IsSystemGroup { get; set; } = false;

        // =================== Navigation properties ===================

        /// <summary>
        /// Nhóm quyền cha (nếu có).
        /// </summary>
        [ForeignKey("ParentGroupId")]
        public virtual PermissionGroup ParentGroup { get; set; }

        /// <summary>
        /// Danh sách các nhóm quyền con trực thuộc.
        /// </summary>
        public virtual ICollection<PermissionGroup> ChildGroups { get; set; }

    }
}

