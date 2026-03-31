using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ConstructionProject.Models;

namespace ConstructionProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Index()
        {
            // Check if user has auth token in cookie
            var hasAuthToken = Request.Cookies.ContainsKey("authToken");

            if (!hasAuthToken && !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get user email from cookie claim if available
            var userEmail = User.FindFirst("email")?.Value ?? Request.Cookies["userEmail"] ?? "Guest";
            var userRole = User.FindFirst("role")?.Value ?? Request.Cookies["userRole"] ?? "User";

            ViewBag.UserEmail = userEmail;
            ViewBag.UserRole = userRole;
            ViewBag.UserName = userEmail;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
