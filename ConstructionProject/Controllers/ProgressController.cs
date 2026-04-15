using System;
using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class ProgressController : Controller
    {
        private readonly IProgressService _service;

        public ProgressController(IProgressService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var progressList = await _service.GetAllProgressAsync();
            return View(progressList);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var progress = await _service.GetByIdAsync(id);
            if (progress == null) return NotFound();
            return View(progress);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Progress progress)
        {
            if (ModelState.IsValid)
            {
                var created = await _service.RecordProgressAsync(progress);
                return RedirectToAction(nameof(Details), new { id = created.ProgressId });
            }
            return View(progress);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var progress = await _service.GetByIdAsync(id);
            if (progress == null) return NotFound();
            return View(progress);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Progress progress)
        {
            if (id != progress.ProgressId) return BadRequest();

            if (ModelState.IsValid)
            {
                var updated = await _service.UpdateProgressAsync(id, progress);
                if (updated == null) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(progress);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteProgressAsync(id);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
