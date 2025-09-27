using System.ComponentModel.DataAnnotations;

namespace LexiFlow.TranslationAPI.Models
{
    public class TranslationRequest
    {
        [Required(ErrorMessage = "Text is required")]
        [StringLength(1000, ErrorMessage = "Text cannot exceed 1000 characters")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Source language is required")]
        [StringLength(5, ErrorMessage = "Invalid language code")]
        public string SourceLang { get; set; } = "en";

        [Required(ErrorMessage = "Target language is required")]
        [StringLength(5, ErrorMessage = "Invalid language code")]
        public string TargetLang { get; set; } = "vi";
    }
}
