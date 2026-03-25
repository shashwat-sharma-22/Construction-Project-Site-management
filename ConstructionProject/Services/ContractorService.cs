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

        public async Task<List<Contractor>> GetAllAsync()
        {
            return await _db.Contractors.Include(c => c.Workforces).ToListAsync();
        }
    }
}
