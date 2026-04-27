using NUnit.Framework;
using Moq;
using ConstructionProject.Controllers;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Tests.Controllers
{
    [TestFixture]
    public class InventoryControllerTests
    {
        private Mock<IInventoryService> _mockService = null!;
        private InventoryController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IInventoryService>();
            _controller = new InventoryController(_mockService.Object);
        }

        [TearDown]
        public void TearDown() => _controller?.Dispose();

        [Test]
        public async Task Index_ReturnsView()
        {
            _mockService.Setup(s => s.GetAllEquipmentAsync()).ReturnsAsync(new List<Equipment>());
            _mockService.Setup(s => s.GetAllMaterialsAsync()).ReturnsAsync(new List<Material>());

            var result = await _controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Create_Get_ReturnsView()
        {
            var result = _controller.Create() as ViewResult;
            Assert.That(result, Is.Not.Null);
        }
    }
}
