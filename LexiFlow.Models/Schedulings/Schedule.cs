using LexiFlow.Models.Cores;
using LexiFlow.Models.Medias;
using LexiFlow.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models.Schedulings
{
    /// <summary>
    /// Lịch trình - Quản lý các lịch trình cá nhân, nhóm hoặc khóa học
    /// Hỗ trợ cấu hình, chia sẻ và đồng bộ với các hệ thống bên ngoài
    /// </summary>
    [Index(nameof(Name), Name = "IX_Schedule_Name")]
    public class Schedule : AuditableEntity
    {
        /// <summary>
        /// Mã định danh duy nhất cho lịch trình
        /// Sử dụng để phân biệt từng lịch trình trong hệ thống
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Tên lịch trình (bắt buộc, tối đa 100 ký tự)
        /// Tên hiển thị của lịch trình, phải duy nhất trong phạm vi người dùng
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Mô tả chi tiết về lịch trình
        /// Giải thích mục đích, nội dung và cách sử dụng của lịch trình
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Loại lịch trình (Personal, Team, Course)
        /// Xác định phạm vi và cách thức sử dụng của lịch trình
        /// </summary>
        [StringLength(50)]
        public string ScheduleType { get; set; }

        /// <summary>
        /// Màu chủ đề để hiển thị lịch trình (tối đa 255 ký tự)
        /// Mã màu hex hoặc tên màu để phân biệt trực quan các lịch trình
        /// </summary>
        [StringLength(255)]
        public string ThemeColor { get; set; }

        /// <summary>
        /// Mã định danh tệp phương tiện liên quan (nếu có)
        /// Liên kết lịch trình với tệp phương tiện để hiển thị
        /// </summary>
        public int? MediaFileId { get; set; }

        /// <summary>
        /// Tệp phương tiện liên quan (nếu có)
        /// </summary>
        [ForeignKey("MediaFileId")]
        public virtual MediaFile MediaFile { get; set; }

        // Cải tiến: Cấu hình nhắc nhở
        /// <summary>
        /// Bật tính năng nhắc nhở cho lịch trình
        /// True: Cho phép gửi nhắc nhở, False: Tắt tất cả nhắc nhở
        /// </summary>
        public bool EnableReminders { get; set; } = true;

        // Cải tiến: Cấu hình đồng bộ
        /// <summary>
        /// Cho phép đồng bộ lịch trình với hệ thống bên ngoài
        /// True: Bật đồng bộ, False: Chỉ sử dụng nội bộ
        /// </summary>
        public bool EnableSync { get; set; } = false;

        /// <summary>
        /// Nguồn đồng bộ (Google Calendar, Outlook, etc.)
        /// Chỉ định hệ thống bên ngoài để đồng bộ dữ liệu
        /// </summary>
        public string SyncSource { get; set; }

        /// <summary>
        /// Cấu hình chi tiết cho đồng bộ (dạng JSON)
        /// Chứa các tham số như tần suất, phạm vi đồng bộ
        /// </summary>
        public string SyncSettings { get; set; }

        // Cải tiến: Cấu hình chia sẻ
        /// <summary>
        /// Danh sách người dùng được chia sẻ lịch trình (dạng JSON array)
        /// Chỉ định ai có quyền truy cập lịch trình này
        /// </summary>
        public string SharedWith { get; set; }

        /// <summary>
        /// Quyền xem chi tiết cho người được chia sẻ
        /// Định nghĩa mức độ truy cập (Full, Limited, ReadOnly)
        /// </summary>
        public string ViewPermissions { get; set; }

        /// <summary>
        /// Quyền chỉnh sửa cho người được chia sẻ
        /// Xác định ai có thể sửa đổi lịch trình
        /// </summary>
        public string EditPermissions { get; set; }

        /// <summary>
        /// Lịch trình công khai hay riêng tư
        /// True: Ai cũng có thể xem, False: Chỉ người được chia sẻ
        /// </summary>
        public bool IsPublic { get; set; } = false;

        // Cải tiến: Thời gian áp dụng
        /// <summary>
        /// Thời điểm bắt đầu hiệu lực của lịch trình
        /// Lịch trình chỉ hoạt động từ ngày này trở đi
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Thời điểm kết thúc hiệu lực của lịch trình
        /// Lịch trình ngừng hoạt động sau ngày này
        /// </summary>
        public DateTime? ValidTo { get; set; }

        // Cải tiến: Lịch biểu tiết học
        /// <summary>
        /// Tổng số tiết học trong lịch trình
        /// Áp dụng cho lịch trình kiểu Course
        /// </summary>
        public int? TotalSessions { get; set; }

        /// <summary>
        /// Mẫu phân bố các tiết học (dạng JSON)
        /// Định nghĩa cách sắp xếp thời gian cho từng tiết
        /// </summary>
        public string SessionPattern { get; set; }

        /// <summary>
        /// Danh sách các mục lịch trình thuộc về lịch trình này
        /// Liên kết ngược để quản lý tất cả sự kiện trong lịch
        /// </summary>
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }
}
