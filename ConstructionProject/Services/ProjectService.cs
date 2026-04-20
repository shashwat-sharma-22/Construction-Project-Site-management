using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllWithContractorAsync();
        }

        public async Task<IEnumerable<Project>> SearchProjectsAsync(string searchTerm)
        {
            var query = _projectRepository.Query().AsQueryable();

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
            return await _projectRepository.Query()
                .Include(p => p.Contractor)
                .Where(p => p.ContractorId == contractorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> SearchProjectsByContractorAsync(int contractorId, string searchTerm)
        {
            var query = _projectRepository.Query()
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
            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> GetProjectDetailsAsync(int id)
        {
            return await _projectRepository.GetByIdWithContractorAsync(id);
        }

        public async Task<bool> UpdateProjectPlanAsync(int id, Project updated)
        {
            var existing = await _projectRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.ProjectName = updated.ProjectName;
            existing.startDate = updated.startDate;
            existing.endDate = updated.endDate;
            existing.budget = updated.budget;
            existing.ContractorId = updated.ContractorId;

            await _projectRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return false;

            _projectRepository.Remove(project);
            await _projectRepository.SaveChangesAsync();
            return true;
        }
    }
}
