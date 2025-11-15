using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexiFlow.Helpers;

namespace LexiFlow.Models.Enums.LevelEnums
{
    /// <summary>
    /// Các cấp độ của kỳ thi Năng lực Tiếng Nhật (JLPT)
    /// 日本語能力試験レベル (Nihongo Nouryoku Shiken Reberu)
    /// </summary>
    public enum JlptLevel
    {
        /// <summary>
        /// Cấp độ N5 - Sơ cấp (Dễ nhất)
        /// </summary>
        [Display(Name = "N5", ShortName = "Cấp độ N5")]
        [Description("Cấp độ N5 - Sơ cấp (Dễ nhất)")]
        [Icon("🟢")]
        N5,

        /// <summary>
        /// Cấp độ N4 - Sơ cấp
        /// </summary>
        [Display(Name = "N4", ShortName = "Cấp độ N4")]
        [Description("Cấp độ N4 - Sơ cấp")]
        [Icon("🔵")]
        N4,

        /// <summary>
        /// Cấp độ N3 - Trung cấp
        /// </summary>
        [Display(Name = "N3", ShortName = "Cấp độ N3")]
        [Description("Cấp độ N3 - Trung cấp")]
        [Icon("🟠")]
        N3,

        /// <summary>
        /// Cấp độ N2 - Trung thượng cấp
        /// </summary>
        [Display(Name = "N2", ShortName = "Cấp độ N2")]
        [Description("Cấp độ N2 - Trung thượng cấp")]
        [Icon("🔴")]
        N2,

        /// <summary>
        /// Cấp độ N1 - Cao cấp (Khó nhất)
        /// </summary>
        [Display(Name = "N1", ShortName = "Cấp độ N1")]
        [Description("Cấp độ N1 - Cao cấp (Khó nhất)")]
        [Icon("⚫")]
        N1,

        /// <summary>
        /// Cấp độ không xác định
        /// </summary>
        [Display(Name = "Không xác định", ShortName = "Không xác định")]
        [Description("Cấp độ không xác định")]
        [Icon("❔")]
        None = 99,

        /// <summary>
        /// Cấp độ tất cả
        /// </summary>
        [Display(Name = "Tất cả", ShortName = "Tất cả")]
        [Description("Cấp độ tất cả")]
        [Icon("🌈")]
        All = 100,
    }

}
