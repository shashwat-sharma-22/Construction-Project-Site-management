using ConstructionProject.Models;
using Microsoft.EntityFrameworkCore;
namespace ConstructionProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Workforce> Workforces { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<SafetyInspection> SafetyInspections { get; set; }
        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                UserId = 1,
                Name = "Super Admin",
                Email = "admin@construction.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 26, 11, 15, 52, 337, DateTimeKind.Utc)
            });
        }

    }
}
