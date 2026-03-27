using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Data;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Services
{
    public class SafetyService
    {
        private readonly AppDbContext _db;

        public SafetyService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<SafetyInspection> RecordInspectionAsync(SafetyInspection inspection)
        {
            _db.SafetyInspections.Add(inspection);
            await _db.SaveChangesAsync();
            return inspection;
        }

        public async Task<bool> UpdateInspectionAsync(int id, SafetyInspection updated)
        {
            var existing = await _db.SafetyInspections.FindAsync(id);
            if (existing == null) return false;

            existing.ProjectId = updated.ProjectId;
            existing.InspectionDate = updated.InspectionDate;
            existing.ComplianceStatus = updated.ComplianceStatus;
            existing.IssuesFound = updated.IssuesFound;

            _db.SafetyInspections.Update(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSafetyStatusAsync(int inspectionId, ComplianceStatus status)
        {
            var inspection = await _db.SafetyInspections.FindAsync(inspectionId);
            if (inspection == null) return false;

            inspection.ComplianceStatus = status;
            _db.SafetyInspections.Update(inspection);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<SafetyInspection?> GetInspectionByIdAsync(int id)
        {
            return await _db.SafetyInspections
                .Include(s => s.Project)
                .FirstOrDefaultAsync(s => s.InspectionId == id);
        }

        public async Task<List<SafetyInspection>> GetInspectionHistoryAsync(int projectId)
        {
            return await _db.SafetyInspections
                .Where(s => s.ProjectId == projectId)
                .Include(s => s.Project)
                .OrderByDescending(s => s.InspectionDate)
                .ToListAsync();
        }

        public async Task<List<SafetyInspection>> GetAllInspectionsAsync()
        {
            return await _db.SafetyInspections
                .Include(s => s.Project)
                .OrderByDescending(s => s.InspectionDate)
                .ToListAsync();
        }

        public async Task<List<SafetyInspection>> GetInspectionsByStatusAsync(ComplianceStatus status)
        {
            return await _db.SafetyInspections
                .Where(s => s.ComplianceStatus == status)
                .Include(s => s.Project)
                .OrderByDescending(s => s.InspectionDate)
                .ToListAsync();
        }

        public async Task<bool> DeleteInspectionAsync(int inspectionId)
        {
            var inspection = await _db.SafetyInspections.FindAsync(inspectionId);
            if (inspection == null) return false;

            _db.SafetyInspections.Remove(inspection);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<SafetyComplianceReport> GetComplianceReportAsync(int projectId, DateTime? start = null, DateTime? end = null)
        {
            var query = _db.SafetyInspections.Where(s => s.ProjectId == projectId);

            if (start.HasValue)
                query = query.Where(s => s.InspectionDate >= start.Value);

            if (end.HasValue)
                query = query.Where(s => s.InspectionDate <= end.Value);

            var inspections = await query.OrderByDescending(s => s.InspectionDate).ToListAsync();

            return new SafetyComplianceReport
            {
                ProjectId = projectId,
                TotalInspections = inspections.Count,
                CompliantInspections = inspections.Count(s => s.ComplianceStatus == ComplianceStatus.COMPLIANT),
                NonCompliantInspections = inspections.Count(s => s.ComplianceStatus == ComplianceStatus.NON_COMPLIANT),
                ComplianceRate = inspections.Count > 0 
                    ? Math.Round((decimal)inspections.Count(s => s.ComplianceStatus == ComplianceStatus.COMPLIANT) / inspections.Count * 100, 2)
                    : 0,
                Inspections = inspections
            };
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
