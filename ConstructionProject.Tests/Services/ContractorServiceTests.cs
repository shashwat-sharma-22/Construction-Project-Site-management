using NUnit.Framework;
using Moq;
using ConstructionProject.Services;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;

namespace ConstructionProject.Tests.Services
{
    [TestFixture]
    public class ContractorServiceTests
    {
        private Mock<IContractorRepository> _mockRepo = null!;
        private ContractorService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IContractorRepository>();
            _service = new ContractorService(_mockRepo.Object);
        }

        [Test]
        public async Task GetAllContractors_ReturnsList()
        {
            _mockRepo.Setup(r => r.GetAllWithWorkforcesAsync())
                .ReturnsAsync(new List<Contractor> { new() { ContractorId = 1, ContractorName = "C1" } });

            var result = await _service.GetAllContractorsAsync();
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetContractorDetails_ValidId_Returns()
        {
            _mockRepo.Setup(r => r.GetByIdWithWorkforcesAsync(1))
                .ReturnsAsync(new Contractor { ContractorId = 1 });

            var result = await _service.GetContractorDetailsAsync(1);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteContractor_InvalidId_ReturnsFalse()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Contractor?)null);
            Assert.That(await _service.DeleteContractorAsync(99), Is.False);
        }
    }
}
