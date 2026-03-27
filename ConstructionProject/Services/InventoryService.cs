using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Data;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Services
{
    public class InventoryService
    {
        private readonly AppDbContext _db;

        public InventoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Material> AddMaterialAsync(Material material)
        {
            _db.Materials.Add(material);
            await _db.SaveChangesAsync();
            return material;
        }

        public async Task<Equipment> AddEquipmentAsync(Equipment equipment)
        {
            _db.Equipments.Add(equipment);
            await _db.SaveChangesAsync();
            return equipment;
        }

        public async Task<bool> AssignEquipmentAsync(int equipmentId, int projectTaskId)
        {
            var equipment = await _db.Equipments.FindAsync(equipmentId);
            if (equipment == null) return false;

            var task = await _db.Tasks.FindAsync(projectTaskId);
            if (task == null) return false;

            equipment.Status = EquipmentStatus.IN_USE;
            _db.Equipments.Update(equipment);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMaterialQuantityAsync(int materialId, decimal quantityChange)
        {
            var material = await _db.Materials.FindAsync(materialId);
            if (material == null) return false;

            material.QuantityAvailable += quantityChange;
            _db.Materials.Update(material);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEquipmentStatusAsync(int equipmentId, EquipmentStatus status)
        {
            var equipment = await _db.Equipments.FindAsync(equipmentId);
            if (equipment == null) return false;

            equipment.Status = status;
            _db.Equipments.Update(equipment);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Material?> GetMaterialAsync(int id)
        {
            return await _db.Materials.FirstOrDefaultAsync(m => m.MaterialId == id);
        }

        public async Task<Equipment?> GetEquipmentAsync(int id)
        {
            return await _db.Equipments.FirstOrDefaultAsync(e => e.EquipmentId == id);
        }

        public async Task<List<Material>> GetAllMaterialsAsync()
        {
            return await _db.Materials.ToListAsync();
        }

        public async Task<List<Equipment>> GetAllEquipmentAsync()
        {
            return await _db.Equipments.ToListAsync();
        }

        public async Task<InventoryStatus> GetInventoryStatusAsync()
        {
            var materials = await _db.Materials.ToListAsync();
            var equipments = await _db.Equipments.ToListAsync();

            return new InventoryStatus
            {
                TotalMaterials = materials.Count,
                TotalEquipment = equipments.Count,
                AvailableEquipment = equipments.Count(e => e.Status == EquipmentStatus.AVAILABLE),
                InUseEquipment = equipments.Count(e => e.Status == EquipmentStatus.IN_USE),
                MaintenanceEquipment = equipments.Count(e => e.Status == EquipmentStatus.MAINTENANCE),
                TotalMaterialValue = materials.Sum(m => m.QuantityAvailable * m.UnitCost),
                Materials = materials,
                Equipments = equipments
            };
        }
    }

    public class InventoryStatus
    {
        public int TotalMaterials { get; set; }
        public int TotalEquipment { get; set; }
        public int AvailableEquipment { get; set; }
        public int InUseEquipment { get; set; }
        public int MaintenanceEquipment { get; set; }
        public decimal TotalMaterialValue { get; set; }
        public List<Material> Materials { get; set; } = new();
        public List<Equipment> Equipments { get; set; } = new();
    }
}
