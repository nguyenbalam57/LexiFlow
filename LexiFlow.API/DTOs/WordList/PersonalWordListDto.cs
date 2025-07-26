using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.WordList
{
    public class PersonalWordListDto
    {
        public int ListID { get; set; }
        public string ListName { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PersonalWordListDetailDto : PersonalWordListDto
    {
        public List<PersonalWordListItemDto> Items { get; set; } = new List<PersonalWordListItemDto>();
    }

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

    public class CreatePersonalWordListDto
    {
        [Required]
        [StringLength(100)]
        public string ListName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }

    public class UpdatePersonalWordListDto
    {
        [Required]
        [StringLength(100)]
        public string ListName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string RowVersionString { get; set; }
    }

    public class AddWordToListDto
    {
        [Required]
        public int VocabularyID { get; set; }
    }

    public class AddMultipleWordsToListDto
    {
        [Required]
        public List<int> VocabularyIDs { get; set; } = new List<int>();
    }
}
