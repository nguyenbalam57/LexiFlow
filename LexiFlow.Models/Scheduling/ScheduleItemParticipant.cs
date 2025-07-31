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
    /// Người tham gia mục lịch trình
    /// </summary>
    [Index(nameof(ItemId), nameof(UserId), IsUnique = true, Name = "IX_ScheduleItemParticipant_Item_User")]
    public class ScheduleItemParticipant : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantId { get; set; }

        [Required]
        public int ItemId { get; set; }

        public int? UserId { get; set; }

        public int? GroupId { get; set; }

        // Cải tiến: Vai trò và trạng thái
        [StringLength(50)]
        public string ParticipantRole { get; set; }

        [StringLength(50)]
        public string AttendanceStatus { get; set; } // Invited, Confirmed, Attended, Absent

        public int? ResponseStatus { get; set; } // 0: None, 1: Accepted, 2: Tentative, 3: Declined

        // Cải tiến: Theo dõi tham gia
        public DateTime? JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public int? AttendanceMinutes { get; set; }
        public bool IsLate { get; set; } = false;

        // Cải tiến: Thông tin bổ sung
        public string Notes { get; set; }
        public string AttendanceCode { get; set; }
        public string Feedback { get; set; }

        // Cải tiến: Ghi chú của người tham gia
        public string ParticipantNotes { get; set; }
        public DateTime? ResponseTime { get; set; }
        public string ResponseReason { get; set; }

        // Navigation properties
        [ForeignKey("ItemId")]
        public virtual ScheduleItem Item { get; set; }

        [ForeignKey("UserId")]
        public virtual User.User User { get; set; }

        [ForeignKey("GroupId")]
        public virtual User.Group Group { get; set; }
    }
}
