using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class ContractorController : Controller
    {
        private readonly ContractorService _service;

        public ContractorController(ContractorService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var contractors = await _service.GetAllContractorsAsync();
            return View(contractors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var contractor = await _service.GetContractorDetailsAsync(id);
            if (contractor == null) return NotFound();
            return View(contractor);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Contractor contractor)
        {
            if (ModelState.IsValid)
            {
                var created = await _service.AddContractorAsync(contractor);
                return RedirectToAction(nameof(Details), new { id = created.ContractorId });
            }
            return View(contractor);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var contractor = await _service.GetContractorDetailsAsync(id);
            if (contractor == null) return NotFound();
            return View(contractor);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Contractor contractor)
        {
            if (id != contractor.ContractorId) return BadRequest();

            if (ModelState.IsValid)
            {
                var ok = await _service.UpdateContractorAsync(id, contractor);
                if (!ok) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(contractor);
        }


        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteContractorAsync(id);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
