using NUnit.Framework;
using Moq;
using ConstructionProject.Controllers;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Tests.Controllers
{
    [TestFixture]
    public class SafetyControllerTests
    {
        private Mock<ISafetyService> _mockService = null!;
        private SafetyController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ISafetyService>();
            _controller = new SafetyController(_mockService.Object);
        }

        [TearDown]
        public void TearDown() => _controller?.Dispose();

        [Test]
        public async Task Index_ReturnsAllInspections()
        {
            _mockService.Setup(s => s.GetAllInspectionsAsync())
                .ReturnsAsync(new List<SafetyInspection> { new() { InspectionId = 1 } });

            var result = await _controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Details_ValidId_ReturnsInspection()
        {
            _mockService.Setup(s => s.GetInspectionByIdAsync(1))
                .ReturnsAsync(new SafetyInspection { InspectionId = 1 });

            var result = await _controller.Details(1) as ViewResult;

            Assert.That((result!.Model as SafetyInspection)!.InspectionId, Is.EqualTo(1));
        }

        [Test]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            _mockService.Setup(s => s.GetInspectionByIdAsync(99)).ReturnsAsync((SafetyInspection?)null);
            Assert.That(await _controller.Details(99), Is.InstanceOf<NotFoundResult>());
        }
    }
}
