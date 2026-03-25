using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructionProject.Models
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal QuantityAvailable { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitCost { get; set; }
    }
}
