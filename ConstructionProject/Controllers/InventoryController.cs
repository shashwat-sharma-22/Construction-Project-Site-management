using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class InventoryController : Controller
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var model = new InventoryIndexViewModel
            {
                Equipments = await _service.GetAllEquipmentAsync(),
                Materials = await _service.GetAllMaterialsAsync()
            };

            return View(model);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromForm] string? itemType,
            [FromForm] string? equipmentName,
            [FromForm] string? materialName,
            [FromForm] string? status,
            [FromForm] decimal quantity,
            [FromForm] decimal unitCost,
            [FromForm] string? unit)
        {
            if (string.IsNullOrWhiteSpace(itemType))
            {
                ModelState.AddModelError("", "Item type is required");
                return View();
            }

            if (itemType == "equipment")
            {
                if (string.IsNullOrWhiteSpace(equipmentName))
                {
                    ModelState.AddModelError("", "Equipment name is required");
                    return View();
                }

                if (string.IsNullOrWhiteSpace(status) || !System.Enum.TryParse<EquipmentStatus>(status, true, out var parsedStatus))
                {
                    ModelState.AddModelError("", "Valid equipment status is required");
                    return View();
                }

                var equipment = new Equipment
                {
                    Name = equipmentName,
                    Status = parsedStatus
                };
                await _service.AddEquipmentAsync(equipment);
            }
            else if (itemType == "material")
            {
                if (string.IsNullOrWhiteSpace(materialName))
                {
                    ModelState.AddModelError("", "Material name is required");
                    return View();
                }

                var material = new Material
                {
                    Name = materialName,
                    QuantityAvailable = quantity,
                    UnitCost = unitCost
                };
                await _service.AddMaterialAsync(material);
            }
            else
            {
                ModelState.AddModelError("", "Unsupported item type");
                return View();
            }

            return RedirectToAction("Index");
        }
    }
}
