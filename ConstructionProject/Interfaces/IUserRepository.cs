using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(AppUser user);
        Task<List<AppUser>> GetAllAsync();
        Task<List<AppUser>> GetActiveByRoleAsync(UserRole role);
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser?> GetActiveByEmailAsync(string email);
        void Remove(AppUser user);
        Task SaveChangesAsync();
    }
}