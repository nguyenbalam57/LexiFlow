using System.ComponentModel.DataAnnotations;

namespace LexiFlow.API.DTOs.TechnicalTerm
{
    public class TermRelationDto
    {
        public int Id { get; set; }
        public int TermId1 { get; set; }
        public int TermId2 { get; set; }

        [StringLength(50)]
        public string RelationType { get; set; }

        public string Notes { get; set; }
    }
}
