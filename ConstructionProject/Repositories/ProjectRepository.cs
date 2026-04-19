using ConstructionProject.Data;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _db;

        public ProjectRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<Project>> GetAllAsync()
        {
            return _db.Projects.ToListAsync();
        }

        public Task<List<Project>> GetAllWithContractorAsync()
        {
            return _db.Projects.Include(p => p.Contractor).ToListAsync();
        }

        public Task AddAsync(Project project)
        {
            return _db.Projects.AddAsync(project).AsTask();
        }

        public Task<Project?> GetByIdAsync(int id)
        {
            return _db.Projects.FindAsync(id).AsTask();
        }

        public Task<Project?> GetByIdWithContractorAsync(int id)
        {
            return _db.Projects.Include(p => p.Contractor).FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public void Remove(Project project)
        {
            _db.Projects.Remove(project);
        }

        public IQueryable<Project> Query()
        {
            return _db.Projects.AsQueryable();
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}