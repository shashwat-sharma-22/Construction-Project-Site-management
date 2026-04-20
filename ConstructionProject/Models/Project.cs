using System.ComponentModel.DataAnnotations;

namespace ConstructionProject.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        //[Required]
        public string? ProjectName { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? startDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? endDate { get; set; }

        public decimal budget { get; set; }

        public int? ContractorId { get; set; }
        public Contractor? Contractor { get; set; }
    }
}
