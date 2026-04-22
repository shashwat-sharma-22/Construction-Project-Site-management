using NUnit.Framework;
using Moq;
using ConstructionProject.Services;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Tests.Services
{
    [TestFixture]
    public class ProgressServiceTests
    {
        private Mock<IProgressRepository> _mockRepo = null!;
        private ProgressService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IProgressRepository>();
            _service = new ProgressService(_mockRepo.Object);
        }

        [Test]
        public async Task GetAllProgress_ReturnsList()
        {
            _mockRepo.Setup(r => r.GetAllWithProjectAsync())
                .ReturnsAsync(new List<Progress> { new() { ProgressId = 1 } });

            var result = await _service.GetAllProgressAsync();
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task RecordProgress_SavesAndReturns()
        {
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Progress>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.RecordProgressAsync(new Progress { ProjectId = 1, CompletedTasks = 5 });

            Assert.That(result.CompletedTasks, Is.EqualTo(5));
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteProgress_InvalidId_ReturnsFalse()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Progress?)null);
            Assert.That(await _service.DeleteProgressAsync(99), Is.False);
        }
    }
}
