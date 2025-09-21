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
    /// Mẫu lặp lại
    /// </summary>
    public class ScheduleRecurrence : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecurrenceId { get; set; }

        [StringLength(50)]
        public string RecurrenceType { get; set; }

        public int? Interval { get; set; }

        [StringLength(20)]
        public string DaysOfWeek { get; set; }

        public int? DayOfMonth { get; set; }

        public int? MonthOfYear { get; set; }

        // Cải tiến: Mẫu lặp phức tạp
        public string CustomPattern { get; set; }
        public bool IncludeWeekends { get; set; } = true;
        public string ExcludeDates { get; set; }
        public string IncludeDates { get; set; }

        // Cải tiến: Tính cách ngày nghỉ lễ
        public bool ExcludeHolidays { get; set; } = false;
        public string HolidayAdjustmentRule { get; set; }

        // Cải tiến: Thứ tự và quy tắc
        public string WeekOfMonth { get; set; } // First, Second, Last
        public string DayPosition { get; set; } // Before, After

        public DateTime? RecurrenceEnd { get; set; }

        public int? MaxOccurrences { get; set; }

        // Cải tiến: Quy tắc ngoại lệ
        public string ExceptionRules { get; set; }
        public string ExceptionDates { get; set; }

        // Navigation properties
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }
}
