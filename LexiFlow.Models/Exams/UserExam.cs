using LexiFlow.Models.Cores;
using LexiFlow.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Exams
{
    /// <summary>
    /// Phiên làm bài của user với tracking chi tiết
    /// </summary>
    [Index(nameof(UserId), nameof(StartTime), Name = "IX_UserExam_User_StartTime")]
    [Index(nameof(Status), Name = "IX_UserExam_Status")]
    [Index(nameof(UserId), nameof(Status), Name = "IX_UserExam_User_Status")]
    [Index(nameof(IsDeleted), nameof(Status), Name = "IX_UserExam_SoftDelete_Status")]
    [Index(nameof(EndTime), Name = "IX_UserExam_EndTime")]
    public class UserExam : AuditableEntity
    {
        /// <summary>
        /// Id phiên làm bài (Tự tăng)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserExamId { get; set; }

        /// <summary>
        /// Id người dùng (Khóa ngoại)
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Liên kết đến kỳ thi
        /// </summary>
        public int? JLPTExamId { get; set; }

        /// <summary>
        /// Thời gian bắt đầu làm bài
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Thời gian kết thúc làm bài
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Tổng thời gian làm bài (tính bằng giây)
        /// </summary>
        public int? Duration { get; set; } // in seconds

        /// <summary>
        /// Trạng thái làm bài (InProgress, Completed, Abandoned, Paused, Reviewing)
        /// Bắt đầu từ trạng thái chưa bắt đầu
        /// NotStarted (Chưa làm gì) -> InProgress (đang làm) -> Completed (hoàn thành) hoặc Abandoned (bỏ dở)
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = ExamStatus.NotStarted;

        /// <summary>
        /// Tổng số câu hỏi trong bài thi
        /// </summary>
        public int? TotalQuestions { get; set; }

        /// <summary>
        /// Số câu hỏi đã trả lời
        /// </summary>
        public int? CorrectAnswers { get; set; }

        /// <summary>
        /// Số câu hỏi trả lời đúng
        /// </summary>
        public int? IncorrectAnswers { get; set; }

        /// <summary>
        /// Số câu hỏi bỏ qua
        /// </summary>
        public int? SkippedAnswers { get; set; }

        /// <summary>
        /// Điểm số đạt được (tính theo phần trăm)
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? ScorePercentage { get; set; }

        /// <summary>
        /// Điểm số đạt được (tính theo điểm)
        /// </summary>
        public int? ScorePoints { get; set; }

        /// <summary>
        /// Đạt hay không (true/false)
        /// </summary>
        public bool? IsPassed { get; set; }

        /// <summary>
        /// Ghi chú thêm (nếu có)
        /// </summary>
        public string Notes { get; set; }

        // Progress tracking

        /// <summary>
        /// Chỉ số câu hỏi hiện tại (bắt đầu từ 0)
        /// </summary>
        public int? CurrentQuestionIndex { get; set; } = 0;

        /// <summary>
        /// Danh sách các câu hỏi đã đánh dấu (theo JSON)
        /// </summary>
        public string BookmarkedQuestions { get; set; } // JSON array

        /// <summary>
        /// Bài thi có giới hạn thời gian hay không
        /// </summary>
        public bool IsTimeLimited { get; set; } = true;

        /// <summary>
        /// Tổng thời gian làm bài (nếu có, tính bằng giây)
        /// </summary>
        public int? TimeLimit { get; set; } // Total time in seconds

        /// <summary>
        /// Thời gian còn lại (nếu có, tính bằng giây)
        /// </summary>
        public int? TimeRemaining { get; set; }

        // Advanced analytics
        /// <summary>
        /// Thời gian đã dành cho từng câu hỏi (theo JSON)
        /// </summary>
        public string QuestionTimeSpent { get; set; } // JSON: {questionId: timeInSeconds}

        /// <summary>
        /// Số lần tạm dừng
        /// </summary>
        public int PauseCount { get; set; } = 0;

        /// <summary>
        /// Tổng thời gian tạm dừng (nếu có, tính bằng giây)
        /// </summary>
        public int TotalPauseTime { get; set; } = 0; // in seconds

        // Browser/Device info

        /// <summary>
        /// Thông tin trình duyệt, thiết bị (UserAgent, IP, DeviceType)
        /// </summary>
        [StringLength(200)]
        public string UserAgent { get; set; }

        /// <summary>
        /// Địa chỉ IP khi làm bài
        /// </summary>
        [StringLength(50)]
        public string IpAddress { get; set; }

        // Navigation properties

        /// <summary>
        /// Liên kết đến người dùng
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Liên kết đến kỳ thi
        /// </summary>
        [ForeignKey("JLPTExamId")]
        public virtual JLPTExam Exam { get; set; }

        /// <summary>
        /// Các câu trả lời của người dùng trong bài thi này
        /// </summary>
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }

        // Business methods

        /// <summary>
        /// Kiểm tra xem bài thi đã hoàn thành hay chưa
        /// </summary>
        public bool IsCompleted => Status == ExamStatus.Completed;

        /// <summary>
        /// Kiểm tra xem bài thi đang làm dở hay không
        /// </summary>
        public bool IsInProgress => Status == ExamStatus.InProgress;

        /// <summary>
        /// Kiểm tra xem bài thi đã bị bỏ dở hay không
        /// </summary>
        public bool IsAbandoned => Status == ExamStatus.Abandoned;

        /// <summary>
        /// Đang làm bài
        /// </summary>
        public void StartExam()
        {
            if (Status == ExamStatus.NotStarted)
            {
                StartTime = DateTime.UtcNow;
                Status = ExamStatus.InProgress;
            }
        }

        /// <summary>
        /// Hoàn thành bài thi
        /// </summary>
        public void CompleteExam()
        {
            EndTime = DateTime.UtcNow;
            Duration = (int)(EndTime.Value - StartTime).TotalSeconds;
            Status = ExamStatus.Completed;
        }

        /// <summary>
        /// Bỏ dở bài thi
        /// </summary>
        public void AbandonExam()
        {
            EndTime = DateTime.UtcNow;
            Duration = (int)(EndTime.Value - StartTime).TotalSeconds;
            Status = ExamStatus.Abandoned;
        }

        // Helper methods for JSON fields

        /// <summary>
        /// Lấy danh sách các câu hỏi đã đánh dấu
        /// </summary>
        /// <returns></returns>
        public List<int> GetBookmarkedQuestions()
        {
            if (string.IsNullOrEmpty(BookmarkedQuestions))
                return new List<int>();
            return JsonSerializer.Deserialize<List<int>>(BookmarkedQuestions);
        }

        /// <summary>
        /// Thiết lập danh sách các câu hỏi đã đánh dấu
        /// </summary>
        /// <param name="questions"></param>
        public void SetBookmarkedQuestions(List<int> questions)
        {
            BookmarkedQuestions = JsonSerializer.Serialize(questions);
        }

    }

    /// <summary>
    /// Constants cho exam status
    /// </summary>
    public static class ExamStatus
    {
        /// <summary>
        /// Chưa bắt đầu
        /// </summary>
        public const string NotStarted = "NotStarted";

        /// <summary>
        /// Đang làm bài
        /// </summary>
        public const string InProgress = "InProgress";

        /// <summary>
        /// Đã hoàn thành
        /// </summary>
        public const string Completed = "Completed";

        /// <summary>
        /// Bỏ dở
        /// </summary>
        public const string Abandoned = "Abandoned";

        /// <summary>
        /// Tạm dừng
        /// </summary>
        public const string Paused = "Paused";

        /// <summary>
        /// Đang xem lại
        /// </summary>
        public const string Reviewing = "Reviewing";
    }
}
