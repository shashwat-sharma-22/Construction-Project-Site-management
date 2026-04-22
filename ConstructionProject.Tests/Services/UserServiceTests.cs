using NUnit.Framework;
using Moq;
using ConstructionProject.Services;
using ConstructionProject.Interfaces;
using ConstructionProject.Models;
using ConstructionProject.DTOs;

namespace ConstructionProject.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockRepo = null!;
        private UserService _service = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IUserRepository>();
            _service = new UserService(_mockRepo.Object);
        }

        [Test]
        public async Task GetAllUsers_ReturnsList()
        {
            _mockRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<AppUser> { new() { UserId = 1, Name = "Admin", Email = "a@b.com", Role = UserRole.Admin } });

            var result = await _service.GetAllUsers();
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task RegisterUser_DuplicateEmail_ReturnsNull()
        {
            _mockRepo.Setup(r => r.EmailExistsAsync("a@b.com")).ReturnsAsync(true);

            var result = await _service.RegisterUser(new RegisterUserDto
            {
                Name = "Test",
                Email = "a@b.com",
                Password = "123456",
                Role = UserRole.Admin
            });

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteUser_InvalidId_ReturnsFalse()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((AppUser?)null);
            Assert.That(await _service.DeleteUser(99), Is.False);
        }
    }
}
