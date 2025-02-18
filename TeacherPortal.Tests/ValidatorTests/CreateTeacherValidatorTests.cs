using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using TeacherPortal.Application.Teachers.Commands;
using TeacherPortal.Domain.Models;
using TeacherPortal.Infrastructure;

namespace TeacherPortal.Tests.ValidatorTests
{
    public class CreateTeacherValidatorTests
    {
        private readonly CreateTeacherValidator _validator;
        private readonly TeacherDBContext _context;

        public CreateTeacherValidatorTests()
        {
            var options = new DbContextOptionsBuilder<TeacherDBContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new TeacherDBContext(options);
            _context.Database.EnsureCreated();

            _validator = new CreateTeacherValidator(_context);
        }

        [Fact]
        public async Task Should_Have_Error_When_Username_Is_Empty()
        {
            var model = new CreateTeacher { Username = string.Empty };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(teacher => teacher.Username);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Empty()
        {
            var model = new CreateTeacher { Email = string.Empty };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(teacher => teacher.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new CreateTeacher { Email = "invalid-email" };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(teacher => teacher.Email);
        }

        [Fact]
        public async Task Should_Have_Error_When_Email_Already_Exists()
        {
            var existingTeacher = new Teacher
            {
                Username = "existinguser",
                Email = "existing@example.com",
                FirstName = "John",
                LastName = "Doe",
                PasswordHash = "hashedpassword"
            };

            _context.Teachers.Add(existingTeacher);
            _context.SaveChanges();

            var model = new CreateTeacher { Email = "existing@example.com" };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(teacher => teacher.Email).WithErrorMessage("Email already exists.");
        }

        [Fact]
        public async Task Should_Have_Error_When_Username_Already_Exists()
        {
            var existingTeacher = new Teacher
            {
                Username = "existinguser",
                Email = "unique@example.com",
                FirstName = "John",
                LastName = "Doe",
                PasswordHash = "hashedpassword"
            };

            _context.Teachers.Add(existingTeacher);
            _context.SaveChanges();

            var model = new CreateTeacher { Username = "existinguser" };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(teacher => teacher.Username).WithErrorMessage("Username already exists.");
        }

        [Fact]
        public async Task Should_Have_Error_When_FirstName_Is_Empty()
        {
            var model = new CreateTeacher { FirstName = string.Empty };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(teacher => teacher.FirstName).WithErrorMessage("First name is required.");
        }

        [Fact]
        public async Task Should_Have_Error_When_LastName_Is_Empty()
        {
            var model = new CreateTeacher { LastName = string.Empty };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(teacher => teacher.LastName).WithErrorMessage("Last name is required.");
        }

        [Fact]
        public async Task Should_Have_Error_When_PasswordHash_Is_Empty()
        {
            var model = new CreateTeacher { Password = string.Empty };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(teacher => teacher.Password).WithErrorMessage("Password is required.");
        }

        [Fact]
        public async Task Should_Have_Error_When_PasswordHash_Is_Too_Short()
        {
            var model = new CreateTeacher { Password = "short" };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(teacher => teacher.Password).WithErrorMessage("The length of 'Password' must be at least 6 characters. You entered 5 characters.");
        }

        [Fact]
        public async Task Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var model = new CreateTeacher
            {
                Username = "newuser",
                Email = "newuser@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "validpassword"
            };
            var result = await _validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(teacher => teacher.Username);
            result.ShouldNotHaveValidationErrorFor(teacher => teacher.Email);
            result.ShouldNotHaveValidationErrorFor(teacher => teacher.FirstName);
            result.ShouldNotHaveValidationErrorFor(teacher => teacher.LastName);
            result.ShouldNotHaveValidationErrorFor(teacher => teacher.Password);
        }
    }
}
