using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Material> AddMaterialAsync(Material material)
        {
            await _inventoryRepository.AddMaterialAsync(material);
            await _inventoryRepository.SaveChangesAsync();
            return material;
        }

        public async Task<Equipment> AddEquipmentAsync(Equipment equipment)
        {
            await _inventoryRepository.AddEquipmentAsync(equipment);
            await _inventoryRepository.SaveChangesAsync();
            return equipment;
        }

        public async Task<bool> AssignEquipmentAsync(int equipmentId, int projectTaskId)
        {
            var equipment = await _inventoryRepository.GetEquipmentByIdAsync(equipmentId);
            if (equipment == null) return false;

            var task = await _inventoryRepository.GetTaskByIdAsync(projectTaskId);
            if (task == null) return false;

            equipment.Status = EquipmentStatus.IN_USE;
            await _inventoryRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMaterialQuantityAsync(int materialId, decimal quantityChange)
        {
            var material = await _inventoryRepository.GetMaterialByIdAsync(materialId);
            if (material == null) return false;

            material.QuantityAvailable += quantityChange;
            await _inventoryRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEquipmentStatusAsync(int equipmentId, EquipmentStatus status)
        {
            var equipment = await _inventoryRepository.GetEquipmentByIdAsync(equipmentId);
            if (equipment == null) return false;

            equipment.Status = status;
            await _inventoryRepository.SaveChangesAsync();
            return true;
        }

        public async Task<Material?> GetMaterialAsync(int id)
        {
            return await _inventoryRepository.GetMaterialByIdAsync(id);
        }

        public async Task<Equipment?> GetEquipmentAsync(int id)
        {
            return await _inventoryRepository.GetEquipmentByIdAsync(id);
        }

        public async Task<List<Material>> GetAllMaterialsAsync()
        {
            return await _inventoryRepository.GetAllMaterialsAsync();
        }

        public async Task<List<Equipment>> GetAllEquipmentAsync()
        {
            return await _inventoryRepository.GetAllEquipmentAsync();
        }

        public async Task<InventoryStatus> GetInventoryStatusAsync()
        {
            var materials = await _inventoryRepository.GetAllMaterialsAsync();
            var equipments = await _inventoryRepository.GetAllEquipmentAsync();

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
