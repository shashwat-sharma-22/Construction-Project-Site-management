using ConstructionProject.DTOs;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // ── Register a new user (Admin only) ─────────────────────────────────
        public async Task<UserResponseDto?> RegisterUser(RegisterUserDto dto)
        {
            var exists = await _userRepository.EmailExistsAsync(dto.Email);

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

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapToDto(user);
        }

        // ── Get all users ─────────────────────────────────────────────────────
        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto).ToList();
        }

        // ── Get users by role ─────────────────────────────────────────────────
        public async Task<List<UserResponseDto>> GetUsersByRole(UserRole role)
        {
            var users = await _userRepository.GetActiveByRoleAsync(role);
            return users.Select(MapToDto).ToList();
        }


        // ── Get single user ───────────────────────────────────────────────────
        public async Task<UserResponseDto?> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToDto(user);
        }

        // ── Update role ───────────────────────────────────────────────────────
        public async Task<UserResponseDto?> UpdateRole(int userId, UserRole newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            user.Role = newRole;
            await _userRepository.SaveChangesAsync();
            return MapToDto(user);
        }

        // ── Activate / Deactivate user ────────────────────────────────────────
        public async Task<bool> SetActiveStatus(int userId, bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = isActive;
            await _userRepository.SaveChangesAsync();
            return true;
        }

        // ── Delete user ───────────────────────────────────────────────────────
        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        // ── Login check ───────────────────────────────────────────────────────
        public async Task<AppUser?> ValidateLogin(LoginDto dto)
        {
            var user = await _userRepository.GetActiveByEmailAsync(dto.Email);

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
