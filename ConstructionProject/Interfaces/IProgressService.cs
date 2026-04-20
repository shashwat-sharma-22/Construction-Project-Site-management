using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IProgressService
    {
        Task<IEnumerable<Progress>> GetAllProgressAsync();
        Task<IEnumerable<Progress>> GetProgressByProjectIdsAsync(IEnumerable<int> projectIds);
        Task<Progress> RecordProgressAsync(Progress progress);
        Task<Progress?> UpdateProgressAsync(int id, Progress updated);
        Task<Progress?> GetByIdAsync(int id);
        Task<bool> DeleteProgressAsync(int id);
        Task<IEnumerable<Progress>> GetProgressReportAsync(int projectId, DateTime? start = null, DateTime? end = null);
    }
}