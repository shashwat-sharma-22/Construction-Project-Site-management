using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface ISafetyRepository
    {
        Task<bool> ProjectExistsAsync(int projectId);
        Task AddAsync(SafetyInspection inspection);
        Task<SafetyInspection?> GetByIdAsync(int id);
        Task<SafetyInspection?> GetByIdWithProjectAsync(int id);
        IQueryable<SafetyInspection> Query();
        void Remove(SafetyInspection inspection);
        Task SaveChangesAsync();
    }
}