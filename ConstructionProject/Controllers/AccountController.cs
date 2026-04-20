using Microsoft.AspNetCore.Mvc;
using ConstructionProject.DTOs;
using ConstructionProject.Interfaces;
using System.Threading.Tasks;

namespace ConstructionProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public AccountController(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet("Account/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Account/Login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }
                


            var user = await _userService.ValidateLogin(loginDto);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(loginDto);
            }

            var token = _jwtTokenService.GenerateToken(user);

            // Store token in cookie
            Response.Cookies.Append("authToken", token, new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                Expires = System.DateTimeOffset.UtcNow.AddHours(1)
            });

            // Store user info in cookies for easier access
            Response.Cookies.Append("userEmail", user.Email ?? "", new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                Expires = System.DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("userRole", user.Role.ToString(), new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                Expires = System.DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("userId", user.UserId.ToString(), new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                Expires = System.DateTimeOffset.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Account/Profile")]
        public async Task<IActionResult> Profile()
        {
            var userEmail = Request.Cookies["userEmail"];
            if (string.IsNullOrEmpty(userEmail))
                return RedirectToAction("Login");

            var users = await _userService.GetAllUsers();
            var userDto = users.FirstOrDefault(u => u.Email == userEmail);
            if (userDto == null)
                return NotFound();

            return View(userDto);
        }

        [HttpPost("Account/Logout")]
        public IActionResult Logout()
        {
            // Delete all auth cookies
            Response.Cookies.Delete("authToken");
            Response.Cookies.Delete("userEmail");
            Response.Cookies.Delete("userRole");
            Response.Cookies.Delete("userId");

            return RedirectToAction("Login");
        }
    }
}
