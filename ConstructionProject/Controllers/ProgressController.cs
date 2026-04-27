using System;
using System.Linq;
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
        private readonly IProjectService _projectService;
        private readonly IContractorService _contractorService;

        public ProgressController(IProgressService service, IProjectService projectService, IContractorService contractorService)
        {
            _service = service;
            _projectService = projectService;
            _contractorService = contractorService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userRole = GetUserRole();
            ViewData["UserRole"] = userRole;

            List<Progress> progressList;

            if (userRole == "Contractor")
            {
                var contractor = await GetCurrentContractorAsync();
                if (contractor == null)
                {
                    return View(new List<Progress>());
                }

                var projects = await _projectService.GetProjectsByContractorAsync(contractor.ContractorId);
                var projectIds = projects.Select(p => p.ProjectId);
                progressList = (await _service.GetProgressByProjectIdsAsync(projectIds)).ToList();
            }
            else
            {
                progressList = (await _service.GetAllProgressAsync()).ToList();
            }

            return View(progressList);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var progress = await _service.GetByIdAsync(id);
            if (progress == null)
            {
                return NotFound();
            }
            ViewData["UserRole"] = GetUserRole();
            return View(progress);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            if (GetUserRole() != "SiteEngineer")
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Progress progress)
        {
            if (GetUserRole() != "SiteEngineer")
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var created = await _service.RecordProgressAsync(progress);
                return RedirectToAction("Details", new { id = created.ProgressId });
            }
            return View(progress);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (GetUserRole() != "SiteEngineer")
            {
                return RedirectToAction("Index");
            }

            var progress = await _service.GetByIdAsync(id);
            if (progress == null)
            {
                return NotFound();
            }
            return View(progress);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Progress progress)
        {
            if (GetUserRole() != "SiteEngineer")
            {
                return RedirectToAction("Index");
            }

            if (id != progress.ProgressId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var updated = await _service.UpdateProgressAsync(id, progress);
                if (updated == null)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            return View(progress);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (GetUserRole() != "SiteEngineer")
            {
                return RedirectToAction("Index");
            }

            var ok = await _service.DeleteProgressAsync(id);
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
