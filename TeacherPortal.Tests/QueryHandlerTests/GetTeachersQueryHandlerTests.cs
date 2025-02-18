using Azure.Core;
using Azure;
using Microsoft.EntityFrameworkCore;
using Moq;
using TeacherPortal.Application.Common.Interfaces;
using TeacherPortal.Application.Common.Models;
using TeacherPortal.Application.Teachers.Queries;
using TeacherPortal.Domain.Models;
using TeacherPortal.Infrastructure;
using TeacherPortal.Tests.DataGenerators;
using FluentAssertions;

namespace TeacherPortal.Tests.QueryhandlerTests
{
    public class GetTeachersQueryHandlerTests
    {
        private readonly DbContextOptions<TeacherDBContext> _options;
        private readonly TeacherDBContext _context;

        public GetTeachersQueryHandlerTests()
        {
            _options = new DbContextOptionsBuilder<TeacherDBContext>()
                .UseInMemoryDatabase("TestDatabase")
                    .Options;

            _context = new TeacherDBContext(_options);
            _context.Database.EnsureCreated(); // Ensure the database is created

            var teachers = TeacherGenerator.GenerateTeachers(5);
            _context.Teachers.AddRange(teachers);
            _context.SaveChanges();
        }

        [Fact]
        public async Task Handle_ReturnsTeacherList()
        {
            // Arrange
            var _handler = new GetTeachersHandler(_context);

            var query = new GetTeachers();

            // Act
            var response = await _handler.Handle(query, CancellationToken.None);

            // Assert
            response.Should().BeOfType<ApiResponse<PaginatedList<TeacherDTO>>>();

            response.Message.Should().BeEquivalentTo("Teachers retrived successfully");
            response.StatusCode.Should().Be(200);
        }
    }
}