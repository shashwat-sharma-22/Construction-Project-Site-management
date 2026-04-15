using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IContractorRepository
    {
        Task<List<Contractor>> GetAllWithWorkforcesAsync();
        Task AddAsync(Contractor contractor);
        Task<Contractor?> GetByIdAsync(int id);
        Task<Contractor?> GetByIdWithWorkforcesAsync(int id);
        Task AddWorkforceAsync(Workforce worker);
        void Remove(Contractor contractor);
        Task SaveChangesAsync();
    }
}