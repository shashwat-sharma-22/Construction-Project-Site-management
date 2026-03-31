using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly ProjectService _service;

        public ProjectController(ProjectService service)
        {
            _service = service;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            var projects = await _service.GetAllProjectsAsync();
            return View(projects);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> Details(int id)
        {
            var project = await _service.GetProjectDetailsAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Project project)
        {
            if (ModelState.IsValid)
            {
                var created = await _service.CreateProjectAsync(project);
                return RedirectToAction(nameof(Details), new { id = created.ProjectId });
            }
            return View(project);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _service.GetProjectDetailsAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Project project)
        {
            if (id != project.ProjectId) return BadRequest();

            if (ModelState.IsValid)
            {
                var ok = await _service.UpdateProjectPlanAsync(id, project);
                if (!ok) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteProjectAsync(id);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
