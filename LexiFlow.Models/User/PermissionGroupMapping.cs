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
    /// Ánh xạ quyền vào các nhóm quyền
    /// </summary>
    [Index(nameof(PermissionGroupId), nameof(PermissionId), IsUnique = true, Name = "IX_PermissionGroupMapping_Group_Permission")]
    public class PermissionGroupMapping : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MappingId { get; set; }

        [Required]
        public int PermissionGroupId { get; set; }

        [Required]
        public int PermissionId { get; set; }

        public int DisplayOrder { get; set; } = 0;

        // Cải tiến: Phân loại và ghi chú
        [StringLength(50)]
        public string SubCategory { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        // Cải tiến: Thuộc tính mở rộng
        public bool IsRequired { get; set; } = false; // Quyền bắt buộc trong nhóm

        public bool IsDefault { get; set; } = false; // Cấp quyền mặc định

        // Cải tiến: Quản lý hệ thống
        public bool IsSystem { get; set; } = false; // Không thể gỡ bỏ

        public int? CreatedByUserId { get; set; }

        // Cải tiến: Phụ thuộc quyền
        public int? DependsOnPermissionId { get; set; }

        // Cải tiến: Trạng thái
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("PermissionGroupId")]
        public virtual PermissionGroup PermissionGroup { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("DependsOnPermissionId")]
        public virtual Permission DependsOnPermission { get; set; }
    }
}
