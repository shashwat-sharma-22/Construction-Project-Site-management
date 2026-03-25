using System.ComponentModel.DataAnnotations;

namespace ConstructionProject.Models
{
    public enum EquipmentStatus
    {
        AVAILABLE,
        IN_USE,
        MAINTENANCE
    }

    public class Equipment
    {
        [Key]
        public int EquipmentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public EquipmentStatus Status { get; set; }
    }
}
