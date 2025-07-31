using LexiFlow.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Scheduling
{
    /// <summary>
    /// Loại mục lịch trình
    /// </summary>
    [Index(nameof(TypeName), IsUnique = true, Name = "IX_ScheduleItemType_Name")]
    public class ScheduleItemType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        [StringLength(20)]
        public string DefaultColor { get; set; }

        // Cải tiến: Cấu hình hiển thị
        [StringLength(255)]
        public string IconClass { get; set; }

        public int? DefaultDurationMinutes { get; set; }

        public string DefaultReminders { get; set; }

        // Cải tiến: Quy tắc xử lý
        public bool AllowOverlap { get; set; } = true;
        public bool RequiresApproval { get; set; } = false;
        public string CompletionRules { get; set; }

        // Cải tiến: Mẫu nội dung
        public string TemplateTitle { get; set; }
        public string TemplateDescription { get; set; }
        public string TemplateFields { get; set; }

        // Navigation properties
        public virtual ICollection<ScheduleItem> ScheduleItems { get; set; }
    }
}
