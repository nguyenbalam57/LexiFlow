using LexiFlow.Models.Cores;
using LexiFlow.Models.Users;
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
    /// Người tham gia mục lịch trình - Quản lý thông tin chi tiết về từng người tham gia trong các sự kiện lịch trình
    /// Hỗ trợ theo dõi vai trò, trạng thái tham dự, thời gian tham gia và phản hồi từ người tham gia
    /// </summary>
    [Index(nameof(ScheduleItemId), nameof(UserId), IsUnique = true, Name = "IX_ScheduleItemParticipant_Item_User")]
    public class ScheduleItemParticipant : BaseEntity
    {
        /// <summary>
        /// Mã định danh duy nhất cho người tham gia
        /// Sử dụng để phân biệt từng bản ghi người tham gia trong hệ thống
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantId { get; set; }

        /// <summary>
        /// Mã định danh của mục lịch trình
        /// Liên kết người tham gia với sự kiện cụ thể trong lịch trình
        /// </summary>
        [Required]
        public int ScheduleItemId { get; set; }

        /// <summary>
        /// Thông tin mục lịch trình mà người này tham gia
        /// </summary>
        [ForeignKey("ScheduleItemId")]
        public virtual ScheduleItem ScheduleItem { get; set; }

        /// <summary>
        /// Mã định danh người dùng tham gia (có thể null nếu là người ngoài hệ thống)
        /// Cho phép mời cả người dùng đã đăng ký và khách mời bên ngoài
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Thông tin người dùng tham gia
        /// Cung cấp quyền truy cập vào hồ sơ và thông tin liên lạc của người tham gia
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Mã định danh nhóm tham gia (có thể null nếu là cá nhân)
        /// Hỗ trợ mời cả nhóm người dùng vào sự kiện
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Thông tin nhóm tham gia
        /// Cung cấp quyền truy cập vào hồ sơ và thông tin liên lạc của nhóm tham gia
        /// </summary>
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        // Cải tiến: Vai trò và trạng thái
        /// <summary>
        /// Vai trò của người tham gia trong sự kiện (Organizer, Speaker, Attendee, Moderator, etc.)
        /// Xác định quyền hạn và trách nhiệm của người tham gia
        /// </summary>
        [StringLength(50)]
        public string ParticipantRole { get; set; }

        /// <summary>
        /// Trạng thái tham dự của người tham gia (Invited, Confirmed, Attended, Absent)
        /// Theo dõi quá trình từ lời mời đến tham dự thực tế
        /// </summary>
        [StringLength(50)]
        public string AttendanceStatus { get; set; }

        /// <summary>
        /// Trạng thái phản hồi lời mời (0: Chưa phản hồi, 1: Chấp nhận, 2: Có thể, 3: Từ chối)
        /// Cho phép người tham gia thể hiện ý định tham dự trước sự kiện
        /// </summary>
        public int? ResponseStatus { get; set; }

        // Cải tiến: Theo dõi tham gia
        /// <summary>
        /// Thời điểm người tham gia bắt đầu tham gia sự kiện
        /// Ghi nhận check-in thực tế của người tham gia
        /// </summary>
        public DateTime? JoinedAt { get; set; }

        /// <summary>
        /// Thời điểm người tham gia rời khỏi sự kiện
        /// Ghi nhận check-out của người tham gia (nếu rời sớm)
        /// </summary>
        public DateTime? LeftAt { get; set; }

        /// <summary>
        /// Tổng thời gian tham gia sự kiện (tính bằng phút)
        /// Thống kê thời lượng tham dự thực tế để đánh giá mức độ tham gia
        /// </summary>
        public int? AttendanceMinutes { get; set; }

        /// <summary>
        /// Đánh dấu người tham gia có đến muộn hay không
        /// Hỗ trợ thống kê và quản lý tính đúng giờ
        /// </summary>
        public bool IsLate { get; set; } = false;

        // Cải tiến: Thông tin bổ sung
        /// <summary>
        /// Ghi chú của người tổ chức về người tham gia
        /// Lưu trữ thông tin quan trọng hoặc yêu cầu đặc biệt
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Mã xác nhận tham dự (QR code, PIN, etc.)
        /// Sử dụng để check-in nhanh chóng tại sự kiện
        /// </summary>
        public string AttendanceCode { get; set; }

        /// <summary>
        /// Phản hồi đánh giá từ người tham gia sau sự kiện
        /// Thu thập ý kiến để cải thiện chất lượng tổ chức
        /// </summary>
        public string Feedback { get; set; }

        // Cải tiến: Ghi chú của người tham gia
        /// <summary>
        /// Ghi chú cá nhân của người tham gia về sự kiện
        /// Cho phép người tham gia lưu lại thông tin quan trọng
        /// </summary>
        public string ParticipantNotes { get; set; }

        /// <summary>
        /// Thời điểm người tham gia phản hồi lời mời
        /// Theo dõi tốc độ phản hồi của người được mời
        /// </summary>
        public DateTime? ResponseTime { get; set; }

        /// <summary>
        /// Lý do phản hồi của người tham gia (đặc biệt khi từ chối)
        /// Giúp hiểu rõ nguyên nhân không tham gia để cải thiện
        /// </summary>
        public string ResponseReason { get; set; }

    }
}
