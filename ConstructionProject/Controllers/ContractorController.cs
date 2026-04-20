using System.Threading.Tasks;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class ContractorController : Controller
    {
        private readonly IContractorService _service;

        public ContractorController(IContractorService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? search)
        {
            var userRole = GetUserRole();
            if (userRole == "Contractor")
            {
                return RedirectToAction("Index", "Home");
            }

            List<Contractor> contractors;
            if (string.IsNullOrWhiteSpace(search))
            {
                contractors = (await _service.GetAllContractorsAsync()).ToList();
            }
            else
            {
                contractors = (await _service.SearchContractorsAsync(search.Trim())).ToList();
            }

            ViewData["CurrentSearch"] = search;
            return View(contractors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var userRole = GetUserRole();
            if (userRole == "Contractor")
            {
                return RedirectToAction("Index", "Home");
            }

            var contractor = await _service.GetContractorDetailsAsync(id);
            if (contractor == null)
            {
                return NotFound();
            }
            return View(contractor);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Contractor contractor)
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var created = await _service.AddContractorAsync(contractor);
                return RedirectToAction("Details", new { id = created.ContractorId });
            }
            return View(contractor);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index", "Home");
            }

            var contractor = await _service.GetContractorDetailsAsync(id);
            if (contractor == null)
            {
                return NotFound();
            }
            return View(contractor);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] Contractor contractor)
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != contractor.ContractorId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var ok = await _service.UpdateContractorAsync(id, contractor);
                if (!ok)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            return View(contractor);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (GetUserRole() == "Contractor")
            {
                return RedirectToAction("Index", "Home");
            }

            var ok = await _service.DeleteContractorAsync(id);
            if (!ok)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpGet("workforce")]
        public async Task<IActionResult> Workforce(string? search)
        {
            var userRole = GetUserRole();
            if (userRole != "Contractor")
            {
                return RedirectToAction("Index");
            }

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
            {
                return RedirectToAction("Index");
            }

            var contractor = await GetCurrentContractorAsync();
            if (contractor == null)
            {
                return RedirectToAction("Workforce");
            }

            ViewData["ContractorId"] = contractor.ContractorId;
            ViewData["ContractorName"] = contractor.ContractorName;
            return View(new Workforce { ContractorId = contractor.ContractorId });
        }

        [HttpPost("workforce/add")]
        public async Task<IActionResult> AddWorker([FromForm] Workforce worker)
        {
            if (GetUserRole() != "Contractor")
            {
                return RedirectToAction("Index");
            }

            var contractor = await GetCurrentContractorAsync();
            if (contractor == null)
            {
                return RedirectToAction("Workforce");
            }

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
            {
                return RedirectToAction("Index");
            }

            var contractor = await GetCurrentContractorAsync();
            if (contractor == null)
            {
                return RedirectToAction("Workforce");
            }

            await _service.RemoveWorkerAsync(id, contractor.ContractorId);
            return RedirectToAction("Workforce");
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
            return await _service.GetContractorByEmailAsync(email);
        }
    }
}
