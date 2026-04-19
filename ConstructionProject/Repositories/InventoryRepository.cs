using ConstructionProject.Data;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _db;

        public InventoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task AddMaterialAsync(Material material)
        {
            return _db.Materials.AddAsync(material).AsTask();
        }

        public Task AddEquipmentAsync(Equipment equipment)
        {
            return _db.Equipments.AddAsync(equipment).AsTask();
        }

        public Task<Equipment?> GetEquipmentByIdAsync(int id)
        {
            return _db.Equipments.FindAsync(id).AsTask();
        }

        public Task<ProjectTask?> GetTaskByIdAsync(int id)
        {
            return _db.Tasks.FindAsync(id).AsTask();
        }

        public Task<Material?> GetMaterialByIdAsync(int id)
        {
            return _db.Materials.FindAsync(id).AsTask();
        }

        public Task<List<Material>> GetAllMaterialsAsync()
        {
            return _db.Materials.ToListAsync();
        }

        public Task<List<Equipment>> GetAllEquipmentAsync()
        {
            return _db.Equipments.ToListAsync();
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}