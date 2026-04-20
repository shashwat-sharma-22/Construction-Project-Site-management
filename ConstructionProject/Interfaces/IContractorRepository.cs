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
        IQueryable<Contractor> QueryContractors();
        IQueryable<Workforce> QueryWorkforces();
        Task<Contractor?> GetByEmailAsync(string email);
        Task<Workforce?> GetWorkerAsync(int workerId, int contractorId);
        void RemoveWorker(Workforce worker);
        Task<List<Workforce>> GetWorkforceByContractorAsync(int contractorId);
        Task SaveChangesAsync();
    }
}