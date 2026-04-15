using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

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
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();
            return project;
        }

        public async Task<Project?> GetProjectDetailsAsync(int id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateProjectPlanAsync(int id, Project updated)
        {
            var existing = await _projectRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.ProjectName = updated.ProjectName;
            existing.startDate = updated.startDate;
            existing.endDate = updated.endDate;
            existing.budget = updated.budget;

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
