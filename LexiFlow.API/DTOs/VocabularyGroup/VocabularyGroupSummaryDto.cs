namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// Data transfer object for vocabulary group summary
    /// </summary>
    public class VocabularyGroupSummaryDto
    {
        /// <summary>
        /// Group ID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// Name of the vocabulary group
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// Description of the vocabulary group
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Category ID this group belongs to
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Number of vocabulary items in this group
        /// </summary>
        public int VocabularyCount { get; set; }

        /// <summary>
        /// Flag indicating if the group is active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
