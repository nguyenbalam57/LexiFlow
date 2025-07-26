using System;
using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TechnicalTerm
{
    /// <summary>
    /// DTO cho thuật ngữ kỹ thuật
    /// </summary>
    public class TechnicalTermDto
    {
        public int Id { get; set; }
        public string Term { get; set; }
        public string LanguageCode { get; set; }
        public string Reading { get; set; }
        public string Field { get; set; }
        public string SubField { get; set; }
        public string Abbreviation { get; set; }
        public string Department { get; set; }
        public string Definition { get; set; }
        public string Context { get; set; }
        public string RelatedTerms { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string RowVersionString { get; set; }
        public List<TermExampleDto> Examples { get; set; } = new List<TermExampleDto>();
        public List<TermTranslationDto> Translations { get; set; } = new List<TermTranslationDto>();
    }
}