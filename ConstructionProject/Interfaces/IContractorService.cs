using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IContractorService
    {
        Task<IEnumerable<Contractor>> GetAllContractorsAsync();
        Task<Contractor> AddContractorAsync(Contractor contractor);
        Task<bool> AssignContractorAsync(int contractorId, Workforce worker);
        Task<Contractor?> GetContractorDetailsAsync(int id);
        Task<bool> UpdateContractorAsync(int id, Contractor updated);
        Task<bool> DeleteContractorAsync(int id);
        Task<List<Contractor>> GetAllAsync();
    }
}