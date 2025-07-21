using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Entities
{
    /// <summary>
    /// Đối tượng nhóm từ vựng
    /// </summary>
    public class VocabularyGroup
    {
        /// <summary>
        /// ID nhóm
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// Tên nhóm
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ID danh mục
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// ID người tạo
        /// </summary>
        public int? CreatedByUserID { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Thời gian cập nhật
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Danh sách từ vựng trong nhóm
        /// </summary>
        public List<Vocabulary> Vocabularies { get; set; }
    }
}
