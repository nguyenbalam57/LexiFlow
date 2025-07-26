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
}
