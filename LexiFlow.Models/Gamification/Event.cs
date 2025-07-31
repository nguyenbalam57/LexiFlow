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
    /// Sự kiện trong hệ thống gamification
    /// </summary>
    [Index(nameof(EventName), IsUnique = true, Name = "IX_Event_Name")]
    public class Event : BaseEntity, IActivatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string EventType { get; set; }

        [StringLength(50)]
        public string RewardType { get; set; }

        public int? RewardValue { get; set; }

        public int? BadgeId { get; set; }

        // Cải tiến: Thông tin hiển thị
        [StringLength(255)]
        public string BannerImageUrl { get; set; }
        [StringLength(255)]
        public string EventLogoUrl { get; set; }
        [StringLength(50)]
        public string ThemeColor { get; set; }

        // Cải tiến: Nội dung sự kiện
        public string EventRules { get; set; }
        public string EventSchedule { get; set; }
        public string EventTiers { get; set; }

        // Cải tiến: Thông tin quản lý
        public int? OrganizerUserId { get; set; }
        public string SponsorsInfo { get; set; }
        public string ExternalLinks { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string ParticipationType { get; set; }

        public int? MaxParticipants { get; set; }

        // Cải tiến: Cấu hình sự kiện
        public bool RequiresRegistration { get; set; } = false;
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public int? MinimumLevel { get; set; }

        // Navigation properties
        [ForeignKey("BadgeId")]
        public virtual Badge Badge { get; set; }

        [ForeignKey("OrganizerUserId")]
        public virtual User.User OrganizerUser { get; set; }

        public virtual ICollection<UserEvent> UserEvents { get; set; }
        public virtual ICollection<Challenge> Challenges { get; set; }
        public virtual ICollection<Leaderboard> Leaderboards { get; set; }
        public virtual ICollection<DailyTask> DailyTasks { get; set; }
    }
}
