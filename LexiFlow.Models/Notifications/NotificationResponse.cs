using LexiFlow.Models.Cores;
using LexiFlow.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Notifications
{
    /// <summary>
    /// Phản hồi thông báo
    /// </summary>
    public class NotificationResponse : BaseEntity
    {
        /// <summary>
        /// Khóa chính của phản hồi thông báo Tự động tăng
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponseId { get; set; }

        /// <summary>
        /// ID của người nhận thông báo
        /// Thông báo này liên kết với NotificationRecipient để biết ai đã phản hồi và thông báo đến ai.
        /// Thông tin này giúp theo dõi phản hồi của từng người nhận một cách riêng biệt.
        /// </summary>
        [Required]
        public int RecipientId { get; set; }

        /// <summary>
        /// Nội dung phản hồi
        /// </summary>
        public string ResponseContent { get; set; }

        // Cải tiến: Loại phản hồi

        /// <summary>
        /// Loại phản hồi (ví dụ: Text, Button, Action)
        /// Nội dung chi tiết về loại phản hồi để xác định cách xử lý và hiển thị phản hồi.
        /// Ví dụ: nếu là "Button", có thể có các nút như "Accept", "Decline". 
        /// Nếu là "Action", có thể liên kết với một hành động cụ thể trong hệ thống.
        /// Nếu là "Text", đó có thể là phản hồi dạng văn bản tự do từ người dùng.
        /// </summary>
        [StringLength(50)]
        public string ResponseType { get; set; } // Text, Button, Action

        /// <summary>
        /// Hành động phản hồi (ví dụ: Accept, Decline, MoreInfo)
        /// Nội dung chi tiết về hành động phản hồi để xác định cách xử lý và hiển thị phản hồi.
        /// Ví dụ: nếu là "Accept", có thể thực hiện hành động chấp nhận thông báo.
        /// Nếu là "Decline", có thể thực hiện hành động từ chối thông báo.
        /// Nếu là "MoreInfo", có thể yêu cầu thêm thông tin từ người dùng.
        /// </summary>
        [StringLength(50)]
        public string ResponseAction { get; set; }

        /// <summary>
        /// Dữ liệu bổ sung liên quan đến phản hồi (nếu có)
        /// Thông tin này có thể bao gồm các tham số bổ sung hoặc ngữ cảnh liên quan đến phản hồi.
        /// Ví dụ: nếu phản hồi là một hành động, dữ liệu này có thể chứa thông tin về hành động đó.
        /// </summary>
        public string ResponseData { get; set; }

        // Cải tiến: Thêm thông tin

        /// <summary>
        /// ID của người dùng đã phản hồi (có thể null nếu là hệ thống)
        /// Nếu phản hồi được tạo bởi hệ thống (ví dụ: tự động gửi phản hồi), trường này có thể để null.
        /// </summary>
        public int? RespondedByUserId { get; set; }

        /// <summary>
        /// Xác định xem phản hồi có phải từ hệ thống hay không
        /// </summary>
        public bool IsSystemResponse { get; set; } = false;

        /// <summary>
        /// Xác định xem phản hồi có phải là riêng tư hay không
        /// Nếu là true, phản hồi này chỉ có thể được xem bởi người nhận và người gửi (nếu có).
        /// </summary>
        public bool IsPrivate { get; set; } = false;

        /// <summary>
        /// Thời gian phản hồi
        /// </summary>
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// URL đính kèm của phản hồi (nếu có)
        /// </summary>
        public string AttachmentUrl { get; set; }

        // Cải tiến: Liên kết với hành động

        /// <summary>
        /// Loại hành động liên kết với phản hồi
        /// Ví dụ: "OpenUrl", "CompleteTask", "ViewDetails"
        /// </summary>
        public string ActionType { get; set; }

        /// <summary>
        /// Mục tiêu hành động liên kết với phản hồi (ví dụ: URL, ID nhiệm vụ)
        /// </summary>
        public string ActionTarget { get; set; }

        /// <summary>
        /// Xác định xem hành động đã hoàn thành hay chưa
        /// </summary>
        public bool ActionCompleted { get; set; } = false;

        /// <summary>
        /// Thời gian hành động được hoàn thành (nếu có)
        /// </summary>
        public DateTime? ActionCompletedAt { get; set; }

        // Navigation properties

        /// <summary>
        /// Người nhận thông báo liên quan đến phản hồi
        /// </summary>
        [ForeignKey("RecipientId")]
        public virtual NotificationRecipient Recipient { get; set; }

        /// <summary>
        /// Người dùng đã phản hồi (nếu có)
        /// </summary>
        [ForeignKey("RespondedByUserId")]
        public virtual User RespondedByUser { get; set; }
    }
}
