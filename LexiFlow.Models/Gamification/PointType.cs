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

namespace LexiFlow.Models.Gamification
{
    /// <summary>
    /// Loại điểm thưởng
    /// </summary>
    [Index(nameof(TypeName), IsUnique = true, Name = "IX_PointType_Name")]
    public class PointType : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PointTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string IconPath { get; set; }

        public int Multiplier { get; set; } = 1;

        // Cải tiến: Màu sắc và hiển thị
        [StringLength(20)]
        public string ColorCode { get; set; }

        // Cải tiến: Hạn chế tích lũy
        public int? DailyLimit { get; set; }
        public int? WeeklyLimit { get; set; }

        // Cải tiến: Quy đổi và sử dụng
        public bool IsSpendable { get; set; } = false;
        public float? ExchangeRate { get; set; } = 1.0f;
        public string SpendableOn { get; set; }

        public bool IsActive { get; set; } = true;

        public int? DisplayOrder { get; set; }

        // Navigation properties
        public virtual ICollection<UserPoint> UserPoints { get; set; }
    }
}
