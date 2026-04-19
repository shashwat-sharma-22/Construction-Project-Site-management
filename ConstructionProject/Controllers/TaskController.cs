using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly ProjectService _projectService;
        private readonly ContractorService _contractorService;

        public TaskController(TaskService taskService, ProjectService projectService, ContractorService contractorService)
        {
            _taskService = taskService;
            _projectService = projectService;
            _contractorService = contractorService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> Index(int projectId, string? search)
        {
            var project = await _projectService.GetProjectDetailsAsync(projectId);
            if (project == null) return NotFound();

            var tasks = string.IsNullOrWhiteSpace(search)
                ? await _taskService.GetTasksByProjectAsync(projectId)
                : await _taskService.SearchTasksAsync(projectId, search.Trim());

            ViewData["CurrentSearch"] = search;
            ViewData["ProjectId"] = projectId;
            ViewData["ProjectName"] = project.ProjectName;
            ViewData["UserRole"] = GetUserRole();
            return View(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            ViewData["UserRole"] = GetUserRole();
            return View(task);
        }

        [HttpGet("create/{projectId}")]
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _projectService.GetProjectDetailsAsync(projectId);
            if (project == null) return NotFound();

            ViewData["ProjectId"] = projectId;
            ViewData["ProjectName"] = project.ProjectName;
            ViewData["UserRole"] = GetUserRole();
            await PopulateWorkforceDropdown(project);
            return View(new ProjectTask { ProjectId = projectId, Deadline = DateTime.Today });
        }

        [HttpPost("create/{projectId}")]
        public async Task<IActionResult> Create(int projectId, [FromForm] ProjectTask task)
        {
            task.ProjectId = projectId;

            if (ModelState.IsValid)
            {
                var created = await _taskService.CreateTaskAsync(task);
                return RedirectToAction(nameof(Index), new { projectId });
            }

            var project = await _projectService.GetProjectDetailsAsync(projectId);
            ViewData["ProjectId"] = projectId;
            ViewData["ProjectName"] = project?.ProjectName;
            ViewData["UserRole"] = GetUserRole();
            await PopulateWorkforceDropdown(project);
            return View(task);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            ViewData["ProjectId"] = task.ProjectId;
            ViewData["UserRole"] = GetUserRole();
            var project = await _projectService.GetProjectDetailsAsync(task.ProjectId);
            await PopulateWorkforceDropdown(project, task.WorkerId);
            return View(task);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] ProjectTask task)
        {
            if (id != task.TaskId) return BadRequest();

            if (ModelState.IsValid)
            {
                var ok = await _taskService.UpdateTaskAsync(id, task);
                if (!ok) return NotFound();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewData["ProjectId"] = task.ProjectId;
            ViewData["UserRole"] = GetUserRole();
            var proj = await _projectService.GetProjectDetailsAsync(task.ProjectId);
            await PopulateWorkforceDropdown(proj, task.WorkerId);
            return View(task);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            var projectId = task.ProjectId;
            await _taskService.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index), new { projectId });
        }

        private async System.Threading.Tasks.Task PopulateWorkforceDropdown(Project? project, int? selectedWorkerId = null)
        {
            if (project?.ContractorId != null)
            {
                var workforce = await _contractorService.GetWorkforceByContractorAsync(project.ContractorId.Value);
                ViewBag.Workforce = new SelectList(workforce, "WorkerId", "Name", selectedWorkerId);
            }
            else
            {
                ViewBag.Workforce = new SelectList(Enumerable.Empty<Workforce>(), "WorkerId", "Name");
            }
        }

        private string GetUserRole()
        {
            return User.FindFirst("role")?.Value ?? Request.Cookies["userRole"] ?? "User";
        }
    }
}
