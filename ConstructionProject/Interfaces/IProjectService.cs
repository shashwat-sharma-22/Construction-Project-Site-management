using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<IEnumerable<Project>> SearchProjectsAsync(string searchTerm);
        Task<IEnumerable<Project>> GetProjectsByContractorAsync(int contractorId);
        Task<IEnumerable<Project>> SearchProjectsByContractorAsync(int contractorId, string searchTerm);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project?> GetProjectDetailsAsync(int id);
        Task<bool> UpdateProjectPlanAsync(int id, Project updated);
        Task<bool> DeleteProjectAsync(int id);
    }
}