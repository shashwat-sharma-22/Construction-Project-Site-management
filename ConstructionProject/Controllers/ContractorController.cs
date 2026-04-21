using System;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProject.DTOs;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class ContractorController : Controller
    {
        private readonly IContractorService _service;
        private readonly IUserService _userService;

        public ContractorController(IContractorService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? search)
        {
            var userRole = GetUserRole();
            if (userRole == "Contractor")
                return RedirectToAction("Index", "Home");

            var contractors = string.IsNullOrWhiteSpace(search)
                ? await _service.GetAllContractorsAsync()
                : await _service.SearchContractorsAsync(search.Trim());

            ViewData["CurrentSearch"] = search;
            return View(contractors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var userRole = GetUserRole();
            if (userRole == "Contractor")
                return RedirectToAction("Index", "Home");

            var contractor = await _service.GetContractorDetailsAsync(id);
            if (contractor == null) return NotFound();
            return View(contractor);
        }

        [HttpGet("create")]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            if (GetUserRole() == "Contractor")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] Contractor contractor)
        {
            if (GetUserRole() == "Contractor")
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var created = await _service.AddContractorAsync(contractor);

                if (!string.IsNullOrWhiteSpace(contractor.ContactInfo))
                {
                    var existingUsers = await _userService.GetAllUsers();
                    var existingUser = existingUsers.FirstOrDefault(u =>
                        !string.IsNullOrWhiteSpace(u.Email) &&
                        string.Equals(u.Email, contractor.ContactInfo, StringComparison.OrdinalIgnoreCase));

                    if (existingUser == null)
                    {
                        await _userService.RegisterUser(new RegisterUserDto
                        {
                            Name = contractor.ContractorName,
                            Email = contractor.ContactInfo,
                            Password = "Contractor@123",
                            Role = UserRole.Contractor
                        });
                    }
                    else if (!string.Equals(existingUser.Role, UserRole.Contractor.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        await _userService.UpdateRole(existingUser.UserId, UserRole.Contractor);
                    }
                }

                return RedirectToAction(nameof(Details), new { id = created.ContractorId });
            }
            return View(contractor);
        }

        [HttpGet("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (GetUserRole() == "Contractor")
                return RedirectToAction("Index", "Home");

            var contractor = await _service.GetContractorDetailsAsync(id);
            if (contractor == null) return NotFound();
            return View(contractor);
        }

        [HttpPost("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [FromForm] Contractor contractor)
        {
            if (GetUserRole() == "Contractor")
                return RedirectToAction("Index", "Home");

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (GetUserRole() == "Contractor")
                return RedirectToAction("Index", "Home");

            var ok = await _service.DeleteContractorAsync(id);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        // ===== Workforce Management (for Contractor role) =====

        [HttpGet("workforce")]
        public async Task<IActionResult> Workforce(string? search)
        {
            var userRole = GetUserRole();
            if (userRole != "Contractor")
                return RedirectToAction(nameof(Index));

            var contractor = await GetCurrentContractorAsync();
            if (contractor == null)
            {
                ViewData["NoContractor"] = true;
                return View(new Contractor());
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var filtered = await _service.SearchWorkforceAsync(contractor.ContractorId, search.Trim());
                contractor.Workforces = filtered.ToList();
            }

            ViewData["CurrentSearch"] = search;
            return View(contractor);
        }

        [HttpGet("workforce/add")]
        public async Task<IActionResult> AddWorker()
        {
            if (GetUserRole() != "Contractor")
                return RedirectToAction(nameof(Index));

            var contractor = await GetCurrentContractorAsync();
            if (contractor == null) return RedirectToAction("Workforce");

            ViewData["ContractorId"] = contractor.ContractorId;
            ViewData["ContractorName"] = contractor.ContractorName;
            return View(new Workforce { ContractorId = contractor.ContractorId });
        }

        [HttpPost("workforce/add")]
        public async Task<IActionResult> AddWorker([FromForm] Workforce worker)
        {
            if (GetUserRole() != "Contractor")
                return RedirectToAction(nameof(Index));

            var contractor = await GetCurrentContractorAsync();
            if (contractor == null) return RedirectToAction("Workforce");

            worker.ContractorId = contractor.ContractorId;

            if (ModelState.IsValid)
            {
                await _service.AssignContractorAsync(contractor.ContractorId, worker);
                return RedirectToAction("Workforce");
            }

            ViewData["ContractorId"] = contractor.ContractorId;
            ViewData["ContractorName"] = contractor.ContractorName;
            return View(worker);
        }

        [HttpPost("workforce/remove/{id}")]
        public async Task<IActionResult> RemoveWorker(int id)
        {
            if (GetUserRole() != "Contractor")
                return RedirectToAction(nameof(Index));

            var contractor = await GetCurrentContractorAsync();
            if (contractor == null) return RedirectToAction("Workforce");

            await _service.RemoveWorkerAsync(id, contractor.ContractorId);
            return RedirectToAction("Workforce");
        }

        private string GetUserRole()
        {
            return User.FindFirst("role")?.Value ?? Request.Cookies["userRole"] ?? "User";
        }

        private string GetUserEmail()
        {
            return User.FindFirst("email")?.Value ?? Request.Cookies["userEmail"] ?? "";
        }

        private async Task<Contractor?> GetCurrentContractorAsync()
        {
            var email = GetUserEmail();
            if (string.IsNullOrEmpty(email)) return null;
            return await _service.GetContractorByEmailAsync(email);
        }
    }
}
