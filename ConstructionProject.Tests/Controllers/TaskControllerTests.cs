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
    public class TaskControllerTests
    {
        private Mock<ITaskService> _mockTaskService = null!;
        private Mock<IProjectService> _mockProjectService = null!;
        private Mock<IContractorService> _mockContractorService = null!;
        private TaskController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockProjectService = new Mock<IProjectService>();
            _mockContractorService = new Mock<IContractorService>();
            _controller = new TaskController(_mockTaskService.Object, _mockProjectService.Object, _mockContractorService.Object);
            var claims = new List<Claim> { new("role", "Admin") };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"))
                }
            };
        }

        [TearDown]
        public void TearDown() => _controller?.Dispose();

        [Test]
        public async Task Index_ValidProject_ReturnsTasks()
        {
            _mockProjectService.Setup(s => s.GetProjectDetailsAsync(1))
                .ReturnsAsync(new Project { ProjectId = 1, ProjectName = "P1" });
            _mockTaskService.Setup(s => s.GetTasksByProjectAsync(1))
                .ReturnsAsync(new List<ProjectTask> { new() { TaskId = 1, TaskName = "T1" } });

            var result = await _controller.Index(1) as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Index_InvalidProject_ReturnsNotFound()
        {
            _mockProjectService.Setup(s => s.GetProjectDetailsAsync(99)).ReturnsAsync((Project?)null);
            Assert.That(await _controller.Index(99), Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Details_ValidId_ReturnsTask()
        {
            _mockTaskService.Setup(s => s.GetTaskByIdAsync(1))
                .ReturnsAsync(new ProjectTask { TaskId = 1, TaskName = "T1" });

            var result = await _controller.Details(1) as ViewResult;

            Assert.That((result!.Model as ProjectTask)!.TaskId, Is.EqualTo(1));
        }
    }
}
