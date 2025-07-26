namespace LexiFlow.API.DTOs.Kanji
{
    public class UpdateKanjiDto : CreateKanjiDto
    {
        public string RowVersionString { get; set; }
    }
}
