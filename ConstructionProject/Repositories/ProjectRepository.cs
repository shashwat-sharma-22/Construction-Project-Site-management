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

        public Task AddAsync(Project project)
        {
            return _db.Projects.AddAsync(project).AsTask();
        }

        public Task<Project?> GetByIdAsync(int id)
        {
            return _db.Projects.FindAsync(id).AsTask();
        }

        public void Remove(Project project)
        {
            _db.Projects.Remove(project);
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}