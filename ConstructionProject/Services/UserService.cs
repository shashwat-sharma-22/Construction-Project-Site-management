using ConstructionProject.Data;
using ConstructionProject.DTOs;
using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionProject.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // ── Register a new user (Admin only) ─────────────────────────────────
        public async Task<UserResponseDto?> RegisterUser(RegisterUserDto dto)
        {
            // Check if email already exists
            var exists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email);

            if (exists) return null;

            var user = new AppUser
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        // ── Get all users ─────────────────────────────────────────────────────
        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(MapToDto).ToList();
        }

        // ── Get users by role ─────────────────────────────────────────────────
        public async Task<List<UserResponseDto>> GetUsersByRole(UserRole role)
        {
            var users = await _context.Users
                .Where(u => u.Role == role && u.IsActive)
                .ToListAsync();
            return users.Select(MapToDto).ToList();
        }


        // ── Get single user ───────────────────────────────────────────────────
        public async Task<UserResponseDto?> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : MapToDto(user);
        }

        // ── Update role ───────────────────────────────────────────────────────
        public async Task<UserResponseDto?> UpdateRole(int userId, UserRole newRole)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.Role = newRole;
            await _context.SaveChangesAsync();
            return MapToDto(user);
        }

        // ── Activate / Deactivate user ────────────────────────────────────────
        public async Task<bool> SetActiveStatus(int userId, bool isActive)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Delete user ───────────────────────────────────────────────────────
        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Login check ───────────────────────────────────────────────────────
        public async Task<AppUser?> ValidateLogin(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.IsActive);

            if (user == null) return null;

            // Verify hashed password
            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            return valid ? user : null;
        }

        // ── Helper: convert entity to response DTO ────────────────────────────
        private UserResponseDto MapToDto(AppUser u) => new UserResponseDto
        {
            UserId = u.UserId,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role.ToString(),
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        };
    }
}
