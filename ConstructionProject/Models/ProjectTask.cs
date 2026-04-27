using System.ComponentModel.DataAnnotations;

namespace ConstructionProject.Models
{
    public class ProjectTask
    {
        [Key]
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string? TaskName { get; set; }
        public int? WorkerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        public TaskStatus Status { get; set; }

        public Project? Project { get; set; }
        public Workforce? Worker { get; set; }
    }

    public enum TaskStatus { PENDING, IN_PROGRESS, COMPLETED }
}
