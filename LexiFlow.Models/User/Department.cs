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
    /// Phòng ban trong hệ thống
    /// </summary>
    [Index(nameof(DepartmentName), Name = "IX_Department_Name")]
    [Index(nameof(DepartmentCode), IsUnique = true, Name = "IX_Department_Code")]
    public class Department : AuditableEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        [StringLength(20)]
        public string DepartmentCode { get; set; }

        public int? ParentDepartmentId { get; set; }

        public int? ManagerUserId { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // Cải tiến: Vị trí trong cấu trúc tổ chức
        public int DisplayOrder { get; set; } = 0;

        // Cải tiến: Chi tiết liên hệ
        [StringLength(255)]
        public string ContactEmail { get; set; }

        [StringLength(50)]
        public string ContactPhone { get; set; }

        [StringLength(255)]
        public string Location { get; set; }

        // Cải tiến: Ngân sách và chi tiết tổ chức
        [StringLength(50)]
        public string CostCenter { get; set; }

        public int? HeadCount { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("ParentDepartmentId")]
        public virtual Department ParentDepartment { get; set; }

        [ForeignKey("ManagerUserId")]
        public virtual User Manager { get; set; }

        public virtual ICollection<Department> ChildDepartments { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
