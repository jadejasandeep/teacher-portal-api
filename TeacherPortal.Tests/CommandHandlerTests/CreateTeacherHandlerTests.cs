using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TeacherPortal.Application.Common.Models;
using TeacherPortal.Application.Teachers.Commands;
using TeacherPortal.Infrastructure;
using TeacherPortal.Tests.DataGenerators;

namespace TeacherPortal.Tests.CommandHandlerTests
{
    public class CreateTeacherHandlerTests 
    {
        private readonly DbContextOptions<TeacherDBContext> _options;
        private readonly TeacherDBContext _context;

        public CreateTeacherHandlerTests()
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
            var request = new CreateTeacher
            {
                Username = "testusersample",
                Email = "testsample@example.com",
                FirstName = "Testsample123",
                LastName = "Usersample123",
                Password = "hashedpassword",
                
            };
            var _handler = new CreateTeacherHandler(_context);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().BeOfType<ApiResponse<int>>();
            response.Data.Should().BeGreaterThan(0);
            response.Message.Should().BeEquivalentTo("Teacher created successfully");
            _context.Teachers.FirstOrDefault(x => x.FirstName == request.FirstName).Should().NotBeNull();
            
        }
    }
}
