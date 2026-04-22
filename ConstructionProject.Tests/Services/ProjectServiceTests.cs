using NUnit.Framework;
using Moq;
using ConstructionProject.Services;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Tests.Services
{
    [TestFixture]
    public class ProjectServiceTests
    {
        private Mock<IProjectRepository> _mockRepo = null!;
        private ProjectService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IProjectRepository>();
            _service = new ProjectService(_mockRepo.Object);
        }

        [Test]
        public async Task GetAllProjects_ReturnsList()
        {
            _mockRepo.Setup(r => r.GetAllWithContractorAsync())
                .ReturnsAsync(new List<Project> { new() { ProjectId = 1 } });

            var result = await _service.GetAllProjectsAsync();
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task CreateProject_CallsSaveAndReturns()
        {
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Project>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.CreateProjectAsync(new Project { ProjectName = "Test" });

            Assert.That(result.ProjectName, Is.EqualTo("Test"));
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteProject_ValidId_ReturnsTrue()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Project { ProjectId = 1 });
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            Assert.That(await _service.DeleteProjectAsync(1), Is.True);
        }

        [Test]
        public async Task DeleteProject_InvalidId_ReturnsFalse()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Project?)null);
            Assert.That(await _service.DeleteProjectAsync(99), Is.False);
        }
    }
}
