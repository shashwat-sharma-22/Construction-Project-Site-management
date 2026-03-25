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

            // update allowed fields
            existing.ProjectName = updated.ProjectName;
            existing.startDate = updated.startDate;
            existing.endDate = updated.endDate;
            existing.budget = updated.budget;

            _db.Projects.Update(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
