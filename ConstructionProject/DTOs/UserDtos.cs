using System.ComponentModel.DataAnnotations;
using ConstructionProject.Models;

namespace ConstructionProject.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }

        [Required]
        public UserRole Role { get; set; }  // 0=Admin 1=ProjectManager 2=SiteEngineer 3=Contractor 4=SafetyOfficer
    }

    // Used for login
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }

    // Used to update a user's role
    public class UpdateRoleDto
    {
        [Required]
        public UserRole NewRole { get; set; }
    }

    // What we send back — never expose PasswordHash
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Response after successful login
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}
