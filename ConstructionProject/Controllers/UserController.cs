using ConstructionProject.DTOs;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly IJwtTokenService _tokenService;
        private readonly IContractorService _contractorService;

        public UserController(IUserService service, IJwtTokenService tokenService, IContractorService contractorService)
        {
            _service = service;
            _tokenService = tokenService;
            _contractorService = contractorService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _service.ValidateLogin(dto);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            var token = _tokenService.GenerateToken(user);
            var response = new LoginResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };

            return Ok(response);
        }

<<<<<<< HEAD
        [HttpGet("create")]
=======
        [HttpPost("register")]
>>>>>>> d7d5705477dedea1a66915168bbb15c8f9d75615
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST api/user/register  — Admin only
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _service.RegisterUser(dto);

            if (user == null)
                return Conflict(new { message = "Email already exists." });

            if (dto.Role == UserRole.Contractor && !string.IsNullOrWhiteSpace(dto.Email))
            {
                var existingContractor = await _contractorService.GetContractorByEmailAsync(dto.Email);
                if (existingContractor == null)
                {
                    await _contractorService.AddContractorAsync(new Contractor
                    {
                        ContractorName = dto.Name,
                        ContactInfo = dto.Email,
                        Specialization = "General"
                    });
                }
            }

            return Ok(user);
        }

        [HttpGet("")]
        [HttpGet("index")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _service.GetAllUsers();
            return View(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _service.GetUserById(id);
            if (user == null)
                return NotFound(new { message = $"User {id} not found." });
            return View(user);
        }

<<<<<<< HEAD
        // GET api/user/role/SiteEngineer  — Admin only
=======
>>>>>>> d7d5705477dedea1a66915168bbb15c8f9d75615
        [HttpGet("role/{role}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByRole(UserRole role)
        {
            var users = await _service.GetUsersByRole(role);
            return View(users);
        }

<<<<<<< HEAD
        // GET api/user/edit/5  — Admin only
=======
>>>>>>> d7d5705477dedea1a66915168bbb15c8f9d75615
        [HttpGet("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _service.GetUserById(id);
            if (user == null)
                return NotFound(new { message = $"User {id} not found." });
            return View(user);
        }

        // PUT api/user/5/role  — Admin only
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto dto)
        {
            var user = await _service.UpdateRole(id, dto.NewRole);
            if (user == null)
                return NotFound(new { message = $"User {id} not found." });
            return Ok(user);
        }

        [HttpPut("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _service.SetActiveStatus(id, false);
            if (!result)
                return NotFound(new { message = $"User {id} not found." });
            return RedirectToAction("Index");
        }

        [HttpPut("{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _service.SetActiveStatus(id, true);
            if (!result)
                return NotFound(new { message = $"User {id} not found." });
            return RedirectToAction("Index");
        }

        [HttpPost("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteUser(id);
            if (!result)
                return NotFound(new { message = $"User {id} not found." });
            return RedirectToAction("Index");
        }
    }
}
