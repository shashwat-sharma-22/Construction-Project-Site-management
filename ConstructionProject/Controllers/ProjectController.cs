using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _service;

        public ProjectController(ProjectService service)
        {
            _service = service;
        }

        [HttpPost("createProject")]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (project == null) return BadRequest();

            var created = await _service.CreateProjectAsync(project);
            return CreatedAtAction(nameof(GetProjectDetails), new { id = created.ProjectId }, created);
        }

        [HttpPut("updateProjectPlan/{id}")]
        public async Task<IActionResult> UpdateProjectPlan(int id, [FromBody] Project project)
        {
            if (project == null) return BadRequest();

            var ok = await _service.UpdateProjectPlanAsync(id, project);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpGet("getProjectDetails/{id}")]
        public async Task<IActionResult> GetProjectDetails(int id)
        {
            var project = await _service.GetProjectDetailsAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }
    }
}
