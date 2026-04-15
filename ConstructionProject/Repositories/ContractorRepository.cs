using ConstructionProject.Data;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Repositories
{
    public class ContractorRepository : IContractorRepository
    {
        private readonly AppDbContext _db;

        public ContractorRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<Contractor>> GetAllWithWorkforcesAsync()
        {
            return _db.Contractors.Include(c => c.Workforces).ToListAsync();
        }

        public Task AddAsync(Contractor contractor)
        {
            return _db.Contractors.AddAsync(contractor).AsTask();
        }

        public Task<Contractor?> GetByIdAsync(int id)
        {
            return _db.Contractors.FindAsync(id).AsTask();
        }

        public Task<Contractor?> GetByIdWithWorkforcesAsync(int id)
        {
            return _db.Contractors
                .Include(c => c.Workforces)
                .FirstOrDefaultAsync(c => c.ContractorId == id);
        }

        public Task AddWorkforceAsync(Workforce worker)
        {
            return _db.Workforces.AddAsync(worker).AsTask();
        }

        public void Remove(Contractor contractor)
        {
            _db.Contractors.Remove(contractor);
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}