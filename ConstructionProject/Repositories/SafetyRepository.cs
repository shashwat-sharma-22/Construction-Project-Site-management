using ConstructionProject.Data;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Repositories
{
    public class SafetyRepository : ISafetyRepository
    {
        private readonly AppDbContext _db;

        public SafetyRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<bool> ProjectExistsAsync(int projectId)
        {
            return _db.Projects.AnyAsync(p => p.ProjectId == projectId);
        }

        public Task AddAsync(SafetyInspection inspection)
        {
            return _db.SafetyInspections.AddAsync(inspection).AsTask();
        }

        public Task<SafetyInspection?> GetByIdAsync(int id)
        {
            return _db.SafetyInspections.FindAsync(id).AsTask();
        }

        public Task<SafetyInspection?> GetByIdWithProjectAsync(int id)
        {
            return _db.SafetyInspections
                .Include(s => s.Project)
                .FirstOrDefaultAsync(s => s.InspectionId == id);
        }

        public IQueryable<SafetyInspection> Query()
        {
            return _db.SafetyInspections.AsQueryable();
        }

        public void Remove(SafetyInspection inspection)
        {
            _db.SafetyInspections.Remove(inspection);
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}