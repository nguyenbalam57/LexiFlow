using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Sync
{
    /// <summary>
    /// Thông tin về các mục đã xóa
    /// </summary>
    public class DeletedItem
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeletedItemID { get; set; }

        /// <summary>
        /// Loại entity
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntityType { get; set; }

        /// <summary>
        /// ID entity
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// ID người dùng xóa
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// Thời gian xóa
        /// </summary>
        public DateTime DeletedAt { get; set; }

        /// <summary>
        /// Lý do xóa
        /// </summary>
        [StringLength(500)]
        public string DeletionReason { get; set; }

        /// <summary>
        /// Dữ liệu sao lưu
        /// </summary>
        public string BackupData { get; set; }

        /// <summary>
        /// Người dùng liên quan
        /// </summary>
        [ForeignKey("UserID")]
        public virtual Models.User.User User { get; set; }
    }
}
