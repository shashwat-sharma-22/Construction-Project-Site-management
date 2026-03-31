using System.ComponentModel.DataAnnotations;

namespace ConstructionProject.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        
        public string? ProjectName { get; set; }
        
        public DateTime? startDate { get; set; }

        public DateTime? endDate { get; set; }

        public decimal budget { get; set; }

    }
}
