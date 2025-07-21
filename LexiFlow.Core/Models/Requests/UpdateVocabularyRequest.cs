using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models.Requests
{
    /// <summary>
    /// Yêu cầu cập nhật từ vựng
    /// </summary>
    public class UpdateVocabularyRequest
    {
        /// <summary>
        /// Từ tiếng Nhật
        /// </summary>
        public string Japanese { get; set; }

        /// <summary>
        /// Phiên âm Kana
        /// </summary>
        public string Kana { get; set; }

        /// <summary>
        /// Phiên âm Romaji
        /// </summary>
        public string Romaji { get; set; }

        /// <summary>
        /// Nghĩa tiếng Việt
        /// </summary>
        public string Vietnamese { get; set; }

        /// <summary>
        /// Nghĩa tiếng Anh
        /// </summary>
        public string English { get; set; }

        /// <summary>
        /// Ví dụ
        /// </summary>
        public string Example { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Cấp độ JLPT
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Nhóm từ vựng
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Loại từ
        /// </summary>
        public string PartOfSpeech { get; set; }
    }
}
