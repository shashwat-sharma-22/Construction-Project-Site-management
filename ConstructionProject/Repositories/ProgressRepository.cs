using ConstructionProject.Data;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Repositories
{
    public class ProgressRepository : IProgressRepository
    {
        private readonly AppDbContext _db;

        public ProgressRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<Progress>> GetAllWithProjectAsync()
        {
            return _db.Progresses.Include(p => p.Project).ToListAsync();
        }

        public Task AddAsync(Progress progress)
        {
            return _db.Progresses.AddAsync(progress).AsTask();
        }

        public Task<Progress?> GetByIdAsync(int id)
        {
            return _db.Progresses.FindAsync(id).AsTask();
        }

        public Task<Progress?> GetByIdWithProjectAsync(int id)
        {
            return _db.Progresses
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.ProgressId == id);
        }

        public void Remove(Progress progress)
        {
            _db.Progresses.Remove(progress);
        }

        public IQueryable<Progress> Query()
        {
            return _db.Progresses.AsQueryable();
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}