using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Text.Json;
using TeacherPortal.Application.Common.Exceptions;
using TeacherPortal.Application.Common.Extensions;
using TeacherPortal.Application.Common.Models;


namespace TeacherPortal.Tests.CommonTests
{
    public class GlobalExceptionHandlerTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly GlobalExceptionHandler _middleware;

        public GlobalExceptionHandlerTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _middleware = new GlobalExceptionHandler(_nextMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_Should_Return_ValidationErrorResponse_On_CustomValidationException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var validationErrors = new List<ValidationError>
        {
            new ValidationError() { PropertyName = "PropertyName", ErrorCode = "ErrorCode",ErrorMessage="ErrorMessage" }
        };
            var customValidationException = new CustomValidationException(validationErrors);

            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(customValidationException);

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Act
            await _middleware.InvokeAsync(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<ValidationErrorResponse>(responseBody);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            response.Should().NotBeNull();
            response.message.Should().Be("Validation failed.");
            response.errors.Should().HaveCount(1);
            response.errors.First().PropertyName.Should().Be("PropertyName");
            response.errors.First().ErrorMessage.Should().Be("ErrorMessage");
            response.errors.First().ErrorCode.Should().Be("ErrorCode");
        }

        [Fact]
        public async Task InvokeAsync_Should_Return_ErrorResponse_On_UnauthorizedAccessException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var unauthorizedAccessException = new UnauthorizedAccessException("Invalid username or password.");

            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(unauthorizedAccessException);

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Act
            await _middleware.InvokeAsync(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            response.Should().NotBeNull();
            response.message.Should().Be("Invalid username or password.");
            response.details.Should().Be("Invalid username or password.");
        }

        [Fact]
        public async Task InvokeAsync_Should_Return_ErrorResponse_On_GenericException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var genericException = new Exception("An unexpected error occurred.");

            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(genericException);

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Act
            await _middleware.InvokeAsync(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            response.Should().NotBeNull();
            response.message.Should().Be("An unexpected error occurred.");
            response.details.Should().Be("An unexpected error occurred.");
        }
    }
}
