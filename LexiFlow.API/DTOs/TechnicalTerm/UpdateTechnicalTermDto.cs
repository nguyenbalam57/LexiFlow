using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TechnicalTerm
{
    /// <summary>
    /// DTO cho cập nhật thuật ngữ kỹ thuật
    /// </summary>
    public class UpdateTechnicalTermDto
    {
        [Required]
        [StringLength(100)]
        public string Term { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = "ja";

        [StringLength(200)]
        public string Reading { get; set; }

        [StringLength(100)]
        public string Field { get; set; }

        [StringLength(100)]
        public string SubField { get; set; }

        [StringLength(50)]
        public string Abbreviation { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        public string Definition { get; set; }

        public string Context { get; set; }

        public string RelatedTerms { get; set; }

        public string Status { get; set; } = "Active";

        public List<TermExampleDto> Examples { get; set; } = new List<TermExampleDto>();

        public List<TermTranslationDto> Translations { get; set; } = new List<TermTranslationDto>();

        [Required]
        public string RowVersionString { get; set; }
    }
}