using ConstructionProject.Models;
using ConstructionProject.Services;

namespace ConstructionProject.Interfaces
{
    public interface ISafetyService
    {
        Task<SafetyInspection> RecordInspectionAsync(SafetyInspection inspection);
        Task<bool> UpdateInspectionAsync(int id, SafetyInspection updated);
        Task<bool> UpdateSafetyStatusAsync(int inspectionId, ComplianceStatus status);
        Task<SafetyInspection?> GetInspectionByIdAsync(int id);
        Task<List<SafetyInspection>> GetInspectionHistoryAsync(int projectId);
        Task<List<SafetyInspection>> GetAllInspectionsAsync();
        Task<List<SafetyInspection>> GetInspectionsByStatusAsync(ComplianceStatus status);
        Task<bool> DeleteInspectionAsync(int inspectionId);
        Task<SafetyComplianceReport> GetComplianceReportAsync(int projectId, DateTime? start = null, DateTime? end = null);
    }
}