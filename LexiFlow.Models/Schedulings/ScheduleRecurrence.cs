using LexiFlow.Models.Cores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Schedulings
{
    /// <summary>
    /// Mẫu lặp lại lịch trình - Định nghĩa các quy tắc lặp lại cho các mục lịch trình
    /// Hỗ trợ tạo ra chuỗi sự kiện định kỳ với các mẫu phức tạp và quy tắc ngoại lệ
    /// </summary>
    public class ScheduleRecurrence : BaseEntity
    {
        /// <summary>
        /// Mã định danh duy nhất cho mẫu lặp lại
        /// Sử dụng để phân biệt từng quy tắc lặp lại trong hệ thống
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Loại mẫu lặp lại (Daily, Weekly, Monthly, Yearly, Custom)
        /// Xác định cơ bản cách thức lặp lại của sự kiện
        /// </summary>
        [StringLength(50)]
        public string RecurrenceType { get; set; }

        /// <summary>
        /// Khoảng cách lặp lại (ví dụ: mỗi 2 ngày, mỗi 3 tuần)
        /// Số lần lặp lại giữa các sự kiện liên tiếp
        /// </summary>
        public int? Interval { get; set; }

        /// <summary>
        /// Các ngày trong tuần lặp lại (dạng chuỗi bit hoặc danh sách)
        /// Áp dụng cho mẫu lặp lại hàng tuần (ví dụ: "1,3,5" cho thứ 2,4,6)
        /// </summary>
        [StringLength(20)]
        public string DaysOfWeek { get; set; }

        /// <summary>
        /// Ngày trong tháng lặp lại (1-31)
        /// Áp dụng cho mẫu lặp lại hàng tháng
        /// </summary>
        public int? DayOfMonth { get; set; }

        /// <summary>
        /// Tháng trong năm lặp lại (1-12)
        /// Áp dụng cho mẫu lặp lại hàng năm
        /// </summary>
        public int? MonthOfYear { get; set; }

        // Cải tiến: Mẫu lặp phức tạp
        /// <summary>
        /// Mẫu lặp lại tùy chỉnh (chuỗi JSON hoặc biểu thức CRON)
        /// Hỗ trợ các quy tắc lặp lại phức tạp không thể định nghĩa bằng các trường cơ bản
        /// </summary>
        public string CustomPattern { get; set; }

        /// <summary>
        /// Bao gồm các ngày cuối tuần trong mẫu lặp lại hay không
        /// True: Bao gồm, False: Bỏ qua các ngày thứ 7 và Chủ nhật
        /// </summary>
        public bool IncludeWeekends { get; set; } = true;

        /// <summary>
        /// Danh sách các ngày loại trừ khỏi mẫu lặp lại (dạng JSON array)
        /// Các ngày cụ thể không tạo sự kiện mặc dù phù hợp với quy tắc
        /// </summary>
        public string ExcludeDates { get; set; }

        /// <summary>
        /// Danh sách các ngày bổ sung vào mẫu lặp lại (dạng JSON array)
        /// Các ngày cụ thể tạo sự kiện ngay cả khi không phù hợp với quy tắc chính
        /// </summary>
        public string IncludeDates { get; set; }

        // Cải tiến: Tính cách ngày nghỉ lễ
        /// <summary>
        /// Loại trừ các ngày nghỉ lễ khỏi mẫu lặp lại
        /// True: Không tạo sự kiện vào các ngày nghỉ lễ, False: Tạo bình thường
        /// </summary>
        public bool ExcludeHolidays { get; set; } = false;

        /// <summary>
        /// Quy tắc điều chỉnh khi ngày lặp lại trùng với ngày nghỉ lễ
        /// Ví dụ: "NextBusinessDay" - chuyển sang ngày làm việc tiếp theo
        /// </summary>
        public string HolidayAdjustmentRule { get; set; }

        // Cải tiến: Thứ tự và quy tắc
        /// <summary>
        /// Tuần trong tháng cho mẫu lặp lại (First, Second, Third, Fourth, Last)
        /// Áp dụng cho các mẫu như "thứ 2 tuần đầu tiên của tháng"
        /// </summary>
        public string WeekOfMonth { get; set; }

        /// <summary>
        /// Vị trí ngày trong tuần (Before, After) - cho các quy tắc phức tạp
        /// Hỗ trợ các mẫu như "ngày làm việc trước ngày nghỉ lễ"
        /// </summary>
        public string DayPosition { get; set; }

        /// <summary>
        /// Thời điểm kết thúc mẫu lặp lại
        /// Sau ngày này sẽ không tạo thêm sự kiện lặp lại
        /// </summary>
        public DateTime? RecurrenceEnd { get; set; }

        /// <summary>
        /// Số lần xuất hiện tối đa của mẫu lặp lại
        /// Giới hạn số lượng sự kiện được tạo ra
        /// </summary>
        public int? MaxOccurrences { get; set; }

        // Cải tiến: Quy tắc ngoại lệ
        /// <summary>
        /// Các quy tắc ngoại lệ cho mẫu lặp lại (dạng JSON)
        /// Định nghĩa các thay đổi đặc biệt cho một số sự kiện trong chuỗi
        /// </summary>
        public string ExceptionRules { get; set; }

        /// <summary>
        /// Danh sách các ngày ngoại lệ (dạng JSON array)
        /// Các ngày có quy tắc riêng biệt không tuân theo mẫu chính
        /// </summary>
        public string ExceptionDates { get; set; }

        // Navigation properties
        /// <summary>
        /// Danh sách các mục lịch trình sử dụng mẫu lặp lại này
        /// Liên kết ngược để truy cập tất cả sự kiện được tạo từ mẫu này
        /// </summary>
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }
}
