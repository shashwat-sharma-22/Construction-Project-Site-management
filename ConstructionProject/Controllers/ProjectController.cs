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
            ViewBag.UserRole = userRole;
            ViewData["CurrentSearch"] = search;

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
            return View(project);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index");
            }

            var allContractors = (await _contractorService.GetAllContractorsAsync()).ToList();
            var availableContractors = allContractors.Where(c => c.IsAssigned == false).ToList();

            ViewBag.Contractors = new SelectList(availableContractors, "ContractorId", "ContractorName");
            return View(new Project { startDate = DateTime.Today, endDate = DateTime.Today });
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Project project)
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index");
            }

            if (project.ContractorId.HasValue)
            {
                var contractor = await _contractorService.GetContractorDetailsAsync(project.ContractorId.Value);
                if (contractor != null && contractor.IsAssigned)
                {
                    ModelState.AddModelError("ContractorId", "This contractor is already assigned to another project.");
                }
            }

            if (ModelState.IsValid) 
            {
                var created = await _service.CreateProjectAsync(project);
                return RedirectToAction("Details", new { id = created.ProjectId });
            }

            var allContractorsForError = (await _contractorService.GetAllContractorsAsync()).ToList();
            var availableContractorsForError = allContractorsForError.Where(c => c.IsAssigned == false).ToList();

            ViewBag.Contractors = new SelectList(availableContractorsForError, "ContractorId", "ContractorName");
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

            var allContractorsForEdit = (await _contractorService.GetAllContractorsAsync()).ToList();
            var availableContractorsForEdit = allContractorsForEdit.Where(c => c.IsAssigned == false || c.ContractorId == project.ContractorId).ToList();

            ViewBag.Contractors = new SelectList(availableContractorsForEdit, "ContractorId", "ContractorName", project.ContractorId);
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

            if (project.ContractorId.HasValue)
            {
                var contractor = await _contractorService.GetContractorDetailsAsync(project.ContractorId.Value);
                var existingProject = await _service.GetProjectDetailsAsync(id);
                if (contractor != null && contractor.IsAssigned && existingProject?.ContractorId != project.ContractorId)
                {
                    ModelState.AddModelError("ContractorId", "This contractor is already assigned to another project.");
                }
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

            var allContractorsForEditError = (await _contractorService.GetAllContractorsAsync()).ToList();
            var availableContractorsForEditError = allContractorsForEditError.Where(c => c.IsAssigned == false || c.ContractorId == project.ContractorId).ToList();

            ViewBag.Contractors = new SelectList(availableContractorsForEditError, "ContractorId", "ContractorName", project.ContractorId);
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

            var allContractors = (await _contractorService.GetAllContractorsAsync()).ToList();
            ViewBag.AllContractors = allContractors;

            List<Project> projects;
            if (string.IsNullOrWhiteSpace(search))
            {
                projects = (await _service.GetAllProjectsAsync()).ToList();
            }
            else
            {
                projects = (await _service.SearchProjectsAsync(search.Trim())).ToList();
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

            if (contractorId.HasValue)
            {
                var contractor = await _contractorService.GetContractorDetailsAsync(contractorId.Value);
                if (contractor != null && contractor.IsAssigned && project.ContractorId != contractorId.Value)
                {
                    TempData["ErrorMessage"] = "This contractor is already assigned to another project.";
                    return RedirectToAction("AssignContractor");
                }
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

            return "";
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
