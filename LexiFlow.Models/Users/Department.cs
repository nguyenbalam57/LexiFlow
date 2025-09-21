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
    /// Phòng ban trong hệ thống
    /// </summary>
    [Index(nameof(DepartmentName), Name = "IX_Department_Name")]
    [Index(nameof(DepartmentCode), IsUnique = true, Name = "IX_Department_Code")]
    public class Department : AuditableEntity
    {
        /// <summary>
        /// Khóa chính của phòng ban (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }

        /// <summary>
        /// Tên phòng ban (bắt buộc, tối đa 100 ký tự).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Mã định danh phòng ban (duy nhất, tối đa 20 ký tự).
        /// </summary>
        [StringLength(20)]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// ID của phòng ban cha (nếu có).
        /// </summary>
        public int? ParentDepartmentId { get; set; }

        /// <summary>
        /// ID của người quản lý phòng ban.
        /// </summary>
        public int? ManagerUserId { get; set; }

        /// <summary>
        /// Mô tả chi tiết về phòng ban.
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Thứ tự hiển thị của phòng ban trong sơ đồ tổ chức.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Vị trí hoặc địa điểm của phòng ban.
        /// </summary>
        [StringLength(255)]
        public string Location { get; set; }

        /// <summary>
        /// Số lượng nhân sự hiện tại trong phòng ban.
        /// </summary>
        public int? HeadCount { get; set; }

        /// <summary>
        /// Phòng ban cha (điều hướng quan hệ).
        /// </summary>
        [ForeignKey("ParentDepartmentId")]
        public virtual Department ParentDepartment { get; set; }

        /// <summary>
        /// Người quản lý phòng ban (điều hướng quan hệ).
        /// </summary>
        [ForeignKey("ManagerUserId")]
        public virtual User Manager { get; set; }

        /// <summary>
        /// Danh sách các phòng ban con trực thuộc.
        /// </summary>
        public virtual ICollection<Department> ChildDepartments { get; set; }

        /// <summary>
        /// Danh sách người dùng thuộc phòng ban.
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        /// <summary>
        /// Danh sách nhóm (team) thuộc phòng ban.
        /// </summary>
        public virtual ICollection<Team> Teams { get; set; }
    }
}

