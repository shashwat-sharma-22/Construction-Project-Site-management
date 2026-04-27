using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IContractorService
    {
        Task<IEnumerable<Contractor>> GetAllContractorsAsync();
        Task<IEnumerable<Contractor>> SearchContractorsAsync(string searchTerm);
        Task<Contractor> AddContractorAsync(Contractor contractor);
        Task<bool> AssignContractorAsync(int contractorId, Workforce worker);
        Task<Contractor?> GetContractorDetailsAsync(int id);
        Task<bool> UpdateContractorAsync(int id, Contractor updated);
        Task<bool> DeleteContractorAsync(int id);
        Task<List<Contractor>> GetAllAsync();
        Task<IEnumerable<Workforce>> GetWorkforceByContractorAsync(int contractorId);
        Task<IEnumerable<Workforce>> SearchWorkforceAsync(int contractorId, string searchTerm);
        Task<Contractor?> GetContractorByEmailAsync(string email);
        Task<bool> RemoveWorkerAsync(int workerId, int contractorId);
    }
}