using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(int projectId);
        Task<IEnumerable<ProjectTask>> SearchTasksAsync(int projectId, string searchTerm);
        Task<ProjectTask?> GetTaskByIdAsync(int id);
        Task<ProjectTask> CreateTaskAsync(ProjectTask task);
        Task<bool> UpdateTaskAsync(int id, ProjectTask updated);
        Task<bool> DeleteTaskAsync(int id);
    }
}
