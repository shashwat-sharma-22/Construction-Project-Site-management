using System.ComponentModel.DataAnnotations;

namespace ConstructionProject.Models
{
    public class Workforce
    {
        [Key]
        public int WorkerId { get; set; }
        public int ContractorId { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }

        public Contractor? Contractor { get; set; }
    }
}
