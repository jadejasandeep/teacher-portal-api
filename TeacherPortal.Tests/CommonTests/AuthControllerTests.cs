using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using TeacherPortal.Application.Authentication.Commands;
using TeacherPortal.WebAPI.Controllers;
using TeacherPortal.Application.Common.Exceptions;
using TeacherPortal.Application.Common.Models;

namespace TeacherPortal.Tests.CommonTests
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var command = new Login { Username = "testuser", Password = "password123" };
            var expectedResponse = new ApiResponse<string>(200, "Token generated successfully", "valid-jwt-token");

            _mediatorMock.Setup(m => m.Send(It.IsAny<Login>(), default))
                         .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GenerateToken(command) as ObjectResult;

            // Assert
            Assert.NotNull(result);

            var actualApiResponse = result.Value as ApiResponse<string>;
            Assert.NotNull(actualApiResponse);

            Assert.Equal(expectedResponse.StatusCode, actualApiResponse.StatusCode);
            Assert.Equal(expectedResponse.Message, actualApiResponse.Message);
            Assert.Equal(expectedResponse.Data, actualApiResponse.Data);
        }

        [Fact]
        public async Task GenerateToken_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var command = new Login { Username = "invalidUser", Password = "wrongPassword" };
            var expectedResponse = new ApiResponse<string>(401, "Invalid username or password.", null);

            _mediatorMock.Setup(m => m.Send(It.IsAny<Login>(), default))
                         .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GenerateToken(command) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
            Assert.Equal(expectedResponse, result.Value);
        }
    }
}
