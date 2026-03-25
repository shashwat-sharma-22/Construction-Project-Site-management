using System.ComponentModel.DataAnnotations;

namespace ConstructionProject.Models
{
    public class ProjectTask
    {
        [Key]
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string? TaskName { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime Deadline { get; set; }
        public TaskStatus Status { get; set; }

        public Project? Project { get; set; }
    }

    public enum TaskStatus { PENDING, IN_PROGRESS, COMPLETED }
}
