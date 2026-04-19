using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Data;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Services
{
    public class TaskService
    {
        private readonly AppDbContext _db;

        public TaskService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(int projectId)
        {
            return await _db.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Project)
                .Include(t => t.Worker)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> SearchTasksAsync(int projectId, string searchTerm)
        {
            var query = _db.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Project)
                .Include(t => t.Worker)
                .AsQueryable();

            if (int.TryParse(searchTerm, out int id))
            {
                query = query.Where(t => t.TaskId == id || (t.TaskName != null && t.TaskName.Contains(searchTerm)));
            }
            else
            {
                query = query.Where(t => t.TaskName != null && t.TaskName.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }

        public async Task<ProjectTask?> GetTaskByIdAsync(int id)
        {
            return await _db.Tasks
                .Include(t => t.Project)
                .Include(t => t.Worker)
                .FirstOrDefaultAsync(t => t.TaskId == id);
        }

        public async Task<ProjectTask> CreateTaskAsync(ProjectTask task)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateTaskAsync(int id, ProjectTask updated)
        {
            var existing = await _db.Tasks.FindAsync(id);
            if (existing == null) return false;

            existing.TaskName = updated.TaskName;
            existing.WorkerId = updated.WorkerId;
            existing.Deadline = updated.Deadline;
            existing.Status = updated.Status;

            _db.Tasks.Update(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null) return false;

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
