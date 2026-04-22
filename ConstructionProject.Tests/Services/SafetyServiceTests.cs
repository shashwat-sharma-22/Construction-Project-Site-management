using NUnit.Framework;
using Moq;
using ConstructionProject.Services;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Tests.Services
{
    [TestFixture]
    public class SafetyServiceTests
    {
        private Mock<ISafetyRepository> _mockRepo = null!;
        private SafetyService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ISafetyRepository>();
            _service = new SafetyService(_mockRepo.Object);
        }

        [Test]
        public async Task RecordInspection_ValidProject_Returns()
        {
            _mockRepo.Setup(r => r.ProjectExistsAsync(1)).ReturnsAsync(true);
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<SafetyInspection>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.RecordInspectionAsync(new SafetyInspection { ProjectId = 1 });

            Assert.That(result.ProjectId, Is.EqualTo(1));
        }

        [Test]
        public void RecordInspection_InvalidProject_Throws()
        {
            _mockRepo.Setup(r => r.ProjectExistsAsync(99)).ReturnsAsync(false);

            Assert.ThrowsAsync<Exception>(async () =>
                await _service.RecordInspectionAsync(new SafetyInspection { ProjectId = 99 }));
        }

        [Test]
        public async Task GetInspectionById_ValidId_Returns()
        {
            _mockRepo.Setup(r => r.GetByIdWithProjectAsync(1))
                .ReturnsAsync(new SafetyInspection { InspectionId = 1 });

            var result = await _service.GetInspectionByIdAsync(1);
            Assert.That(result, Is.Not.Null);
        }
    }
}
