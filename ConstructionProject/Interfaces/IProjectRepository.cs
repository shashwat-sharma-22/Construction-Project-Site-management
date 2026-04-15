using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
        Task AddAsync(Project project);
        Task<Project?> GetByIdAsync(int id);
        void Remove(Project project);
        Task SaveChangesAsync();
    }
}