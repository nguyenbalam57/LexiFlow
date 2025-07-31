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

namespace LexiFlow.Models.System
{
    /// <summary>
    /// Cài đặt hệ thống
    /// </summary>
    [Index(nameof(SettingKey), IsUnique = true, Name = "IX_Setting_Key")]
    public class Setting : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }

        [Required]
        [StringLength(100)]
        public string SettingKey { get; set; }

        public string SettingValue { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Group { get; set; }

        // Cải tiến: Loại dữ liệu và validation
        [StringLength(50)]
        public string DataType { get; set; } // String, Int, Boolean, JSON, Date

        public string ValidationRules { get; set; }

        public string DefaultValue { get; set; }

        // Cải tiến: Hiển thị và quản lý
        public bool IsEditable { get; set; } = true;

        public bool IsVisible { get; set; } = true;

        public bool RequiresRestart { get; set; } = false;

        public int DisplayOrder { get; set; } = 0;

        // Cải tiến: Phân quyền
        public string AccessRoles { get; set; }
        public bool IsGlobal { get; set; } = true;
        public bool IsUserOverridable { get; set; } = false;

        // Cải tiến: Quản lý phiên bản
        public string VersionAdded { get; set; }
        public string VersionDeprecated { get; set; }
    }
}
