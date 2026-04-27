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
    public class ContractorControllerTests
    {
        private Mock<IContractorService> _mockService = null!;
        private Mock<IUserService> _mockUserService = null!;
        private ContractorController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IContractorService>();
            _mockUserService = new Mock<IUserService>();
            _controller = new ContractorController(_mockService.Object, _mockUserService.Object);
            SetupUserRole("Admin");
        }

        [TearDown]
        public void TearDown() => _controller?.Dispose();

        [Test]
        public async Task Index_ReturnsAllContractors()
        {
            _mockService.Setup(s => s.GetAllContractorsAsync())
                .ReturnsAsync(new List<Contractor> { new() { ContractorId = 1, ContractorName = "C1" } });

            var result = await _controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Details_ValidId_ReturnsContractor()
        {
            _mockService.Setup(s => s.GetContractorDetailsAsync(1))
                .ReturnsAsync(new Contractor { ContractorId = 1, ContractorName = "C1" });

            var result = await _controller.Details(1) as ViewResult;

            Assert.That((result!.Model as Contractor)!.ContractorId, Is.EqualTo(1));
        }

        [Test]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetContractorDetailsAsync(99)).ReturnsAsync((Contractor?)null);
            Assert.That(await _controller.Details(99), Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_ValidId_Redirects()
        {
            _mockService.Setup(s => s.DeleteContractorAsync(1)).ReturnsAsync(true);
            var result = await _controller.Delete(1) as RedirectToActionResult;
            Assert.That(result!.ActionName, Is.EqualTo("Index"));
        }

        private void SetupUserRole(string role)
        {
            var claims = new List<Claim> { new("role", role), new("email", "admin@test.com") };
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
