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

        public async Task<UserResponseDto?> RegisterUser(RegisterUserDto dto)
        {
            var exists = await _userRepository.EmailExistsAsync(dto.Email);

            if (exists)
            {
                return null;
            }

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

        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto).ToList();
        }

        public async Task<List<UserResponseDto>> GetUsersByRole(UserRole role)
        {
            var users = await _userRepository.GetActiveByRoleAsync(role);
            return users.Select(MapToDto).ToList();
        }

        public async Task<UserResponseDto?> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return MapToDto(user);
        }

        public async Task<UserResponseDto?> UpdateRole(int userId, UserRole newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            user.Role = newRole;
            await _userRepository.SaveChangesAsync();
            return MapToDto(user);
        }

        public async Task<bool> SetActiveStatus(int userId, bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.IsActive = isActive;
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<AppUser?> ValidateLogin(LoginDto dto)
        {
            var user = await _userRepository.GetActiveByEmailAsync(dto.Email);

            if (user == null)
            {
                return null;
            }

            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (valid)
            {
                return user;
            }
            return null;
        }

        private UserResponseDto MapToDto(AppUser u)
        {
            var dto = new UserResponseDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role.ToString(),
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            };
            return dto;
        }
    }
}
