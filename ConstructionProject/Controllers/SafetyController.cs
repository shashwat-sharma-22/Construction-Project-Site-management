using System;
using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class SafetyController : Controller
    {
        private readonly ISafetyService _service;

        public SafetyController(ISafetyService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var inspections = await _service.GetAllInspectionsAsync();
            return View(inspections);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var inspection = await _service.GetInspectionByIdAsync(id);
            if (inspection == null)
            {
                return NotFound();
            }
            return View(inspection);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] SafetyInspection inspection)
        {
            if (ModelState.IsValid)
            {
                var created = await _service.RecordInspectionAsync(inspection);
                return RedirectToAction("Details", new { id = created.InspectionId });
            }
            return View(inspection);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var inspection = await _service.GetInspectionByIdAsync(id);
            if (inspection == null)
            {
                return NotFound();
            }
            return View(inspection);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] SafetyInspection inspection)
        {
            if (id != inspection.InspectionId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var updated = await _service.UpdateInspectionAsync(id, inspection);
                if (!updated)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            return View(inspection);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteInspectionAsync(id);
            if (!ok)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
    }
}
