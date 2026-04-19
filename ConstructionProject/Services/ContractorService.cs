using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Data;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Services
{
    public class ContractorService
    {
        private readonly AppDbContext _db;

        public ContractorService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Contractor>> GetAllContractorsAsync()
        {
            return await _db.Contractors.Include(c => c.Workforces).ToListAsync();
        }

        public async Task<IEnumerable<Contractor>> SearchContractorsAsync(string searchTerm)
        {
            var query = _db.Contractors.Include(c => c.Workforces).AsQueryable();

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
            var query = _db.Workforces.Where(w => w.ContractorId == contractorId).AsQueryable();

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
            _db.Contractors.Add(contractor);
            await _db.SaveChangesAsync();
            return contractor;
        }

        public async Task<bool> AssignContractorAsync(int contractorId, Workforce worker)
        {
            var contractor = await _db.Contractors.FindAsync(contractorId);
            if (contractor == null) return false;

            // ensure worker references contractor
            worker.ContractorId = contractorId;
            _db.Workforces.Add(worker);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Contractor?> GetContractorDetailsAsync(int id)
        {
            return await _db.Contractors
                .Include(c => c.Workforces)
                .FirstOrDefaultAsync(c => c.ContractorId == id);
        }

        public async Task<bool> UpdateContractorAsync(int id, Contractor updated)
        {
            var existing = await _db.Contractors.FindAsync(id);
            if (existing == null) return false;

            existing.ContractorName = updated.ContractorName;
            existing.Specialization = updated.Specialization;
            existing.ContactInfo = updated.ContactInfo;

            _db.Contractors.Update(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContractorAsync(int id)
        {
            var contractor = await _db.Contractors.FindAsync(id);
            if (contractor == null) return false;

            _db.Contractors.Remove(contractor);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Contractor>> GetAllAsync()
        {
            return await _db.Contractors.Include(c => c.Workforces).ToListAsync();
        }

        public async Task<IEnumerable<Contractor>> GetAvailableContractorsAsync()
        {
            var assignedIds = await _db.Projects
                .Where(p => p.ContractorId != null)
                .Select(p => p.ContractorId!.Value)
                .ToListAsync();

            return await _db.Contractors
                .Where(c => !assignedIds.Contains(c.ContractorId))
                .Include(c => c.Workforces)
                .ToListAsync();
        }

        public async Task<IEnumerable<Workforce>> GetWorkforceByContractorAsync(int contractorId)
        {
            return await _db.Workforces
                .Where(w => w.ContractorId == contractorId)
                .ToListAsync();
        }

        public async Task<Contractor?> GetContractorByEmailAsync(string email)
        {
            return await _db.Contractors
                .Include(c => c.Workforces)
                .FirstOrDefaultAsync(c => c.ContactInfo == email);
        }

        public async Task<bool> RemoveWorkerAsync(int workerId, int contractorId)
        {
            var worker = await _db.Workforces
                .FirstOrDefaultAsync(w => w.WorkerId == workerId && w.ContractorId == contractorId);
            if (worker == null) return false;

            _db.Workforces.Remove(worker);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
