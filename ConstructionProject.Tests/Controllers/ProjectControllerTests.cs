using NUnit.Framework;
using Moq;
using ConstructionProject.Controllers;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ConstructionProject.Tests.Controllers
{
    [TestFixture]
    public class ProjectControllerTests
    {
        private Mock<IProjectService> _mockProjectService = null!;
        private Mock<IContractorService> _mockContractorService = null!;
        private ProjectController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockProjectService = new Mock<IProjectService>();
            _mockContractorService = new Mock<IContractorService>();
            _controller = new ProjectController(_mockProjectService.Object, _mockContractorService.Object);
            SetupUserRole("Admin", "admin@test.com");
        }

        [TearDown]
        public void TearDown() => _controller?.Dispose();

        [Test]
        public async Task Index_ReturnsAllProjects()
        {
            _mockProjectService.Setup(s => s.GetAllProjectsAsync()) 
                .ReturnsAsync(new List<Project> { new Project { ProjectId = 1, ProjectName = "P1" } });

            var result = await _controller.Index(null) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That((result!.Model as List<Project>)!.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Details_ValidId_ReturnsProject()
        {
            _mockProjectService.Setup(s => s.GetProjectDetailsAsync(1))
                .ReturnsAsync(new Project { ProjectId = 1, ProjectName = "Test" });

            var result = await _controller.Details(1) as ViewResult;

            Assert.That((result!.Model as Project)!.ProjectId, Is.EqualTo(1));
        }

        [Test]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            _mockProjectService.Setup(s => s.GetProjectDetailsAsync(99)).ReturnsAsync((Project?)null);
            Assert.That(await _controller.Details(99), Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_ValidProject_Redirects()
        {
            _mockProjectService.Setup(s => s.CreateProjectAsync(It.IsAny<Project>()))
                .ReturnsAsync(new Project { ProjectId = 1 });

            var result = await _controller.Create(new Project { ProjectName = "New" }) as RedirectToActionResult;

            Assert.That(result!.ActionName, Is.EqualTo("Details"));
        }

        [Test]
        public async Task Delete_ValidId_Redirects()
        {
            _mockProjectService.Setup(s => s.DeleteProjectAsync(1)).ReturnsAsync(true);
            var result = await _controller.Delete(1) as RedirectToActionResult;
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Delete_InvalidId_ReturnsNotFound()
        {
            _mockProjectService.Setup(s => s.DeleteProjectAsync(99)).ReturnsAsync(false);
            Assert.That(await _controller.Delete(99), Is.InstanceOf<NotFoundResult>());
        }

        private void SetupUserRole(string role, string email)
        {
            var claims = new List<Claim> { new("role", role), new("email", email) };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"))
                }
            };
        }
    }
}
