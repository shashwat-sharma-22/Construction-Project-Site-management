using ConstructionProject.Models;
using ConstructionProject.Services;

namespace ConstructionProject.Interfaces
{
    public interface IInventoryService
    {
        Task<Material> AddMaterialAsync(Material material);
        Task<Equipment> AddEquipmentAsync(Equipment equipment);
        Task<bool> AssignEquipmentAsync(int equipmentId, int projectTaskId);
        Task<bool> UpdateMaterialQuantityAsync(int materialId, decimal quantityChange);
        Task<bool> UpdateEquipmentStatusAsync(int equipmentId, EquipmentStatus status);
        Task<Material?> GetMaterialAsync(int id);
        Task<Equipment?> GetEquipmentAsync(int id);
        Task<List<Material>> GetAllMaterialsAsync();
        Task<List<Equipment>> GetAllEquipmentAsync();
        Task<InventoryStatus> GetInventoryStatusAsync();
    }
}