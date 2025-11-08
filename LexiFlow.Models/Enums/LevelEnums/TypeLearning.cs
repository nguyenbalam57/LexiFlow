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
        [Display(Name = "Từ vựng", ShortName = "Từ")]
        [Description("Học từ vựng và nghĩa của từ (Goi).")]
        [Icon("fa-book")] // Ví dụ dùng class của FontAwesome
        Goi,

        [Display(Name = "Ngữ pháp", ShortName = "Ngữ pháp")]
        [Description("Học các cấu trúc ngữ pháp (Bunpou).")]
        [Icon("fa-sitemap")]
        Bunpou,

        [Display(Name = "Đọc hiểu", ShortName = "Đọc")]
        [Description("Luyện kỹ năng đọc hiểu văn bản (Dokkai).")]
        [Icon("fa-glasses")]
        Dokkai,

        [Display(Name = "Nghe hiểu", ShortName = "Nghe")]
        [Description("Luyện kỹ năng nghe hiểu (Choukai).")]
        [Icon("fa-headphones")]
        Choukai,

        [Display(Name = "Hội thoại", ShortName = "Nói")]
        [Description("Luyện kỹ năng hội thoại (Kaiwa).")]
        [Icon("fa-comments")]
        Kaiwa,

        [Display(Name = "Chữ Hán", ShortName = "Kanji")]
        [Description("Học chữ Hán (Kanji).")]
        [Icon("fa-pen-nib")]
        Kanji
    }
}
