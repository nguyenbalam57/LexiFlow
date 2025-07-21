using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexiFlow.Core.Models.Requests
{
    /// <summary>
    /// Yêu cầu tạo từ vựng mới
    /// </summary>
    public class CreateVocabularyRequest
    {
        public string Japanese { get; set; } = string.Empty;
        public string? Kana { get; set; }
        public string? Romaji { get; set; }
        public string? Vietnamese { get; set; }
        public string? English { get; set; }
        public string? Example { get; set; }
        public string? Notes { get; set; }
        public string? Level { get; set; }
        public string? PartOfSpeech { get; set; }
        public int? GroupId { get; set; }
    }
}
