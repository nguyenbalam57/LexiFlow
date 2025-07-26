using LexiFlow.API.DTOs.Kanji;
using LexiFlow.API.DTOs.Learning;

namespace LexiFlow.API.DTOs.Vocabulary
{
    /// <summary>
    /// DTO cho từ vựng chi tiết (bao gồm thông tin về kanji và ngữ pháp liên quan)
    /// </summary>
    public class VocabularyDetailDto
    {
        /// <summary>
        /// Thông tin cơ bản về từ vựng
        /// </summary>
        public VocabularyDto Vocabulary { get; set; }

        /// <summary>
        /// Danh sách kanji trong từ vựng
        /// </summary>
        public List<KanjiReferenceDto> Kanjis { get; set; } = new List<KanjiReferenceDto>();

        /// <summary>
        /// Danh sách ngữ pháp liên quan
        /// </summary>
        public List<GrammarReferenceDto> RelatedGrammar { get; set; } = new List<GrammarReferenceDto>();

        /// <summary>
        /// Các từ đồng nghĩa
        /// </summary>
        public List<VocabularyReferenceDto> Synonyms { get; set; } = new List<VocabularyReferenceDto>();

        /// <summary>
        /// Các từ trái nghĩa
        /// </summary>
        public List<VocabularyReferenceDto> Antonyms { get; set; } = new List<VocabularyReferenceDto>();

        /// <summary>
        /// Tiến độ học của người dùng
        /// </summary>
        public LearningProgressDto UserProgress { get; set; }
    }
}
