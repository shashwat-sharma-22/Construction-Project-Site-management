using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Services
{
    public class SafetyService : ISafetyService
    {
        private readonly ISafetyRepository _safetyRepository;

        public SafetyService(ISafetyRepository safetyRepository)
        {
            _safetyRepository = safetyRepository;
        }

        public async Task<SafetyInspection> RecordInspectionAsync(SafetyInspection inspection)
        {
            var projectExists = await _safetyRepository.ProjectExistsAsync(inspection.ProjectId);

            if (!projectExists)
            {
                throw new Exception("Invalid Project ID");
            }

            await _safetyRepository.AddAsync(inspection);
            await _safetyRepository.SaveChangesAsync();
            return inspection;
        }

        public async Task<bool> UpdateInspectionAsync(int id, SafetyInspection updated)
        {
            var existing = await _safetyRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.ProjectId = updated.ProjectId;
            existing.InspectionDate = updated.InspectionDate;
            existing.ComplianceStatus = updated.ComplianceStatus;
            existing.IssuesFound = updated.IssuesFound;

            await _safetyRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSafetyStatusAsync(int inspectionId, ComplianceStatus status)
        {
            var inspection = await _safetyRepository.GetByIdAsync(inspectionId);
            if (inspection == null) return false;

            inspection.ComplianceStatus = status;
            await _safetyRepository.SaveChangesAsync();
            return true;
        }

        public async Task<SafetyInspection?> GetInspectionByIdAsync(int id)
        {
            return await _safetyRepository.GetByIdWithProjectAsync(id);
        }

        public async Task<List<SafetyInspection>> GetInspectionHistoryAsync(int projectId)
        {
            var query = _safetyRepository.Query()
                .Where(s => s.ProjectId == projectId)
                .OrderByDescending(s => s.InspectionDate);

            return await Task.FromResult(query.ToList());
        }

        public async Task<List<SafetyInspection>> GetAllInspectionsAsync()
        {
            var query = _safetyRepository.Query()
                .OrderByDescending(s => s.InspectionDate);

            return await Task.FromResult(query.ToList());
        }

        public async Task<List<SafetyInspection>> GetInspectionsByStatusAsync(ComplianceStatus status)
        {
            var query = _safetyRepository.Query()
                .Where(s => s.ComplianceStatus == status)
                .OrderByDescending(s => s.InspectionDate);

            return await Task.FromResult(query.ToList());
        }

        public async Task<bool> DeleteInspectionAsync(int inspectionId)
        {
            var inspection = await _safetyRepository.GetByIdAsync(inspectionId);
            if (inspection == null) return false;

            _safetyRepository.Remove(inspection);
            await _safetyRepository.SaveChangesAsync();
            return true;
        }

        public async Task<SafetyComplianceReport> GetComplianceReportAsync(int projectId, DateTime? start = null, DateTime? end = null)
        {
            var query = _safetyRepository.Query().Where(s => s.ProjectId == projectId);

            if (start.HasValue)
                query = query.Where(s => s.InspectionDate >= start.Value);

            if (end.HasValue)
                query = query.Where(s => s.InspectionDate <= end.Value);

            var inspections = query.OrderByDescending(s => s.InspectionDate).ToList();

            return await Task.FromResult(new SafetyComplianceReport
            {
                ProjectId = projectId,
                TotalInspections = inspections.Count,
                CompliantInspections = inspections.Count(s => s.ComplianceStatus == ComplianceStatus.COMPLIANT),
                NonCompliantInspections = inspections.Count(s => s.ComplianceStatus == ComplianceStatus.NON_COMPLIANT),
                ComplianceRate = inspections.Count > 0
                    ? Math.Round((decimal)inspections.Count(s => s.ComplianceStatus == ComplianceStatus.COMPLIANT) / inspections.Count * 100, 2)
                    : 0,
                Inspections = inspections
            });
        }
    }

    public class SafetyComplianceReport
    {
        public int ProjectId { get; set; }
        public int TotalInspections { get; set; }
        public int CompliantInspections { get; set; }
        public int NonCompliantInspections { get; set; }
        public decimal ComplianceRate { get; set; }
        public List<SafetyInspection> Inspections { get; set; } = new();
    }
}
