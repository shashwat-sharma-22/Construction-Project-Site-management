using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressRepository _progressRepository;

        public ProgressService(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        public async Task<IEnumerable<Progress>> GetAllProgressAsync()
        {
            return await _progressRepository.GetAllWithProjectAsync();
        }

        public async Task<Progress> RecordProgressAsync(Progress progress)
        {
            await _progressRepository.AddAsync(progress);
            await _progressRepository.SaveChangesAsync();
            return progress;
        }

        public async Task<Progress?> UpdateProgressAsync(int id, Progress updated)
        {
            var existing = await _progressRepository.GetByIdAsync(id);
            if (existing == null) return null;

            existing.ProjectId = updated.ProjectId;
            existing.ReportDate = updated.ReportDate;
            existing.CompletedTasks = updated.CompletedTasks;
            existing.Remarks = updated.Remarks;

            await _progressRepository.SaveChangesAsync();
            return existing;
        }

        public async Task<Progress?> GetByIdAsync(int id)
        {
            return await _progressRepository.GetByIdWithProjectAsync(id);
        }

        public async Task<bool> DeleteProgressAsync(int id)
        {
            var progress = await _progressRepository.GetByIdAsync(id);
            if (progress == null) return false;

            _progressRepository.Remove(progress);
            await _progressRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Progress>> GetProgressReportAsync(int projectId, DateTime? start = null, DateTime? end = null)
        {
            var query = _progressRepository.Query();

            if (projectId > 0)
                query = query.Where(p => p.ProjectId == projectId);

            if (start.HasValue)
                query = query.Where(p => p.ReportDate >= start.Value.Date);

            if (end.HasValue)
                query = query.Where(p => p.ReportDate <= end.Value.Date);

            return await Task.FromResult(query.OrderBy(p => p.ReportDate).ToList());
        }
    }
}
