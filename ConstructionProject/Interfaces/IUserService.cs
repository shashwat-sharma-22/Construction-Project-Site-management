using ConstructionProject.DTOs;
using ConstructionProject.Models;

namespace ConstructionProject.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto?> RegisterUser(RegisterUserDto dto);
        Task<List<UserResponseDto>> GetAllUsers();
        Task<List<UserResponseDto>> GetUsersByRole(UserRole role);
        Task<UserResponseDto?> GetUserById(int id);
        Task<UserResponseDto?> UpdateRole(int userId, UserRole newRole);
        Task<bool> SetActiveStatus(int userId, bool isActive);
        Task<bool> DeleteUser(int userId);
        Task<AppUser?> ValidateLogin(LoginDto dto);
    }
}