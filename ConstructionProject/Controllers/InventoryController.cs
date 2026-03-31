using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class InventoryController : Controller
    {
        private readonly InventoryService _service;

        public InventoryController(InventoryService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var equipment = await _service.GetAllEquipmentAsync();
            var materials = await _service.GetAllMaterialsAsync();

            ViewBag.Equipment = equipment;
            ViewBag.Materials = materials;

            return View();
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] string itemType, [FromForm] string name, [FromForm] string status, [FromForm] int quantity, [FromForm] string unit)
        {
            if (itemType == "equipment")
            {
                var equipment = new Equipment
                {
                    Name = name,
                    Status = (EquipmentStatus)System.Enum.Parse(typeof(EquipmentStatus), status)
                };
                await _service.AddEquipmentAsync(equipment);
            }
            else if (itemType == "material")
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    ModelState.AddModelError("", "Name is required");
                    return View();
                }
                var material = new Material
                {
                    Name = name
                };
                await _service.AddMaterialAsync(material);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
