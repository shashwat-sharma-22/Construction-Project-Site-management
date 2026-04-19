using System.Collections.Generic;
using System.Linq;
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
            return await _db.Projects.Include(p => p.Contractor).ToListAsync();
        }

        public async Task<IEnumerable<Project>> SearchProjectsAsync(string searchTerm)
        {
            var query = _db.Projects.AsQueryable();

            if (int.TryParse(searchTerm, out int id))
            {
                query = query.Where(p => p.ProjectId == id || (p.ProjectName != null && p.ProjectName.Contains(searchTerm)));
            }
            else
            {
                query = query.Where(p => p.ProjectName != null && p.ProjectName.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByContractorAsync(int contractorId)
        {
            return await _db.Projects
                .Include(p => p.Contractor)
                .Where(p => p.ContractorId == contractorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> SearchProjectsByContractorAsync(int contractorId, string searchTerm)
        {
            var query = _db.Projects
                .Include(p => p.Contractor)
                .Where(p => p.ContractorId == contractorId)
                .AsQueryable();

            if (int.TryParse(searchTerm, out int id))
            {
                query = query.Where(p => p.ProjectId == id || (p.ProjectName != null && p.ProjectName.Contains(searchTerm)));
            }
            else
            {
                query = query.Where(p => p.ProjectName != null && p.ProjectName.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> GetProjectDetailsAsync(int id)
        {
            return await _db.Projects.Include(p => p.Contractor).FirstOrDefaultAsync(p => p.ProjectId == id);
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
            existing.ContractorId = updated.ContractorId;

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
