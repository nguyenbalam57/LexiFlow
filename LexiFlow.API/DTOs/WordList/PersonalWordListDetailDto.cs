namespace LexiFlow.API.DTOs.WordList
{
    public class PersonalWordListDetailDto : PersonalWordListDto
    {
        public List<PersonalWordListItemDto> Items { get; set; } = new List<PersonalWordListItemDto>();
    }
}
