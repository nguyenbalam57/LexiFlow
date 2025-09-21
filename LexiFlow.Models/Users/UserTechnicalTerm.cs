using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.TechnicalTerms;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LexiFlow.Models.Users
{
    /// <summary>
    /// Model đại diện cho mối liên kết giữa người dùng và thuật ngữ kỹ thuật trong hệ thống học tập LexiFlow.
    /// Lưu trữ toàn bộ thông tin về quá trình học tập và tiến độ của người dùng đối với từng thuật ngữ cụ thể.
    /// </summary>
    /// <remarks>
    /// Entity này quản lý:
    /// - Tiến độ học tập và mức độ thành thạo của người dùng
    /// - Thuật toán spaced repetition để tối ưu việc ôn tập
    /// - Thống kê chi tiết về quá trình học (số lần đúng/sai, thời gian phản hồi)
    /// - Cài đặt cá nhân hóa (ưu tiên, ghi chú, ví dụ riêng)
    /// - Lịch sử học tập chi tiết để phân tích và cải thiện
    /// </remarks>
    [Index(nameof(UserId), nameof(TermId), IsUnique = true, Name = "IX_UserTechnicalTerm_User_Term")]
    public class UserTechnicalTerm : BaseEntity
    {
        /// <summary>
        /// Khóa chính của bảng liên kết user-thuật ngữ
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTermId { get; set; }

        /// <summary>
        /// ID của người dùng
        /// </summary>
        /// <remarks>Khóa ngoại tham chiếu đến bảng User</remarks>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// ID của thuật ngữ kỹ thuật
        /// </summary>
        /// <remarks>Khóa ngoại tham chiếu đến bảng TechnicalTerm</remarks>
        [Required]
        public int TermId { get; set; }

        #region Tiến độ học tập cơ bản

        /// <summary>
        /// Mức độ thành thạo của người dùng đối với thuật ngữ này
        /// </summary>
        /// <value>Thang điểm từ 0-10, trong đó 0 là chưa biết và 10 là thành thạo hoàn toàn. Mặc định: 0</value>
        public int ProficiencyLevel { get; set; } = 0;

        /// <summary>
        /// Thời điểm gần nhất người dùng thực hành thuật ngữ này
        /// </summary>
        /// <value>Null nếu chưa bao giờ thực hành</value>
        public DateTime? LastPracticed { get; set; }

        /// <summary>
        /// Tổng số lần người dùng đã học/thực hành thuật ngữ này
        /// </summary>
        /// <value>Số nguyên >= 0. Mặc định: 0</value>
        public int StudyCount { get; set; } = 0;

        /// <summary>
        /// Số lần trả lời đúng
        /// </summary>
        /// <value>Số nguyên >= 0. Mặc định: 0</value>
        public int CorrectCount { get; set; } = 0;

        #endregion

        #region Spaced Repetition Algorithm

        /// <summary>
        /// Ngày dự kiến ôn tập lại thuật ngữ này theo thuật toán spaced repetition
        /// </summary>
        /// <value>Null nếu chưa xác định hoặc không cần ôn tập</value>
        public DateTime? NextReviewDate { get; set; }

        /// <summary>
        /// Độ mạnh của trí nhớ đối với thuật ngữ này trong thuật toán spaced repetition
        /// </summary>
        /// <value>Số nguyên thể hiện mức độ ghi nhớ, càng cao càng nhớ lâu. Mặc định: 0</value>
        public int MemoryStrength { get; set; } = 0;

        /// <summary>
        /// Hệ số dễ dàng trong thuật toán spaced repetition (SM-2)
        /// </summary>
        /// <value>Giá trị double >= 1.3, càng cao thì khoảng cách ôn tập càng dài. Mặc định: 2.5</value>
        public double EaseFactor { get; set; } = 2.5;

        /// <summary>
        /// Khoảng thời gian (tính bằng ngày) đến lần ôn tập tiếp theo
        /// </summary>
        /// <value>Số nguyên > 0. Mặc định: 1 ngày</value>
        public int IntervalDays { get; set; } = 1;

        #endregion

        #region Thống kê chi tiết

        /// <summary>
        /// Số lần trả lời sai
        /// </summary>
        /// <value>Số nguyên >= 0. Mặc định: 0</value>
        public int IncorrectCount { get; set; } = 0;

        /// <summary>
        /// Số lần trả lời đúng liên tiếp hiện tại
        /// </summary>
        /// <value>Số nguyên >= 0, reset về 0 khi trả lời sai. Mặc định: 0</value>
        public int ConsecutiveCorrect { get; set; } = 0;

        /// <summary>
        /// Thời gian phản hồi trung bình tính bằng milliseconds
        /// </summary>
        /// <value>Null nếu chưa có dữ liệu, số nguyên > 0 nếu có</value>
        public int? AverageResponseTimeMs { get; set; }

        #endregion

        #region Cài đặt cá nhân hóa

        /// <summary>
        /// Mức độ ưu tiên học thuật ngữ này theo đánh giá của người dùng
        /// </summary>
        /// <value>Thang điểm từ 1-5, trong đó 1 là ít quan trọng và 5 là rất quan trọng. Mặc định: 3</value>
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Ghi chú cá nhân của người dùng về thuật ngữ này
        /// </summary>
        /// <value>Chuỗi văn bản tự do, có thể chứa mẹo nhớ, liên kết, hoặc bất kỳ ghi chú nào</value>
        public string UserNotes { get; set; }

        /// <summary>
        /// Đánh dấu thuật ngữ này là yêu thích/bookmark
        /// </summary>
        /// <value>True nếu được bookmark, False nếu không. Mặc định: false</value>
        public bool IsBookmarked { get; set; } = false;

        #endregion

        #region Đánh giá chủ quan

        /// <summary>
        /// Đánh giá độ khó của thuật ngữ theo quan điểm cá nhân của người dùng
        /// </summary>
        /// <value>Thang điểm từ 1-5, trong đó 1 là rất dễ và 5 là rất khó. Null nếu chưa đánh giá</value>
        [Range(1, 5)]
        public int? UserDifficultyRating { get; set; }

        /// <summary>
        /// Đánh giá mức độ quan trọng của thuật ngữ theo quan điểm cá nhân của người dùng
        /// </summary>
        /// <value>Thang điểm từ 1-5, trong đó 1 là ít quan trọng và 5 là rất quan trọng. Null nếu chưa đánh giá</value>
        [Range(1, 5)]
        public int? UserImportanceRating { get; set; }

        #endregion

        #region Phân loại và tổ chức

        /// <summary>
        /// Các thẻ (tags) do người dùng tự định nghĩa để phân loại thuật ngữ
        /// </summary>
        /// <value>Chuỗi các tag phân cách bởi dấu phẩy hoặc dấu chấm phẩy, tối đa 255 ký tự</value>
        [StringLength(255)]
        public string UserTags { get; set; }

        /// <summary>
        /// Danh mục do người dùng tự định nghĩa để nhóm thuật ngữ
        /// </summary>
        /// <value>Tên danh mục tùy chỉnh, tối đa 50 ký tự</value>
        [StringLength(50)]
        public string UserCategory { get; set; }

        #endregion

        #region Nội dung cá nhân hóa

        /// <summary>
        /// Ví dụ cá nhân do người dùng tự tạo để minh họa thuật ngữ
        /// </summary>
        /// <value>Câu ví dụ tùy chỉnh giúp người dùng hiểu và nhớ thuật ngữ tốt hơn, tối đa 500 ký tự</value>
        [StringLength(500)]
        public string PersonalExample { get; set; }

        /// <summary>
        /// Định nghĩa cá nhân do người dùng tự viết cho thuật ngữ
        /// </summary>
        /// <value>Định nghĩa bằng ngôn ngữ dễ hiểu của riêng người dùng, tối đa 255 ký tự</value>
        [StringLength(255)]
        public string PersonalDefinition { get; set; }

        #endregion

        #region Lịch sử và phân tích

        /// <summary>
        /// Lịch sử chi tiết quá trình học thuật ngữ này được lưu dưới dạng JSON
        /// </summary>
        /// <value>
        /// Chuỗi JSON chứa:
        /// - Thời gian từng lần học
        /// - Kết quả đúng/sai từng lần
        /// - Thời gian phản hồi từng lần
        /// - Loại bài tập (flashcard, quiz, v.v.)
        /// - Ghi chú từng phiên học
        /// </value>
        public string LearningHistory { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Thông tin người dùng sở hữu bản ghi học tập này
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Thông tin thuật ngữ kỹ thuật được học
        /// </summary>
        [ForeignKey("TermId")]
        public virtual TechnicalTerm Term { get; set; }

        #endregion
    }
}