using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Đối tượng từ vựng
    /// </summary>
    public class Vocabulary
    {
        /// <summary>
        /// ID từ vựng
        /// </summary>
        public int VocabularyID { get; set; }

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
        /// ID nhóm từ vựng
        /// </summary>
        public int? GroupID { get; set; }

        /// <summary>
        /// Cấp độ JLPT
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Loại từ
        /// </summary>
        public string PartOfSpeech { get; set; }

        /// <summary>
        /// Đường dẫn file âm thanh
        /// </summary>
        public string AudioFile { get; set; }

        /// <summary>
        /// ID người tạo
        /// </summary>
        public int? CreatedByUserID { get; set; }

        /// <summary>
        /// ID người cập nhật
        /// </summary>
        public int? UpdatedByUserID { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Phiên bản của từ vựng (để kiểm tra xung đột)
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Trạng thái đồng bộ cục bộ
        /// </summary>
        public string SyncStatus { get; set; }
    }
}
