using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LexiFlow.Models.Learning.Commons;


namespace LexiFlow.Models.Learning.Vocabularys
{
    public class VocabularyCategory
    {
        /*
         * Bảng liên kết nhiều-nhiều giữa từ vựng và danh mục
         * Một từ vựng có thể thuộc nhiều danh mục
         * Một danh mục có thể chứa nhiều từ vựng
         */
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VocabularyCategoryId { get; set; }
        
        /// <summary>
        /// ID của từ vựng
        /// </summary>
        public int VocabularyId { get; set; }

        /// <summary>
        /// Từ vựng liên quan
        /// Virtual để hỗ trợ lazy loading
        /// Có nghĩa là khi truy cập thuộc tính này, 
        /// Entity Framework sẽ tự động tải dữ liệu từ cơ sở dữ liệu nếu chưa được tải
        /// </summary>
        [ForeignKey("VocabularyId")]
        public virtual Vocabulary Vocabulary { get; set; }

        /// <summary>
        /// ID của danh mục
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Danh mục liên quan
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}