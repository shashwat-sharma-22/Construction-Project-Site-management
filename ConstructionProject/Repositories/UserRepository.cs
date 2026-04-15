using ConstructionProject.Data;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return _context.Users.AnyAsync(u => u.Email == email);
        }

        public Task AddAsync(AppUser user)
        {
            return _context.Users.AddAsync(user).AsTask();
        }

        public Task<List<AppUser>> GetAllAsync()
        {
            return _context.Users.ToListAsync();
        }

        public Task<List<AppUser>> GetActiveByRoleAsync(UserRole role)
        {
            return _context.Users.Where(u => u.Role == role && u.IsActive).ToListAsync();
        }

        public Task<AppUser?> GetByIdAsync(int id)
        {
            return _context.Users.FindAsync(id).AsTask();
        }

        public Task<AppUser?> GetActiveByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public void Remove(AppUser user)
        {
            _context.Users.Remove(user);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}