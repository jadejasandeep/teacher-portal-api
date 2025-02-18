using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using TeacherPortal.Application.Students.Commands;
using TeacherPortal.Domain.Models;
using TeacherPortal.Infrastructure;

namespace TeacherPortal.Tests.ValidatorTests
{
    public class CreateStudentValidatorTests
    {
        private readonly CreateStudentValidator _validator;
        private readonly TeacherDBContext _context;

        public CreateStudentValidatorTests()
        {
            var options = new DbContextOptionsBuilder<TeacherDBContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new TeacherDBContext(options);
            _context.Database.EnsureCreated();

            _validator = new CreateStudentValidator(_context);
        }

        [Fact]
        public async Task Should_Have_Error_When_FirstName_Is_Empty()
        {
            var model = new CreateStudent { FirstName = string.Empty };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(student => student.FirstName);
        }

        [Fact]
        public async Task Should_Have_Error_When_LastName_Is_Empty()
        {
            var model = new CreateStudent { LastName = string.Empty };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(student => student.LastName);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Empty()
        {
            var model = new CreateStudent { Email = string.Empty };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(student => student.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new CreateStudent { Email = "invalid-email" };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(student => student.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Already_Exists()
        {
            var existingStudent = new Student
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "existing@example.com"
            };

            _context.Students.Add(existingStudent);
            _context.SaveChanges();

            var model = new CreateStudent { Email = "existing@example.com" };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(student => student.Email).WithErrorMessage("Email already exists.");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var model = new CreateStudent
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(student => student.FirstName);
            result.ShouldNotHaveValidationErrorFor(student => student.LastName);
            result.ShouldNotHaveValidationErrorFor(student => student.Email);
        }
    }
}
