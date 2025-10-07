using LexiFlow.Models.Cores;
using LexiFlow.Models.Exams;
using LexiFlow.Models.Learning.Commons;
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

namespace LexiFlow.Models.Practices
{
    /// <summary>
    /// Entity lưu trữ kết quả bài kiểm tra của học viên
    /// Bao gồm thông tin chi tiết về điểm số, thời gian làm bài, phân tích hiệu suất
    /// Hỗ trợ tracking tiến bộ học tập và so sánh với các user khác
    /// </summary>

    // Tạo chỉ mục để tăng tốc truy vấn theo UserId và ngày test
    [Index(nameof(UserId), nameof(TestDate), Name = "IX_TestResult_User_Date")]

    // Chỉ mục cho việc tìm kiếm theo loại bài test
    [Index(nameof(TestType), Name = "IX_TestResult_Type")]

    // Chỉ mục cho việc sắp xếp và tìm kiếm theo điểm số
    [Index(nameof(Score), Name = "IX_TestResult_Score")]

    // Chỉ mục kết hợp để truy vấn nhanh theo user, loại test và thời gian
    [Index(nameof(UserId), nameof(TestType), nameof(TestDate), Name = "IX_TestResult_User_Type_Date")]

    // Chỉ mục cho soft delete kết hợp với loại test
    [Index(nameof(IsDeleted), nameof(TestType), Name = "IX_TestResult_SoftDelete_Type")]

    // Chỉ mục cho việc tìm kiếm theo cấp độ (N1, N2, Beginner, etc.)
    [Index(nameof(Level), Name = "IX_TestResult_Level")]

    // Chỉ mục cho việc tìm kiếm theo danh mục/chủ đề
    [Index(nameof(CategoryId), Name = "IX_TestResult_Category")]
    public class TestResult : BaseEntity
    {
        /// <summary>
        /// Khóa chính của bảng TestResult
        /// Tự động tăng, duy nhất cho mỗi kết quả test
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestResultId { get; set; }

        /// <summary>
        /// ID của học viên thực hiện bài test
        /// Bắt buộc phải có, liên kết với bảng User
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Ngày giờ thực hiện bài test
        /// Mặc định là thời gian hiện tại (UTC) khi tạo bản ghi
        /// Dùng để tracking lịch sử học tập và thống kê theo thời gian
        /// </summary>
        public DateTime TestDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Loại bài test (ví dụ: "Vocabulary", "Grammar", "Listening", "Reading")
        /// Bắt buộc phải có, tối đa 50 ký tự
        /// Dùng để phân loại và thống kê theo từng skill
        /// </summary>
        [Required]
        public string TestType { get; set; } = SectionTypeExtensions.Listening;

        /// <summary>
        /// Tên cụ thể của bài test
        /// Bắt buộc phải có, tối đa 200 ký tự
        /// Ví dụ: "JLPT N2 Vocabulary Practice", "Daily Grammar Quiz #15"
        /// </summary>
        [Required]
        [StringLength(200)]
        public string TestName { get; set; }

        /// <summary>
        /// Cấp độ của bài test
        /// Tối đa 10 ký tự, có thể null
        /// Ví dụ: "N1", "N2", "N3", "Beginner", "Advanced"
        /// Dùng để phân loại độ khó và thống kê tiến bộ theo level
        /// </summary>
        [StringLength(10)]
        public string Level { get; set; }

        /// <summary>
        /// ID của danh mục/chủ đề mà bài test thuộc về
        /// Có thể null nếu bài test không thuộc danh mục cụ thể nào
        /// Liên kết với bảng Category để biết test thuộc chủ đề gì
        /// </summary>
        public int? CategoryId { get; set; }

        // ===== THÔNG SỐ CỐ BẢN (BASIC METRICS) =====

        /// <summary>
        /// Tổng số câu hỏi trong bài test
        /// Giới hạn từ 1 đến 1000 câu, có thể null
        /// Dùng để tính toán tỷ lệ chính xác và các thống kê khác
        /// </summary>
        [Range(1, 1000)]
        public int? TotalQuestions { get; set; }

        /// <summary>
        /// Số câu trả lời đúng
        /// Giới hạn từ 0 đến 1000, có thể null
        /// Không thể vượt quá TotalQuestions
        /// </summary>
        [Range(0, 1000)]
        public int? CorrectAnswers { get; set; }

        /// <summary>
        /// Điểm số tổng của bài test
        /// Giới hạn từ 0 đến 1000, bắt buộc phải có
        /// Có thể được tính theo công thức phức tạp, không chỉ dựa trên số câu đúng
        /// </summary>
        [Range(0, 1000)]
        public int Score { get; set; }

        /// <summary>
        /// Thời gian làm bài tính bằng giây
        /// Có thể null nếu không giới hạn thời gian
        /// Dùng để phân tích tốc độ làm bài và hiệu suất
        /// </summary>
        public int? DurationSeconds { get; set; }

        // ===== PHÂN TÍCH NÂNG CAO (ADVANCED ANALYTICS) =====

        /// <summary>
        /// Tỷ lệ chính xác tính bằng phần trăm
        /// Kiểu decimal với độ chính xác 5,2 (tối đa 999.99%)
        /// Được tính = (CorrectAnswers / TotalQuestions) * 100
        /// Ví dụ: 85.25% nghĩa là trả lời đúng 85.25% số câu hỏi
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? AccuracyRate { get; set; }

        /// <summary>
        /// Thời gian trả lời trung bình cho mỗi câu hỏi
        /// Tính bằng mili giây (milliseconds)
        /// Được tính = TotalDuration / TotalQuestions
        /// Dùng để đánh giá tốc độ phản xạ và suy nghĩ của học viên
        /// </summary>
        public int? AverageResponseTime { get; set; }

        /// <summary>
        /// Thời gian trả lời lâu nhất cho một câu hỏi
        /// Tính bằng mili giây
        /// Cho biết câu hỏi nào khó nhất hoặc học viên do dự nhất
        /// </summary>
        public int? MaxResponseTime { get; set; }

        /// <summary>
        /// Thời gian trả lời nhanh nhất cho một câu hỏi
        /// Tính bằng mili giây
        /// Có thể cho biết câu hỏi dễ nhất hoặc học viên tự tin nhất
        /// </summary>
        public int? MinResponseTime { get; set; }

        // ===== PHÂN TÍCH HIỆU SUẤT (PERFORMANCE ANALYSIS) =====

        /// <summary>
        /// Các lĩnh vực yếu của học viên
        /// Lưu trữ dưới dạng JSON array chứa danh sách các chủ đề cần cải thiện
        /// Ví dụ: ["Grammar - Particles", "Kanji Reading", "Formal Speech"]
        /// Được AI phân tích dựa trên câu trả lời sai
        /// </summary>
        public string WeakAreas { get; set; }

        /// <summary>
        /// Các lĩnh vực mạnh của học viên
        /// Lưu trữ dưới dạng JSON array chứa danh sách các chủ đề đã thành thạo
        /// Ví dụ: ["Basic Vocabulary", "Numbers", "Time Expression"]
        /// Được AI phân tích dựa trên câu trả lời đúng
        /// </summary>
        public string StrongAreas { get; set; }

        /// <summary>
        /// Các gợi ý cải thiện được tạo bởi AI
        /// Dựa trên kết quả phân tích để đưa ra lời khuyên học tập cụ thể
        /// Ví dụ: "Nên ôn luyện thêm về particles wa và ga", "Luyện tập đọc hiểu nhiều hơn"
        /// Có thể chứa links đến bài học hoặc exercises phù hợp
        /// </summary>
        public string Recommendations { get; set; }

        // ===== THÔNG SỐ SO SÁNH (COMPARISON METRICS) =====

        /// <summary>
        /// Tỷ lệ cải thiện so với lần test trước cùng loại
        /// Kiểu decimal với độ chính xác 5,2 (có thể âm nếu tụt lùi)
        /// Tính = ((ScoreHiện tại - ScoreTrước) / ScoreTrước) * 100
        /// Ví dụ: +15.50% nghĩa là cải thiện 15.5% so với lần trước
        ///        -5.25% nghĩa là giảm 5.25% so với lần trước
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? ImprovementRate { get; set; }

        /// <summary>
        /// Thứ hạng phần trăm so với tất cả users khác cùng level/test type
        /// Giá trị từ 0-100, càng cao càng tốt
        /// Ví dụ: 85 nghĩa là vượt trội hơn 85% users khác
        ///        50 nghĩa là ở mức trung bình
        ///        10 nghĩa là chỉ tốt hơn 10% users (cần cải thiện)
        /// </summary>
        public int? RankPercentile { get; set; }

        // ===== THÔNG TIN PHIÊN LÀM BÀI (SESSION INFO) =====

        /// <summary>
        /// Thông tin User Agent của trình duyệt/ứng dụng
        /// Tối đa 200 ký tự
        /// Dùng để phân tích thiết bị sử dụng và troubleshoot technical issues
        /// Ví dụ: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"
        /// </summary>
        [StringLength(200)]
        public string UserAgent { get; set; }

        /// <summary>
        /// Loại thiết bị được sử dụng để làm bài test
        /// Tối đa 50 ký tự
        /// Ví dụ: "Desktop", "Mobile", "Tablet"
        /// Dùng để phân tích hiệu suất học tập theo từng loại thiết bị
        /// </summary>
        [StringLength(50)]
        public string DeviceType { get; set; }

        // ===== NAVIGATION PROPERTIES (QUAN HỆ DỮ LIỆU) =====

        /// <summary>
        /// Thông tin chi tiết của học viên thực hiện bài test
        /// Quan hệ nhiều-một với bảng User (một user có nhiều test results)
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Thông tin danh mục/chủ đề mà bài test thuộc về
        /// Quan hệ nhiều-một với bảng Category (một category có nhiều test results)
        /// Có thể null nếu test không thuộc category cụ thể
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Danh sách chi tiết từng câu hỏi và câu trả lời trong bài test
        /// Quan hệ một-nhiều với bảng TestDetail
        /// Lưu trữ thông tin: câu hỏi nào, trả lời gì, đúng/sai, thời gian trả lời
        /// </summary>
        public virtual ICollection<TestDetail> TestDetails { get; set; }

        // ===== THUỘC TÍNH TÍNH TOÁN (COMPUTED PROPERTIES) =====

        /// <summary>
        /// Số câu trả lời sai, được tính tự động
        /// = Tổng số câu hỏi - Số câu trả lời đúng
        /// Sử dụng toán tử ?? để xử lý trường hợp null (coi như 0)
        /// </summary>
        public int IncorrectAnswers => (TotalQuestions ?? 0) - (CorrectAnswers ?? 0);

        /// <summary>
        /// Kiểm tra xem học viên có đạt yêu cầu pass bài test không
        /// Tiêu chí: tỷ lệ chính xác >= 60%
        /// Trả về true nếu pass, false nếu fail hoặc chưa có dữ liệu
        /// </summary>
        public bool IsPassed => AccuracyRate >= 60;

        /// <summary>
        /// Xếp loại học lực dựa trên tỷ lệ chính xác
        /// Sử dụng switch expression để phân loại:
        /// A: Xuất sắc (90-100%) - Thành thạo rất tốt
        /// B: Giỏi (80-89%) - Thành thạo tốt
        /// C: Khá (70-79%) - Thành thạo ở mức khá
        /// D: Trung bình (60-69%) - Đạt yêu cầu tối thiểu
        /// F: Yếu (<60%) - Chưa đạt yêu cầu, cần học lại
        /// </summary>
        public string Grade => AccuracyRate switch
        {
            >= 90 => "A",    // Xuất sắc
            >= 80 => "B",    // Giỏi
            >= 70 => "C",    // Khá
            >= 60 => "D",    // Trung bình
            _ => "F"         // Yếu
        };

        // ===== PHƯƠNG THỨC NGHIỆP VỤ (BUSINESS METHODS) =====

        /// <summary>
        /// Tính toán các thông số cơ bản cho bài test
        /// Bao gồm tỷ lệ chính xác dựa trên số câu đúng và tổng số câu
        /// Chỉ thực hiện tính toán khi có đầy đủ dữ liệu (TotalQuestions > 0 và CorrectAnswers có giá trị)
        /// Công thức: AccuracyRate = (CorrectAnswers / TotalQuestions) * 100
        /// </summary>
        public void CalculateMetrics()
        {
            // Kiểm tra điều kiện: phải có ít nhất 1 câu hỏi và có thông tin số câu đúng
            if (TotalQuestions > 0 && CorrectAnswers.HasValue)
            {
                // Tính tỷ lệ chính xác = (số câu đúng / tổng số câu) * 100
                // Ép kiểu về decimal để đảm bảo độ chính xác
                AccuracyRate = (decimal)CorrectAnswers.Value * 100 / TotalQuestions.Value;
            }
        }

        /// <summary>
        /// Cập nhật tỷ lệ cải thiện so với lần test trước cùng loại
        /// Tính toán mức độ tiến bộ hoặc tụt lùi của học viên
        /// </summary>
        /// <param name="previousScore">Tỷ lệ chính xác của lần test trước (có thể null)</param>
        public void UpdateImprovementRate(decimal? previousScore)
        {
            // Chỉ tính toán khi có dữ liệu lần test trước và điểm trước > 0
            if (previousScore.HasValue && previousScore > 0)
            {
                // Công thức tính tỷ lệ cải thiện:
                // ((Điểm hiện tại - Điểm trước) / Điểm trước) * 100
                // 
                // Ví dụ:
                // - Từ 70% lên 84%: ((84-70)/70)*100 = +20% (cải thiện 20%)
                // - Từ 90% xuống 75%: ((75-90)/90)*100 = -16.67% (giảm 16.67%)
                ImprovementRate = ((AccuracyRate ?? 0) - previousScore.Value) / previousScore.Value * 100;
            }
        }
    }
}