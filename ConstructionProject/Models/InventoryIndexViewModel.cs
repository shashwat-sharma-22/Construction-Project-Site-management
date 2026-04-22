using System.Collections.Generic;

namespace ConstructionProject.Models
{
    public class InventoryIndexViewModel
    {
        public List<Equipment> Equipments { get; set; } = new();
        public List<Material> Materials { get; set; } = new();
    }
}
