using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IProgressRepository
    {
        Task<List<Progress>> GetAllWithProjectAsync();
        Task AddAsync(Progress progress);
        Task<Progress?> GetByIdAsync(int id);
        Task<Progress?> GetByIdWithProjectAsync(int id);
        void Remove(Progress progress);
        IQueryable<Progress> Query();
        Task SaveChangesAsync();
    }
}