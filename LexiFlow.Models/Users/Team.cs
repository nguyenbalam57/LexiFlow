using LexiFlow.Models.Cores;
using LexiFlow.Models.Users.UserRelations;
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
    /// Nhóm làm việc trong hệ thống
    /// </summary>
    [Index(nameof(TeamName), Name = "IX_Team_Name")]
    public class Team : AuditableEntity
    {
        /// <summary>
        /// Khóa chính của nhóm (tự tăng).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        /// <summary>
        /// Tên nhóm (bắt buộc, tối đa 100 ký tự).
        /// </summary>
        [Required]
        [StringLength(100)]
        public string TeamName { get; set; }

        /// <summary>
        /// ID của phòng ban mà nhóm trực thuộc (nếu có).
        /// </summary>
        public int? DepartmentId { get; set; }

        /// <summary>
        /// ID của người dùng làm trưởng nhóm.
        /// </summary>
        public int? LeaderUserId { get; set; }

        /// <summary>
        /// Mô tả chi tiết về nhóm.
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Loại nhóm (Project, Functional, Cross-functional...).
        /// </summary>
        [StringLength(50)]
        public string TeamType { get; set; }

        /// <summary>
        /// Sứ mệnh hoặc mục tiêu chính của nhóm.
        /// </summary>
        [StringLength(100)]
        public string Mission { get; set; }

        /// <summary>
        /// Ngày thành lập nhóm.
        /// </summary>
        public DateTime? FormationDate { get; set; }

        /// <summary>
        /// Ngày giải thể nhóm (nếu có).
        /// </summary>
        public DateTime? DisbandDate { get; set; }

        /// <summary>
        /// Giới hạn số lượng thành viên tối đa của nhóm.
        /// </summary>
        public int? MaxMembers { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của nhóm (Active, Dormant, Disbanded).
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        // =================== Navigation properties ===================

        /// <summary>
        /// Phòng ban mà nhóm trực thuộc.
        /// </summary>
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        /// <summary>
        /// Người dùng giữ vai trò trưởng nhóm.
        /// </summary>
        [ForeignKey("LeaderUserId")]
        public virtual User Leader { get; set; }

        /// <summary>
        /// Danh sách quan hệ giữa người dùng và nhóm.
        /// </summary>
        public virtual ICollection<UserTeam> UserTeams { get; set; }
    }
}

