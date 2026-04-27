using NUnit.Framework;
using Moq;
using ConstructionProject.Services;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Tests.Services
{
    [TestFixture]
    public class InventoryServiceTests
    {
        private Mock<IInventoryRepository> _mockRepo = null!;
        private InventoryService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IInventoryRepository>();
            _service = new InventoryService(_mockRepo.Object);
        }

        [Test]
        public async Task AddMaterial_SavesAndReturns()
        {
            _mockRepo.Setup(r => r.AddMaterialAsync(It.IsAny<Material>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.AddMaterialAsync(new Material { Name = "Cement" });

            Assert.That(result.Name, Is.EqualTo("Cement"));
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AddEquipment_SavesAndReturns()
        {
            _mockRepo.Setup(r => r.AddEquipmentAsync(It.IsAny<Equipment>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.AddEquipmentAsync(new Equipment { Name = "Crane" });

            Assert.That(result.Name, Is.EqualTo("Crane"));
        }

        [Test]
        public async Task GetAllMaterials_ReturnsList()
        {
            _mockRepo.Setup(r => r.GetAllMaterialsAsync())
                .ReturnsAsync(new List<Material> { new() { MaterialId = 1, Name = "Cement" } });

            var result = await _service.GetAllMaterialsAsync();
            Assert.That(result.Count, Is.EqualTo(1));
        }
    }
}
