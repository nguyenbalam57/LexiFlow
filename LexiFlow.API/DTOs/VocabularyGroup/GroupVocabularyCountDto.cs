namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// DTO cho số lượng từ vựng trong nhóm
    /// </summary>
    public class GroupVocabularyCountDto
    {
        /// <summary>
        /// ID của nhóm từ vựng
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// Tên nhóm từ vựng
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng từ vựng trong nhóm
        /// </summary>
        public int VocabularyCount { get; set; }
    }
}
