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

namespace LexiFlow.Models.Notifications
{
    /// <summary>
    /// Trạng thái thông báo
    /// Thể hiện trạng thái hiện tại của thông báo, ví dụ: Đang chờ, Đã gửi, Đã đọc, Đã lưu trữ, Đã xóa
    /// </summary>
    [Index(nameof(StatusName), IsUnique = true, Name = "IX_NotificationStatus_Name")]
    public class NotificationStatus : BaseEntity
    {
        /// <summary>
        /// Khóa chính của trạng thái thông báo Tự động tăng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        /// <summary>
        /// Tên trạng thái thông báo
        /// </summary>
        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

        /// <summary>
        /// Mô tả trạng thái thông báo
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Mã màu hiển thị cho trạng thái thông báo
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Cấu hình trạng thái

        /// <summary>
        /// Xác định xem trạng thái này có phải là trạng thái kết thúc hay không
        /// </summary>
        public bool IsTerminal { get; set; } = false;

        /// <summary>
        /// Xác định xem trạng thái này có yêu cầu hành động hay không
        /// </summary>
        public bool RequiresAction { get; set; } = false;

        /// <summary>
        /// Xác định xem trạng thái này có phải là trạng thái mặc định hay không
        /// </summary>
        public bool IsDefault { get; set; } = false;

        // Cải tiến: Chuyển đổi trạng thái

        /// <summary>
        /// Danh sách các trạng thái được phép chuyển đổi từ trạng thái hiện tại (dưới dạng chuỗi phân tách bằng dấu phẩy)
        /// </summary>
        public string AllowedTransitions { get; set; }

        /// <summary>
        /// Hành động được phép khi chuyển đổi trạng thái (dưới dạng chuỗi phân tách bằng dấu phẩy)
        /// </summary>
        public string TransitionActions { get; set; }

        // Navigation properties
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }
    }
}
