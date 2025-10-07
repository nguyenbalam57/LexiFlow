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

namespace LexiFlow.Models.Schedulings
{
    /// <summary>
    /// Loại mục lịch trình - Định nghĩa các loại sự kiện hoặc hoạt động có thể được lên lịch
    /// Hỗ trợ phân loại và cấu hình các mục lịch trình với các quy tắc và mẫu riêng biệt
    /// </summary>
    [Index(nameof(TypeName), IsUnique = true, Name = "IX_ScheduleItemType_Name")]
    public class ScheduleItemType : AuditableEntity
    {
        /// <summary>
        /// Mã định danh duy nhất cho loại mục lịch trình
        /// Sử dụng để phân biệt từng loại sự kiện trong hệ thống
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Tên loại mục lịch trình (duy nhất trong hệ thống)
        /// Ví dụ: "Họp nhóm", "Cuộc gọi", "Sự kiện", "Nhắc nhở", v.v.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        /// <summary>
        /// Mô tả chi tiết về loại mục lịch trình
        /// Giải thích mục đích và cách sử dụng loại sự kiện này
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Đường dẫn đến biểu tượng đại diện cho loại mục lịch trình
        /// Hỗ trợ hiển thị trực quan trong giao diện người dùng
        /// </summary>
        public int? MediaFileId { get; set; }

        /// <summary>
        /// Thông tin tệp phương tiện biểu tượng
        /// </summary>
        [ForeignKey("MediaFileId")]
        public virtual MediaFile IconMedia { get; set; }

        /// <summary>
        /// Màu sắc mặc định cho loại mục lịch trình (mã hex hoặc tên màu)
        /// Sử dụng để phân biệt các loại sự kiện trong lịch trình
        /// </summary>
        [StringLength(20)]
        public string DefaultColor { get; set; }

        /// <summary>
        /// Thời lượng mặc định cho loại mục lịch trình (tính bằng phút)
        /// Được sử dụng khi tạo mới mục lịch trình mà không chỉ định thời gian
        /// </summary>
        public int? DefaultDurationMinutes { get; set; }

        /// <summary>
        /// Cấu hình nhắc nhở mặc định cho loại mục lịch trình
        /// Chuỗi JSON chứa các quy tắc nhắc nhở tự động
        /// </summary>
        public string DefaultReminders { get; set; }

        // Cải tiến: Quy tắc xử lý
        /// <summary>
        /// Cho phép các mục lịch trình của loại này chồng chéo thời gian hay không
        /// True: Cho phép chồng chéo, False: Không cho phép (kiểm tra xung đột)
        /// </summary>
        public bool AllowOverlap { get; set; } = true;

        /// <summary>
        /// Quy tắc hoàn thành cho loại mục lịch trình
        /// Chuỗi JSON định nghĩa các điều kiện để đánh dấu hoàn thành
        /// </summary>
        public string CompletionRules { get; set; }

        // Cải tiến: Mẫu nội dung
        /// <summary>
        /// Tiêu đề mẫu mặc định khi tạo mục lịch trình loại này
        /// Giúp chuẩn hóa cách đặt tên cho các sự kiện tương tự
        /// </summary>
        public string TemplateTitle { get; set; }

        /// <summary>
        /// Mô tả mẫu mặc định cho loại mục lịch trình
        /// Cung cấp nội dung chuẩn để hướng dẫn người dùng
        /// </summary>
        public string TemplateDescription { get; set; }

        /// <summary>
        /// Các trường tùy chỉnh cho loại mục lịch trình
        /// Chuỗi JSON định nghĩa các trường bổ sung cần thu thập
        /// </summary>
        public string TemplateFields { get; set; }

        // Navigation properties
        /// <summary>
        /// Danh sách các mục lịch trình thuộc loại này
        /// Liên kết ngược để truy cập tất cả sự kiện của một loại
        /// </summary>
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }
}
