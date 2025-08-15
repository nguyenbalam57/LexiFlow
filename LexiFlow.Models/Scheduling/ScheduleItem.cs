using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Scheduling
{
    /// <summary>
    /// Mục lịch trình - đại diện cho một sự kiện/nhiệm vụ được lên lịch
    /// Có thể liên kết với StudyTask, lesson, meeting hoặc các hoạt động khác
    /// </summary>
    [Index(nameof(StartTime), Name = "IX_ScheduleItem_StartTime")]
    [Index(nameof(ScheduleId), nameof(StartTime), Name = "IX_ScheduleItem_Schedule_StartTime")]
    [Index(nameof(StudyTaskId), Name = "IX_ScheduleItem_StudyTask")]
    public class ScheduleItem : AuditableEntity, IActivatable
    {
        /// <summary>
        /// ID duy nhất của mục lịch trình
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleItemId { get; set; }

        /// <summary>
        /// ID của lịch trình chứa mục này
        /// </summary>
        [Required]
        public int ScheduleId { get; set; }

        /// <summary>
        /// Tiêu đề của mục lịch trình
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// Mô tả chi tiết về mục lịch trình
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// ID loại mục lịch trình
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// ID quy tắc lặp lại (nếu có)
        /// </summary>
        public int? RecurrenceId { get; set; }

        /// <summary>
        /// ID nhiệm vụ học tập liên quan (nếu có)
        /// </summary>
        public int? StudyTaskId { get; set; }

        /// <summary>
        /// ID nhiệm vụ (alias cho StudyTaskId để mapping với Context)
        /// </summary>
        [NotMapped]
        public int? TaskId 
        { 
            get => StudyTaskId; 
            set => StudyTaskId = value; 
        }

        /// <summary>
        /// Địa điểm tổ chức sự kiện
        /// </summary>
        [StringLength(255)]
        public string Location { get; set; }

        /// <summary>
        /// URL đến địa điểm (Google Maps, etc.)
        /// </summary>
        [StringLength(255)]
        public string LocationUrl { get; set; }

        /// <summary>
        /// Chi tiết về địa điểm
        /// </summary>
        public string LocationDetails { get; set; }

        /// <summary>
        /// Vĩ độ của địa điểm
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Kinh độ của địa điểm
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// URL của các tệp đính kèm (JSON format)
        /// </summary>
        public string AttachmentUrls { get; set; }

        /// <summary>
        /// URL cuộc họp online (Zoom, Teams, etc.)
        /// </summary>
        public string MeetingUrl { get; set; }

        /// <summary>
        /// Mã tham gia cuộc họp
        /// </summary>
        public string JoinCode { get; set; }

        /// <summary>
        /// Thông tin bổ sung
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Có phải sự kiện cả ngày không
        /// </summary>
        public bool IsAllDay { get; set; } = false;

        /// <summary>
        /// Sự kiện đã bị hủy
        /// </summary>
        public bool IsCancelled { get; set; } = false;

        /// <summary>
        /// Sự kiện đã hoàn thành
        /// </summary>
        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Trạng thái chi tiết của mục lịch trình
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, InProgress, Completed, Cancelled, Postponed

        /// <summary>
        /// Phần trăm hoàn thành (0-100)
        /// </summary>
        [Range(0, 100)]
        public float? CompletionPercentage { get; set; }

        /// <summary>
        /// Thời gian hoàn thành thực tế
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Ghi chú về việc hoàn thành
        /// </summary>
        public string CompletionNotes { get; set; }

        /// <summary>
        /// ID bài học liên quan (nếu có)
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// ID tài liệu liên quan (nếu có)
        /// </summary>
        public int? MaterialId { get; set; }

        /// <summary>
        /// Loại thực thể liên quan (Vocabulary, Grammar, Kanji, etc.)
        /// </summary>
        public string RelatedEntityType { get; set; }

        /// <summary>
        /// ID thực thể liên quan
        /// </summary>
        public int? RelatedEntityId { get; set; }

        /// <summary>
        /// Mức độ ưu tiên (1-5, 5 là ưu tiên cao nhất)
        /// </summary>
        [Range(1, 5)]
        public int? PriorityLevel { get; set; } = 3;

        /// <summary>
        /// Mã màu hiển thị (HEX format)
        /// </summary>
        [StringLength(20)]
        public string ColorCode { get; set; } = "#2196F3";

        /// <summary>
        /// ID nguồn tạo ra mục lịch trình
        /// </summary>
        public int? SourceId { get; set; }

        /// <summary>
        /// Loại nguồn (Manual, StudyPlan, Auto, Import, etc.)
        /// </summary>
        [StringLength(50)]
        public string SourceType { get; set; } = "Manual";

        /// <summary>
        /// ID từ hệ thống bên ngoài (nếu được import)
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Thời gian nhắc nhở trước sự kiện (phút)
        /// </summary>
        public int? ReminderMinutes { get; set; }

        /// <summary>
        /// Có cần xác nhận tham dự không
        /// </summary>
        public bool RequiresConfirmation { get; set; } = false;

        /// <summary>
        /// Có tự động đánh dấu hoàn thành không
        /// </summary>
        public bool AutoComplete { get; set; } = false;

        /// <summary>
        /// Thời gian ước tính cần thiết (phút)
        /// </summary>
        public int? EstimatedDurationMinutes { get; set; }

        /// <summary>
        /// Thời gian thực tế đã dành (phút)
        /// </summary>
        public int? ActualDurationMinutes { get; set; }

        /// <summary>
        /// Ghi chú riêng tư của người dùng
        /// </summary>
        public string PrivateNotes { get; set; }

        /// <summary>
        /// Thẻ tag để phân loại (JSON format)
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Có hiển thị trên lịch công khai không
        /// </summary>
        public bool IsPublic { get; set; } = false;

        /// <summary>
        /// Có thể chỉnh sửa được không
        /// </summary>
        public bool IsEditable { get; set; } = true;

        /// <summary>
        /// Số lần hoãn/thay đổi lịch
        /// </summary>
        public int RescheduleCount { get; set; } = 0;

        /// <summary>
        /// Lịch sử thay đổi (JSON format)
        /// </summary>
        public string ChangeHistory { get; set; }

        // Navigation properties
        /// <summary>
        /// Lịch trình chứa mục này
        /// </summary>
        [ForeignKey("ScheduleId")]
        public virtual Schedule Schedule { get; set; }

        /// <summary>
        /// Loại mục lịch trình
        /// </summary>
        [ForeignKey("TypeId")]
        public virtual ScheduleItemType Type { get; set; }

        /// <summary>
        /// Quy tắc lặp lại (nếu có)
        /// </summary>
        [ForeignKey("RecurrenceId")]
        public virtual ScheduleRecurrence Recurrence { get; set; }

        /// <summary>
        /// Nhiệm vụ học tập liên quan (nếu có)
        /// </summary>
        [ForeignKey("StudyTaskId")]
        public virtual Planning.StudyTask StudyTask { get; set; }

        /// <summary>
        /// Danh sách người tham gia
        /// </summary>
        public virtual ICollection<ScheduleItemParticipant> Participants { get; set; }

        /// <summary>
        /// Danh sách nhắc nhở
        /// </summary>
        public virtual ICollection<ScheduleReminder> Reminders { get; set; }

        // Computed Properties
        /// <summary>
        /// Thời lượng dự kiến (phút)
        /// </summary>
        [NotMapped]
        public int CalculatedDurationMinutes
        {
            get
            {
                if (StartTime.HasValue && EndTime.HasValue)
                {
                    return (int)(EndTime.Value - StartTime.Value).TotalMinutes;
                }
                return EstimatedDurationMinutes ?? 0;
            }
        }

        /// <summary>
        /// Kiểm tra xem có đang diễn ra không
        /// </summary>
        [NotMapped]
        public bool IsCurrentlyActive
        {
            get
            {
                var now = DateTime.UtcNow;
                return StartTime.HasValue && EndTime.HasValue 
                    && now >= StartTime.Value && now <= EndTime.Value 
                    && !IsCancelled && !IsCompleted;
            }
        }

        /// <summary>
        /// Kiểm tra xem có quá hạn không
        /// </summary>
        [NotMapped]
        public bool IsOverdue
        {
            get
            {
                return EndTime.HasValue && DateTime.UtcNow > EndTime.Value 
                    && !IsCompleted && !IsCancelled;
            }
        }

        /// <summary>
        /// Kiểm tra xem có sắp diễn ra không (trong vòng 1 giờ)
        /// </summary>
        [NotMapped]
        public bool IsUpcoming
        {
            get
            {
                var now = DateTime.UtcNow;
                return StartTime.HasValue && StartTime.Value > now 
                    && StartTime.Value <= now.AddHours(1) && !IsCancelled;
            }
        }

        // Methods
        /// <summary>
        /// Đánh dấu mục lịch trình là hoàn thành
        /// </summary>
        /// <param name="completedBy">ID người hoàn thành</param>
        /// <param name="notes">Ghi chú hoàn thành</param>
        public virtual void MarkCompleted(int completedBy, string notes = null)
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
            Status = "Completed";
            CompletionPercentage = 100;
            CompletionNotes = notes;
            
            if (StartTime.HasValue)
                ActualDurationMinutes = (int)(DateTime.UtcNow - StartTime.Value).TotalMinutes;

            UpdateModification(completedBy, "Đánh dấu hoàn thành mục lịch trình");
        }

        /// <summary>
        /// Hủy mục lịch trình
        /// </summary>
        /// <param name="cancelledBy">ID người hủy</param>
        /// <param name="reason">Lý do hủy</param>
        public virtual void Cancel(int cancelledBy, string reason = null)
        {
            IsCancelled = true;
            Status = "Cancelled";
            CompletionNotes = string.IsNullOrEmpty(reason) ? "Đã hủy" : $"Đã hủy: {reason}";
            UpdateModification(cancelledBy, $"Hủy mục lịch trình: {reason}");
        }

        /// <summary>
        /// Hoãn/thay đổi lịch mục
        /// </summary>
        /// <param name="newStartTime">Thời gian bắt đầu mới</param>
        /// <param name="newEndTime">Thời gian kết thúc mới</param>
        /// <param name="rescheduledBy">ID người thay đổi</param>
        /// <param name="reason">Lý do thay đổi</param>
        public virtual void Reschedule(DateTime? newStartTime, DateTime? newEndTime, int rescheduledBy, string reason = null)
        {
            var oldStart = StartTime;
            var oldEnd = EndTime;
            
            StartTime = newStartTime;
            EndTime = newEndTime;
            RescheduleCount++;
            
            var changeNote = $"Thay đổi từ {oldStart:dd/MM/yyyy HH:mm} - {oldEnd:dd/MM/yyyy HH:mm} sang {newStartTime:dd/MM/yyyy HH:mm} - {newEndTime:dd/MM/yyyy HH:mm}";
            if (!string.IsNullOrEmpty(reason))
                changeNote += $". Lý do: {reason}";
                
            UpdateModification(rescheduledBy, changeNote);
        }

        /// <summary>
        /// Bắt đầu thực hiện mục lịch trình
        /// </summary>
        public virtual void Start()
        {
            Status = "InProgress";
            if (CompletionPercentage == null || CompletionPercentage == 0)
                CompletionPercentage = 1; // Đánh dấu đã bắt đầu
            UpdateTimestamp();
        }

        /// <summary>
        /// Cập nhật tiến độ thực hiện
        /// </summary>
        /// <param name="percentage">Phần trăm hoàn thành</param>
        /// <param name="updatedBy">ID người cập nhật</param>
        public virtual void UpdateProgress(float percentage, int updatedBy)
        {
            CompletionPercentage = Math.Max(0, Math.Min(100, percentage));
            
            if (CompletionPercentage >= 100)
            {
                MarkCompleted(updatedBy);
            }
            else if (CompletionPercentage > 0 && Status == "Scheduled")
            {
                Start();
            }

            UpdateModification(updatedBy, $"Cập nhật tiến độ: {percentage}%");
        }

        /// <summary>
        /// Lấy tên hiển thị của mục lịch trình
        /// </summary>
        /// <returns>Tên hiển thị</returns>
        public override string GetDisplayName()
        {
            var timeInfo = StartTime.HasValue ? $" - {StartTime.Value:dd/MM/yyyy HH:mm}" : "";
            return $"{Title}{timeInfo}";
        }

        /// <summary>
        /// Validate mục lịch trình
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public override bool IsValid()
        {
            return base.IsValid() 
                && !string.IsNullOrWhiteSpace(Title)
                && ScheduleId > 0
                && (!StartTime.HasValue || !EndTime.HasValue || StartTime.Value <= EndTime.Value)
                && (CompletionPercentage == null || (CompletionPercentage >= 0 && CompletionPercentage <= 100))
                && (PriorityLevel == null || (PriorityLevel >= 1 && PriorityLevel <= 5));
        }
    }
}
