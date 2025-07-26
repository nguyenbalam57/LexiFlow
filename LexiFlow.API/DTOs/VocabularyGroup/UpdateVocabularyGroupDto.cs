namespace LexiFlow.API.DTOs.VocabularyGroup
{
    /// <summary>
    /// Data transfer object for updating a vocabulary group
    /// </summary>
    public class UpdateVocabularyGroupDto : CreateVocabularyGroupDto
    {
        /// <summary>
        /// Row version for concurrency checking
        /// </summary>
        public string RowVersionString { get; set; } = string.Empty;

        /// <summary>
        /// Flag indicating if the group is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
