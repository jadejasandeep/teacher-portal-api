using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TeacherPortal.Application.Common.Models;
using TeacherPortal.Application.Students.Commands;
using TeacherPortal.Infrastructure;

namespace TeacherPortal.Tests.CommandHandlerTests
{
    public class CreateStudentHandlerTests
    {
        private readonly DbContextOptions<TeacherDBContext> _options;
        private readonly TeacherDBContext _context;

        public CreateStudentHandlerTests()
        {
            _options = new DbContextOptionsBuilder<TeacherDBContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new TeacherDBContext(_options);
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ShouldReturnSuccessResponse()
        {
            // Arrange
            var request = new CreateStudent
            {
                FirstName = "John",
                LastName = "Martin",
                Email = "john.martin@example.com",
                TeacherId = 1
            };
            var handler = new CreateStudentHandler(_context);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().BeOfType<ApiResponse<int>>();
            response.Data.Should().BeGreaterThan(0);
            response.Message.Should().BeEquivalentTo("Student created successfully");
            _context.Students.FirstOrDefault(x => x.FirstName == request.FirstName).Should().NotBeNull();
        }
    }
}
