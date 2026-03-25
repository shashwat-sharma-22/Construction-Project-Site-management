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

    }
}
