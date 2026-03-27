using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Data;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Services
{
    public class ProgressService
    {
        private readonly AppDbContext _db;

        public ProgressService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Progress>> GetAllProgressAsync()
        {
            return await _db.Progresses.Include(p => p.Project).ToListAsync();
        }

        public async Task<Progress> RecordProgressAsync(Progress progress)
        {
            _db.Progresses.Add(progress);
            await _db.SaveChangesAsync();
            return progress;
        }

        public async Task<Progress?> UpdateProgressAsync(int id, Progress updated)
        {
            var existing = await _db.Progresses.FindAsync(id);
            if (existing == null) return null;

            existing.ProjectId = updated.ProjectId;
            existing.ReportDate = updated.ReportDate;
            existing.CompletedTasks = updated.CompletedTasks;
            existing.Remarks = updated.Remarks;

            _db.Progresses.Update(existing);
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<Progress?> GetByIdAsync(int id)
        {
            return await _db.Progresses
                .Include(p => p.Project)
                .FirstOrDefaultAsync(p => p.ProgressId == id);
        }

        public async Task<bool> DeleteProgressAsync(int id)
        {
            var progress = await _db.Progresses.FindAsync(id);
            if (progress == null) return false;

            _db.Progresses.Remove(progress);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Progress>> GetProgressReportAsync(int projectId, DateTime? start = null, DateTime? end = null)
        {
            var query = _db.Progresses.AsQueryable();

            if (projectId > 0)
                query = query.Where(p => p.ProjectId == projectId);

            if (start.HasValue)
                query = query.Where(p => p.ReportDate >= start.Value.Date);

            if (end.HasValue)
                query = query.Where(p => p.ReportDate <= end.Value.Date);

            return await query.OrderBy(p => p.ReportDate).ToListAsync();
        }
    }
}
