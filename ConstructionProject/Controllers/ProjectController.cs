using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;
        private readonly IContractorService _contractorService;

        public ProjectController(IProjectService service, IContractorService contractorService)
        {
            _service = service;
            _contractorService = contractorService;
        }

        [HttpGet("")]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string? search)
        {
            var userRole = GetUserRole();
            ViewData["UserRole"] = userRole;

            List<Project> projects;

            if (userRole == "Contractor")
            {
                var contractor = await GetCurrentContractorAsync();
                if (contractor == null)
                {
                    return View(new List<Project>());
                }

                if (string.IsNullOrWhiteSpace(search))
                {
                    projects = (await _service.GetProjectsByContractorAsync(contractor.ContractorId)).ToList();
                }
                else
                {
                    projects = (await _service.SearchProjectsByContractorAsync(contractor.ContractorId, search.Trim())).ToList();
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(search))
                {
                    projects = (await _service.GetAllProjectsAsync()).ToList();
                }
                else
                {
                    projects = (await _service.SearchProjectsAsync(search.Trim())).ToList();
                }
            }

            ViewData["CurrentSearch"] = search;
            return View(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _service.GetProjectDetailsAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["UserRole"] = GetUserRole();
            return View(project);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index");
            }

            ViewBag.Contractors = new SelectList(
                await _contractorService.GetAllContractorsAsync(),
                "ContractorId", "ContractorName");
            return View(new Project { startDate = DateTime.Today, endDate = DateTime.Today });
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Project project)
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var created = await _service.CreateProjectAsync(project);
                return RedirectToAction("Details", new { id = created.ProjectId });
            }

            ViewBag.Contractors = new SelectList(
                await _contractorService.GetAllContractorsAsync(),
                "ContractorId", "ContractorName");
            return View(project);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index");
            }

            var project = await _service.GetProjectDetailsAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            ViewBag.Contractors = new SelectList(
                await _contractorService.GetAllContractorsAsync(),
                "ContractorId", "ContractorName", project.ContractorId);
            return View(project);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Project project)
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index");
            }

            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var ok = await _service.UpdateProjectPlanAsync(id, project);
                if (!ok)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }

            ViewBag.Contractors = new SelectList(
                await _contractorService.GetAllContractorsAsync(),
                "ContractorId", "ContractorName", project.ContractorId);
            return View(project);
        }

        [HttpGet("assign-contractor")]
        public async Task<IActionResult> AssignContractor(string? search)
        {
            var userRole = GetUserRole();
            if (userRole != "Admin" && userRole != "ProjectManager")
            {
                return RedirectToAction("Index");
            }

            ViewData["CurrentSearch"] = search;
            ViewBag.Contractors = new SelectList(
                await _contractorService.GetAllContractorsAsync(),
                "ContractorId", "ContractorName");

            List<Project> projects;
            if (!string.IsNullOrWhiteSpace(search))
            {
                projects = (await _service.SearchProjectsAsync(search.Trim())).ToList();
            }
            else
            {
                projects = (await _service.GetAllProjectsAsync()).ToList();
            }

            return View(projects);
        }

        [HttpPost("assign-contractor")]
        public async Task<IActionResult> AssignContractor(int projectId, int? contractorId)
        {
            var userRole = GetUserRole();
            if (userRole != "Admin" && userRole != "ProjectManager")
            {
                return RedirectToAction("Index");
            }

            var project = await _service.GetProjectDetailsAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            project.ContractorId = contractorId;
            await _service.UpdateProjectPlanAsync(projectId, project);

            if (contractorId.HasValue)
            {
                TempData["SuccessMessage"] = "Contractor assigned to project '" + project.ProjectName + "' successfully.";
            }
            else
            {
                TempData["SuccessMessage"] = "Contractor removed from project '" + project.ProjectName + "' successfully.";
            }

            return RedirectToAction("AssignContractor");
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index");
            }

            var ok = await _service.DeleteProjectAsync(id);
            if (!ok)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        private string GetUserRole()
        {
            var roleClaim = User.FindFirst("role");
            if (roleClaim != null)
            {
                return roleClaim.Value;
            }

            var roleCookie = Request.Cookies["userRole"];
            if (roleCookie != null)
            {
                return roleCookie;
            }

            return "User";
        }

        private string GetUserEmail()
        {
            var emailClaim = User.FindFirst("email");
            if (emailClaim != null)
            {
                return emailClaim.Value;
            }

            var emailCookie = Request.Cookies["userEmail"];
            if (emailCookie != null)
            {
                return emailCookie;
            }

            return "";
        }

        private async Task<Contractor?> GetCurrentContractorAsync()
        {
            var email = GetUserEmail();
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            return await _contractorService.GetContractorByEmailAsync(email);
        }
    }
}
