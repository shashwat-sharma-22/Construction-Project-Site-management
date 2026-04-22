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
            var hasAuthToken = Request.Cookies.ContainsKey("authToken");

            if (!hasAuthToken && !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var emailClaim = User.FindFirst("email");
            string userEmail;
            if (emailClaim != null)
            {
                userEmail = emailClaim.Value;
            }
            else if (Request.Cookies["userEmail"] != null)
            {
                userEmail = Request.Cookies["userEmail"];
            }
            else
            {
                userEmail = "Guest";
            }

            var roleClaim = User.FindFirst("role");
            string userRole;
            if (roleClaim != null)
            {
                userRole = roleClaim.Value;
            }
            else if (Request.Cookies["userRole"] != null)
            {
                userRole = Request.Cookies["userRole"];
            }
            else
            {
                userRole = "User";
            }

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
            string requestId;
            if (Activity.Current != null)
            {
                requestId = Activity.Current.Id;
            }
            else
            {
                requestId = HttpContext.TraceIdentifier;
            }
            return View(new ErrorViewModel { RequestId = requestId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NotFound()
        {
            ViewData["RequestPath"] = Request.Path;
            return View();
        }
    }
}
