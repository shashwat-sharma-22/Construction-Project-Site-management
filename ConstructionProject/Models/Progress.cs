using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructionProject.Models
{
    public class Progress
    {
        [Key]
        public int ProgressId { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Required]
        public DateTime ReportDate { get; set; }

        public int CompletedTasks { get; set; }

        [MaxLength(255)]
        public string? Remarks { get; set; }

        // Navigation property
        public Project? Project { get; set; }
    }
}
