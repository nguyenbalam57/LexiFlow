namespace LexiFlow.API.DTOs.Kanji
{
    /// <summary>
    /// Extension method to support backward compatibility
    /// </summary>
    public static class KanjiExampleExtensions
    {
        /// <summary>
        /// Convert from old-style DTO with Vietnamese and English properties
        /// </summary>
        public static CreateKanjiExampleDto FromLegacyFormat(string japanese, string kana, string vietnamese, string english)
        {
            var dto = new CreateKanjiExampleDto
            {
                Japanese = japanese,
                Kana = kana,
                Meanings = new List<CreateKanjiExampleMeaningDto>()
            };

            if (!string.IsNullOrEmpty(vietnamese))
            {
                dto.Meanings.Add(new CreateKanjiExampleMeaningDto
                {
                    Meaning = vietnamese,
                    Language = "vi",
                    SortOrder = 1
                });
            }

            if (!string.IsNullOrEmpty(english))
            {
                dto.Meanings.Add(new CreateKanjiExampleMeaningDto
                {
                    Meaning = english,
                    Language = "en",
                    SortOrder = 2
                });
            }

            return dto;
        }

        /// <summary>
        /// Get Vietnamese meaning from meanings collection
        /// </summary>
        public static string GetVietnameseMeaning(this KanjiExampleDto example)
        {
            return example.Meanings.FirstOrDefault(m => m.Language == "vi")?.Meaning;
        }

        /// <summary>
        /// Get English meaning from meanings collection
        /// </summary>
        public static string GetEnglishMeaning(this KanjiExampleDto example)
        {
            return example.Meanings.FirstOrDefault(m => m.Language == "en")?.Meaning;
        }
    }
}
