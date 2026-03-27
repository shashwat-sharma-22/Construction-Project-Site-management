using System.ComponentModel.DataAnnotations;

namespace ConstructionProject.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }   // never store plain password

        [Required]
        public UserRole Role { get; set; }          // Admin, ProjectManager, etc.

        public bool IsActive { get; set; } = true;  // Admin can deactivate users

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
