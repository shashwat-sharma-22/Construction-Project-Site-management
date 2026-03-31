using ConstructionProject.DTOs;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _service;
        private readonly JwtTokenService _tokenService;

        public UserController(UserService service, JwtTokenService tokenService)
        {
            _service = service;
            _tokenService = tokenService;
        }

        // POST api/user/login  — Public (no auth required)
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

        // POST api/user/register  — Admin only
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _service.RegisterUser(dto);

            if (user == null)
                return Conflict(new { message = "Email already exists." });

            return View(user);
        }

        // GET api/user  — Admin only
        [HttpGet("")]
        [HttpGet("index")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _service.GetAllUsers();
            return View(users);
        }

        // GET api/user/5  — Admin only
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _service.GetUserById(id);
            if (user == null)
                return NotFound(new { message = $"User {id} not found." });
            return View(user);
        }

        // GET api/user/role/SiteEngineer  — Admin, ProjectManager
        [HttpGet("role/{role}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> GetByRole(UserRole role)
        {
            var users = await _service.GetUsersByRole(role);
            return View(users);
        }

        // PUT api/user/5/role  — Admin only
        [HttpGet("edit/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(int id,
                                                    [FromBody] UpdateRoleDto dto)
        {
            var user = await _service.UpdateRole(id, dto.NewRole);
            if (user == null)
                return NotFound(new { message = $"User {id} not found." });
            return View(user);
        }

        // PUT api/user/5/deactivate  — Admin only
        [HttpPut("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _service.SetActiveStatus(id, false);
            if (!result)
                return NotFound(new { message = $"User {id} not found." });
            return RedirectToAction("Index");
        }

        // PUT api/user/5/activate  — Admin only
        [HttpPut("{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _service.SetActiveStatus(id, true);
            if (!result)
                return NotFound(new { message = $"User {id} not found." });
            return RedirectToAction("Index");
        }

        // DELETE api/user/5  — Admin only
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
