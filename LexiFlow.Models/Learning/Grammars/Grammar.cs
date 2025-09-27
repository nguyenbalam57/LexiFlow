using LexiFlow.Models.Cores;
using LexiFlow.Models.Learning.Commons;
using LexiFlow.Models.Medias;
using LexiFlow.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexiFlow.Models.Learning.Grammars
{
    /// <summary>
    /// Model đại diện cho điểm ngữ pháp trong hệ thống học tập LexiFlow.
    /// Quản lý thông tin chi tiết về các mẫu câu, quy tắc ngữ pháp và cách sử dụng trong tiếng Nhật.
    /// </summary>
    /// <remarks>
    /// Entity này hỗ trợ:
    /// - Lưu trữ các pattern ngữ pháp với đầy đủ thông tin phiên âm và cấp độ
    /// - Phân loại theo loại ngữ pháp, dạng thức và thì của động từ
    /// - Quản lý mối quan hệ phân cấp giữa các điểm ngữ pháp
    /// - Cung cấp quy tắc cấu tạo, cách dùng và lỗi thường gặp
    /// - Tích hợp với hệ thống ví dụ, định nghĩa và dịch thuật
    /// - Theo dõi tiến độ học tập của người dùng
    /// </remarks>
    [Index(nameof(Pattern), IsUnique = true, Name = "IX_Grammar_Pattern")]
    [Index(nameof(Level), Name = "IX_Grammar_Level")]
    public class Grammar : BaseLearning
    {
        /// <summary>
        /// Khóa chính của bảng grammar
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GrammarId { get; set; }

        #region Thông tin cơ bản

        /// <summary>
        /// Mẫu câu ngữ pháp chính (duy nhất trong hệ thống)
        /// </summary>
        /// <value>
        /// Pattern ngữ pháp tiếng Nhật như:
        /// - "Nは Aです" (N là A)
        /// - "Vてください" (Làm ơn hãy V)
        /// - "Nがほしいです" (Muốn có N)
        /// - "Vたことがあります" (Đã từng V)
        /// Tối đa 255 ký tự, không được trùng lặp
        /// </value>
        [Required]
        [StringLength(255)]
        public string Pattern { get; set; }

        /// <summary>
        /// Cách đọc của pattern ngữ pháp bằng hiragana/katakana
        /// </summary>
        /// <value>
        /// Phiên âm tiếng Nhật của pattern, giúp người học biết cách phát âm
        /// Ví dụ: "Nは Aです" → "エヌは エーです"
        /// Tối đa 200 ký tự
        /// </value>
        [StringLength(200)]
        public string Reading { get; set; }

        /// <summary>
        /// Cấp độ JLPT của điểm ngữ pháp này
        /// </summary>
        /// <value>
        /// Các cấp độ JLPT hợp lệ:
        /// - "N5": Cơ bản nhất
        /// - "N4": Sơ cấp
        /// - "N3": Trung cấp thấp
        /// - "N2": Trung cấp cao
        /// - "N1": Cao cấp
        /// Tối đa 100 ký tự
        /// </value>
        [StringLength(100)]
        public string Level { get; set; }

        /// <summary>
        /// ID của category để phân nhóm các điểm ngữ pháp
        /// </summary>
        /// <value>
        /// Null nếu không thuộc category nào.
        /// Dùng để nhóm theo chủ đề như: "Thì", "Điều kiện", "Kính ngữ"
        /// </value>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Mức độ khó của điểm ngữ pháp theo đánh giá chủ quan
        /// </summary>
        /// <value>
        /// Thang điểm từ 1-5:
        /// - 1: Rất dễ
        /// - 2: Dễ  
        /// - 3: Trung bình (mặc định)
        /// - 4: Khó
        /// - 5: Rất khó
        /// </value>
        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3;

        #endregion

        #region Phân loại và cấu trúc ngữ pháp

        /// <summary>
        /// Loại ngữ pháp theo chức năng và cấu trúc
        /// </summary>
        /// <value>
        /// Các loại ngữ pháp phổ biến:
        /// - "Particle": Trợ từ (は, が, を, に, で, v.v.)
        /// - "Conjugation": Chia động từ, tính từ
        /// - "Sentence Pattern": Mẫu câu hoàn chỉnh
        /// - "Expression": Thành ngữ, cách diễn đạt
        /// - "Auxiliary": Động từ phụ trợ
        /// Tối đa 50 ký tự
        /// </value>
        [StringLength(50)]
        public string GrammarType { get; set; }

        /// <summary>
        /// Dạng thức của ngữ pháp (mức độ lịch sự)
        /// </summary>
        /// <value>
        /// Các dạng thức hợp lệ:
        /// - "Casual": Dạng thường (だ/である)
        /// - "Formal": Dạng lịch sự (です/ます)
        /// - "Polite": Dạng tôn kính (keigo)
        /// - "Humble": Dạng khiêm tốn (kenjougo)
        /// Tối đa 50 ký tự
        /// </value>
        [StringLength(50)]
        public string FormType { get; set; }

        /// <summary>
        /// Điểm ngữ pháp này có dạng phủ định không
        /// </summary>
        /// <value>
        /// - True: Là dạng phủ định (ない, ません, じゃない)
        /// - False: Là dạng khẳng định
        /// Mặc định: false
        /// </value>
        public bool IsNegative { get; set; } = false;

        /// <summary>
        /// Điểm ngữ pháp này có thuộc thì quá khứ không
        /// </summary>
        /// <value>
        /// - True: Thì quá khứ (た, でした, だった)
        /// - False: Thì hiện tại/tương lai
        /// Mặc định: false
        /// </value>
        public bool IsPast { get; set; } = false;

        /// <summary>
        /// Mã ngôn ngữ của pattern ngữ pháp
        /// </summary>
        /// <value>
        /// Code chuẩn ISO 639-1:
        /// - "ja": Tiếng Nhật (mặc định)
        /// - "en": Tiếng Anh
        /// - "vi": Tiếng Việt
        /// Mặc định: "ja"
        /// </value>
        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja"; // Ngôn ngữ định nghĩa

        #endregion

        #region Quy tắc và cách sử dụng

        /// <summary>
        /// Quy tắc cấu tạo chi tiết của điểm ngữ pháp
        /// </summary>
        /// <value>
        /// Mô tả cách tạo thành pattern này từ các thành phần cơ bản:
        /// - Cách chia động từ, tính từ
        /// - Thứ tự sắp xếp các thành phần
        /// - Quy tắc biến đổi khi kết hợp
        /// Ví dụ: "Động từ dạng te + ください"
        /// </value>
        public string FormationRules { get; set; }

        /// <summary>
        /// Ghi chú chi tiết về cách sử dụng trong ngữ cảnh thực tế
        /// </summary>
        /// <value>
        /// Thông tin về:
        /// - Khi nào sử dụng pattern này
        /// - Với ai có thể dùng (bạn bè, sếp, người lạ)
        /// - Trong tình huống nào (chính thức, thân mật)
        /// - Sắc thái ý nghĩa đặc biệt
        /// </value>
        public string UsageNotes { get; set; }

        /// <summary>
        /// Những lỗi thường gặp khi sử dụng điểm ngữ pháp này
        /// </summary>
        /// <value>
        /// Danh sách các lỗi phổ biến của học viên:
        /// - Nhầm lẫn với pattern tương tự
        /// - Sử dụng sai ngữ cảnh
        /// - Chia động từ không chính xác
        /// - Thiếu hoặc thừa trợ từ
        /// Kèm theo ví dụ sai và cách sửa
        /// </value>
        public string CommonMistakes { get; set; }

        #endregion

        #region Mối quan hệ và phân cấp

        /// <summary>
        /// Các pattern ngữ pháp có liên quan đến pattern này
        /// </summary>
        /// <value>
        /// Chuỗi IDs của các Grammar khác phân cách bởi dấu phẩy
        /// Hoặc chuỗi patterns có liên quan để tham khảo cross-reference
        /// Giúp học viên hiểu mối liên hệ giữa các điểm ngữ pháp
        /// </value>
        public string RelatedPatterns { get; set; }

        /// <summary>
        /// ID của điểm ngữ pháp cha trong cấu trúc phân cấp
        /// </summary>
        /// <value>
        /// Null nếu đây là ngữ pháp gốc.
        /// Có giá trị nếu ngữ pháp này là biến thể/mở rộng của ngữ pháp khác
        /// Ví dụ: "Vています" là con của "Vます"
        /// </value>
        public int? ParentGrammarId { get; set; }

        #endregion

        #region Hỗ trợ học tập

        /// <summary>
        /// Cách nhớ ngắn gọn hoặc công thức nhanh
        /// </summary>
        /// <value>
        /// Chuỗi ngắn gọn giúp nhớ nhanh pattern:
        /// - "N wa A desu" cho "Nは Aです"
        /// - "V-te kudasai" cho yêu cầu lịch sự
        /// - "N ga hoshii" cho mong muốn
        /// </value>
        public string Shortcut { get; set; }

        /// <summary>
        /// Mẹo ghi nhớ bằng liên tưởng hoặc câu chuyện
        /// </summary>
        /// <value>
        /// Câu chuyện, hình ảnh hoặc liên tưởng giúp nhớ lâu:
        /// - "Hình dung は như cái cầu nối chủ ngữ với vị ngữ"
        /// - "て-form giống như bắt tay, nối các hành động"
        /// Tối đa 255 ký tự
        /// </value>
        [StringLength(255)]
        public string Mnemonic { get; set; }

        /// <summary>
        /// Ghi chú bổ sung của giáo viên hoặc admin
        /// </summary>
        /// <value>
        /// Thông tin thêm không thuộc các trường khác:
        /// - Nguồn gốc lịch sử của pattern
        /// - Xu hướng sử dụng hiện đại
        /// - Ghi chú cho giáo viên
        /// </value>
        public string Notes { get; set; }

        /// <summary>
        /// Các thẻ tag để phân loại và tìm kiếm
        /// </summary>
        /// <value>
        /// Chuỗi tags phân cách bởi dấu phẩy như:
        /// "beginner,particle,subject-marker,essential"
        /// Giúp filter và search nhanh chóng
        /// </value>
        public string Tags { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// Thông tin category mà điểm ngữ pháp này thuộc về
        /// </summary>
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Thông tin điểm ngữ pháp cha (nếu có)
        /// </summary>
        [ForeignKey("ParentGrammarId")]
        public virtual Grammar ParentGrammar { get; set; }

        /// <summary>
        /// Danh sách các điểm ngữ pháp con (biến thể, mở rộng)
        /// </summary>
        /// <value>Collection các Grammar có ParentGrammarId trùng với Id này</value>
        public virtual ICollection<Grammar> ChildGrammars { get; set; }

        /// <summary>
        /// Danh sách các định nghĩa chi tiết của điểm ngữ pháp
        /// </summary>
        /// <value>Collection chứa định nghĩa bằng nhiều ngôn ngữ khác nhau</value>
        public virtual ICollection<Definition> Definitions { get; set; }

        /// <summary>
        /// Danh sách các ví dụ minh họa cách sử dụng
        /// </summary>
        /// <value>Collection chứa câu ví dụ với bản dịch và giải thích</value>
        public virtual ICollection<Example> Examples { get; set; }

        /// <summary>
        /// Danh sách các bản dịch của pattern sang ngôn ngữ khác
        /// </summary>
        /// <value>Collection chứa bản dịch sang tiếng Việt, Anh, v.v.</value>
        public virtual ICollection<Translation> Translations { get; set; }

        /// <summary>
        /// Danh sách file media (âm thanh, hình ảnh) liên quan
        /// </summary>
        /// <value>Collection chứa file phát âm, hình ảnh minh họa, v.v.</value>
        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        /// <summary>
        /// Danh sách tiến độ học tập của các user đối với điểm ngữ pháp này
        /// </summary>
        /// <value>Collection theo dõi mức độ thành thạo của từng học viên</value>
        public virtual ICollection<UserGrammarProgress> UserProgresses { get; set; }

        #endregion
    }
}