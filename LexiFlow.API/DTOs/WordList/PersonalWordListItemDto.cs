namespace LexiFlow.API.DTOs.WordList
{
    public class PersonalWordListItemDto
    {
        public int ItemID { get; set; }
        public int ListID { get; set; }
        public int VocabularyID { get; set; }
        public string Japanese { get; set; }
        public string Kana { get; set; }
        public string Vietnamese { get; set; }
        public string English { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
