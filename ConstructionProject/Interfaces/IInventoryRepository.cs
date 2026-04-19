using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IInventoryRepository
    {
        Task AddMaterialAsync(Material material);
        Task AddEquipmentAsync(Equipment equipment);
        Task<Equipment?> GetEquipmentByIdAsync(int id);
        Task<ProjectTask?> GetTaskByIdAsync(int id);
        Task<Material?> GetMaterialByIdAsync(int id);
        Task<List<Material>> GetAllMaterialsAsync();
        Task<List<Equipment>> GetAllEquipmentAsync();
        Task SaveChangesAsync();
    }
}