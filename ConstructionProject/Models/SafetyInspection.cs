using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructionProject.Models
{
    public enum ComplianceStatus
    {
        COMPLIANT,
        NON_COMPLIANT
    }

    public class SafetyInspection
    {
        [Key]
        public int InspectionId { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Required]
        public DateTime InspectionDate { get; set; }

        [MaxLength(255)]
        public string? IssuesFound { get; set; }

        public ComplianceStatus ComplianceStatus { get; set; }

        // Navigation property
        public Project? Project { get; set; }
    }
}
