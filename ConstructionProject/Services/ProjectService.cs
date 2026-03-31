using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionProject.Data;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _db;

        public ProjectService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _db.Projects.ToListAsync();
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> GetProjectDetailsAsync(int id)
        {
            return await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public async Task<bool> UpdateProjectPlanAsync(int id, Project updated)
        {
            var existing = await _db.Projects.FindAsync(id);
            if (existing == null) return false;

            existing.ProjectName = updated.ProjectName;
            existing.startDate = updated.startDate;
            existing.endDate = updated.endDate;
            existing.budget = updated.budget;

            _db.Projects.Update(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _db.Projects.FindAsync(id);
            if (project == null) return false;

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
