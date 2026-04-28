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

        public IQueryable<Contractor> QueryContractors()
        {
            return _db.Contractors.AsQueryable();
        }

        public IQueryable<Workforce> QueryWorkforces()
        {
            return _db.Workforces.AsQueryable();
        }

        public Task<Contractor?> GetByEmailAsync(string email)
        {
            return _db.Contractors
                .Include(c => c.Workforces)
                .FirstOrDefaultAsync(c => c.ContactInfo == email);
        }

        public Task<Workforce?> GetWorkerAsync(int workerId, int contractorId)
        {
            return _db.Workforces
                .FirstOrDefaultAsync(w => w.WorkerId == workerId && w.ContractorId == contractorId);
        }

        public void RemoveWorker(Workforce worker)
        {
            _db.Workforces.Remove(worker);
        }

        public Task<List<Workforce>> GetWorkforceByContractorAsync(int contractorId)
        {
            return _db.Workforces
                .Where(w => w.ContractorId == contractorId)
                .ToListAsync();
        }

        public async Task ClearContractorFromProjectsAsync(int contractorId)
        {
            var projects = await _db.Projects
                .Where(p => p.ContractorId == contractorId)
                .ToListAsync();

            foreach (var project in projects)
            {
                project.ContractorId = null;
            }

            var workerIds = await _db.Workforces
                .Where(w => w.ContractorId == contractorId)
                .Select(w => w.WorkerId)
                .ToListAsync();

            var tasks = await _db.Tasks
                .Where(t => t.WorkerId != null && workerIds.Contains(t.WorkerId.Value))
                .ToListAsync();

            foreach (var task in tasks)
            {
                task.WorkerId = null;
            }

            var workforces = await _db.Workforces
                .Where(w => w.ContractorId == contractorId)
                .ToListAsync();

            _db.Workforces.RemoveRange(workforces);
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}