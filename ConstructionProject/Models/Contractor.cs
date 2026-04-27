using System.ComponentModel.DataAnnotations;

namespace ConstructionProject.Models
{
    public class Contractor
    {
        [Key]
        public int ContractorId { get; set; }
        public string? ContractorName { get; set; }
        public string? Specialization { get; set; }
        public string? ContactInfo { get; set; }
        public bool IsAssigned { get; set; } = false;

        public ICollection<Workforce>? Workforces { get; set; }
    }
}
