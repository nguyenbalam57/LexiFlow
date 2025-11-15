using LexiFlow.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Models.Enums.LevelEnums
{
    /// <summary>
    /// Phân loại (danh mục) học tập tiếng Nhật
    /// </summary>
    public enum TypeLearning
    {
        /// <summary>
        /// Học từ vựng (Goi)
        /// </summary>
        [Display(Name = "Từ vựng", ShortName = "Từ")]
        [Description("Học từ vựng và nghĩa của từ (Goi).")]
        [Icon("fa-book")] // Ví dụ dùng class của FontAwesome
        Goi = 0,

        /// <summary>
        /// Học ngữ pháp (Bunpou)
        /// </summary>
        [Display(Name = "Ngữ pháp", ShortName = "Ngữ pháp")]
        [Description("Học các cấu trúc ngữ pháp (Bunpou).")]
        [Icon("fa-sitemap")]
        Bunpou = 1,

        /// <summary>
        /// Luyện kỹ năng đọc hiểu (Dokkai)
        /// </summary>
        [Display(Name = "Đọc hiểu", ShortName = "Đọc")]
        [Description("Luyện kỹ năng đọc hiểu văn bản (Dokkai).")]
        [Icon("fa-glasses")]
        Dokkai = 2,

        /// <summary>
        /// Luyện kỹ năng nghe hiểu (Choukai)
        /// </summary>
        [Display(Name = "Nghe hiểu", ShortName = "Nghe")]
        [Description("Luyện kỹ năng nghe hiểu (Choukai).")]
        [Icon("fa-headphones")]
        Choukai = 3,

        /// <summary>
        /// Luyện kỹ năng hội thoại (Kaiwa)
        /// </summary>  
        [Display(Name = "Hội thoại", ShortName = "Nói")]
        [Description("Luyện kỹ năng hội thoại (Kaiwa).")]
        [Icon("fa-comments")]
        Kaiwa = 4,

        /// <summary>
        /// Học chữ Hán (Kanji)
        /// </summary>
        [Display(Name = "Chữ Hán", ShortName = "Kanji")]
        [Description("Học chữ Hán (Kanji).")]
        [Icon("fa-pen-nib")]
        Kanji = 5,

        /// <summary>
        /// Không thuộc loại nào cả
        /// </summary>
        [Display(Name = "Không xác định", ShortName = "Không")]
        [Description("Không thuộc loại học tập nào.")]
        [Icon("fa-question-circle")]
        None = 99,

        /// <summary>
        /// Trạng thái All - Bao gồm tất cả các loại học tập
        /// </summary>
        [Display(Name = "Tất cả", ShortName = "Tất cả")]
        [Description("Bao gồm tất cả các loại học tập.")]
        [Icon("fa-layer-group")]
        All = 100,
    }
}
