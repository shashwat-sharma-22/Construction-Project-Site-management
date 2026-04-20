using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
        Task<List<Project>> GetAllWithContractorAsync();
        Task AddAsync(Project project);
        Task<Project?> GetByIdAsync(int id);
        Task<Project?> GetByIdWithContractorAsync(int id);
        void Remove(Project project);
        IQueryable<Project> Query();
        Task SaveChangesAsync();
    }
}