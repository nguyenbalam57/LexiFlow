using LexiFlow.Models.Cores;
using LexiFlow.Models.Medias;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Notifications
{
    /// <summary>
    /// Loại thông báo
    /// </summary>
    [Index(nameof(TypeName), IsUnique = true, Name = "IX_NotificationType_Name")]
    public class NotificationType : BaseEntity
    {
        /// <summary>
        /// Khóa chính của loại thông báo Tự động tăng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        /// <summary>
        /// Tên loại thông báo
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        /// <summary>
        /// Mô tả loại thông báo
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// ID của phương tiện liên quan đến loại thông báo
        /// </summary>
        public int? MediaId { get; set; }
        [ForeignKey("MediaId")]
        public virtual MediaFile Media { get; set; }

        /// <summary>
        /// Mã màu hiển thị cho loại thông báo
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Mẫu thông báo

        /// <summary>
        /// Chủ đề của mẫu thông báo
        /// </summary>
        public string TemplateSubject { get; set; }

        /// <summary>
        /// Nội dung của mẫu thông báo
        /// </summary>
        public string TemplateContent { get; set; }

        /// <summary>
        /// Tham số của mẫu thông báo
        /// </summary>
        public string TemplateParams { get; set; }

        // Cải tiến: Cấu hình gửi thông báo

        /// <summary>
        /// Cho phép hiển thị trong ứng dụng
        /// </summary>
        public bool EnableInApp { get; set; } = true;

        /// <summary>
        /// Cho phép hiển thị thông báo đẩy
        /// </summary>
        public bool EnablePush { get; set; } = false;

        // Cải tiến: Cấu hình hiển thị

        /// <summary>
        /// Thời gian hiển thị thông báo (giây)
        /// </summary>
        public int DisplayDurationSeconds { get; set; } = 0;

        /// <summary>
        /// Yêu cầu xác nhận khi người dùng nhận thông báo
        /// </summary>
        public bool RequiresAcknowledgment { get; set; } = false;

        /// <summary>
        /// Loại hành động liên kết với thông báo
        /// </summary>
        public string ActionType { get; set; }

        // Navigation properties
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
