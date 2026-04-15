using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Services
{
    public class ContractorService : IContractorService
    {
        private readonly IContractorRepository _contractorRepository;

        public ContractorService(IContractorRepository contractorRepository)
        {
            _contractorRepository = contractorRepository;
        }

        public async Task<IEnumerable<Contractor>> GetAllContractorsAsync()
        {
            return await _contractorRepository.GetAllWithWorkforcesAsync();
        }

        public async Task<Contractor> AddContractorAsync(Contractor contractor)
        {
            await _contractorRepository.AddAsync(contractor);
            await _contractorRepository.SaveChangesAsync();
            return contractor;
        }

        public async Task<bool> AssignContractorAsync(int contractorId, Workforce worker)
        {
            var contractor = await _contractorRepository.GetByIdAsync(contractorId);
            if (contractor == null) return false;

            worker.ContractorId = contractorId;
            await _contractorRepository.AddWorkforceAsync(worker);
            await _contractorRepository.SaveChangesAsync();
            return true;
        }

        public async Task<Contractor?> GetContractorDetailsAsync(int id)
        {
            return await _contractorRepository.GetByIdWithWorkforcesAsync(id);
        }

        public async Task<bool> UpdateContractorAsync(int id, Contractor updated)
        {
            var existing = await _contractorRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.ContractorName = updated.ContractorName;
            existing.Specialization = updated.Specialization;
            existing.ContactInfo = updated.ContactInfo;

            await _contractorRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContractorAsync(int id)
        {
            var contractor = await _contractorRepository.GetByIdAsync(id);
            if (contractor == null) return false;

            _contractorRepository.Remove(contractor);
            await _contractorRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<Contractor>> GetAllAsync()
        {
            return await _contractorRepository.GetAllWithWorkforcesAsync();
        }
    }
}
