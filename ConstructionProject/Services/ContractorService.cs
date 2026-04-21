using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Contractor>> SearchContractorsAsync(string searchTerm)
        {
            var query = _contractorRepository.QueryContractors().Include(c => c.Workforces).AsQueryable();

            if (int.TryParse(searchTerm, out int id))
            {
                query = query.Where(c => c.ContractorId == id || (c.ContractorName != null && c.ContractorName.Contains(searchTerm)));
            }
            else
            {
                query = query.Where(c => c.ContractorName != null && c.ContractorName.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Workforce>> SearchWorkforceAsync(int contractorId, string searchTerm)
        {
            var query = _contractorRepository.QueryWorkforces().Where(w => w.ContractorId == contractorId).AsQueryable();

            if (int.TryParse(searchTerm, out int id))
            {
                query = query.Where(w => w.WorkerId == id || (w.Name != null && w.Name.Contains(searchTerm)));
            }
            else
            {
                query = query.Where(w => w.Name != null && w.Name.Contains(searchTerm));
            }

            return await query.ToListAsync();
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

            await _contractorRepository.ClearContractorFromProjectsAsync(id);
            _contractorRepository.Remove(contractor);
            await _contractorRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<Contractor>> GetAllAsync()
        {
            return await _contractorRepository.GetAllWithWorkforcesAsync();
        }

        public async Task<IEnumerable<Contractor>> GetAvailableContractorsAsync()
        {
            var allContractors = await _contractorRepository.GetAllWithWorkforcesAsync();
            return allContractors;
        }

        public async Task<IEnumerable<Workforce>> GetWorkforceByContractorAsync(int contractorId)
        {
            return await _contractorRepository.GetWorkforceByContractorAsync(contractorId);
        }

        public async Task<Contractor?> GetContractorByEmailAsync(string email)
        {
            return await _contractorRepository.GetByEmailAsync(email);
        }

        public async Task<bool> RemoveWorkerAsync(int workerId, int contractorId)
        {
            var worker = await _contractorRepository.GetWorkerAsync(workerId, contractorId);
            if (worker == null) return false;

            _contractorRepository.RemoveWorker(worker);
            await _contractorRepository.SaveChangesAsync();
            return true;
        }
    }
}
