using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using TeacherPortal.Application.Authentication.Commands;
using TeacherPortal.Application.Common.Models;
using TeacherPortal.Domain.Models;
using TeacherPortal.Infrastructure;

namespace TeacherPortal.Tests.CommandHandlerTests
{
    public class LoginHandlerTests
    {
        private readonly DbContextOptions<TeacherDBContext> _options;
        private readonly TeacherDBContext _context;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            _options = new DbContextOptionsBuilder<TeacherDBContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new TeacherDBContext(_options);
            _context.Database.EnsureCreated();

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(config => config["Jwt:Issuer"]).Returns("testIssuer");
            _configurationMock.Setup(config => config["Jwt:Audience"]).Returns("testAudience");
            _configurationMock.Setup(config => config["Jwt:SecretKey"]).Returns("LongtestSecretKeyLongtestSecretKeyLongtestSecretKeyLongtestSecretKey");

            _handler = new LoginHandler(_context, _configurationMock.Object);
        }

        [Fact]
        public async Task Handle_GivenValidCredentials_ShouldReturnSuccessResponse()
        {
            // Arrange
            var teacher = new Teacher
            {
                Username = "newtestuser",
                PasswordHash = "newhashedpassword",
                Email="sample@email.com",
                FirstName="test",
                LastName="user"
            };
            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            var request = new Login
            {
                Username = "newtestuser",
                Password = "newhashedpassword"
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().BeOfType<ApiResponse<string>>();
            response.Data.Should().NotBeNullOrEmpty();
            response.Message.Should().BeEquivalentTo("Token generated successful");
            
        }

        [Fact]
        public async Task Handle_GivenInvalidCredentials_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var request = new Login
            {
                Username = "wronguser",
                Password = "wrongpassword"
            };

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}