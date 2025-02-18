using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TeacherPortal.Application.Common.Middleware;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TeacherPortal.Tests.CommonTests
{
    public class AuthenticationMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly AuthenticationMiddleware _middleware;
        private readonly DefaultHttpContext _httpContext;

        public AuthenticationMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _middleware = new AuthenticationMiddleware(_nextMock.Object);
            _httpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task InvokeAsync_ShouldCallNext_WhenAllowAnonymous()
        {
            // Arrange
            var endpoint = new Endpoint(context => Task.CompletedTask, new EndpointMetadataCollection(new IAllowAnonymous[] { new AllowAnonymousAttribute() }), "AllowAnonymous");
            _httpContext.SetEndpoint(endpoint);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturn401_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.Equal(StatusCodes.Status401Unauthorized, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ShouldCallNext_WhenUserIsAuthenticated()
        {
            // Arrange
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser") }, "TestAuthType");
            _httpContext.User = new ClaimsPrincipal(identity);
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }
    }
}
