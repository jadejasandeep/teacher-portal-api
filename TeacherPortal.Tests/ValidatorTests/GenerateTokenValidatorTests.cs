using FluentValidation.TestHelper;
using TeacherPortal.Application.Authentication.Commands;

namespace TeacherPortal.Tests.ValidatorTests
{
    public class GenerateTokenValidatorTests
    {
        private readonly LoginValidator _validator;

        public GenerateTokenValidatorTests()
        {
            _validator = new LoginValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Username_Is_Empty()
        {
            var model = new Login { Username = string.Empty };
            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(token => token.Username).WithErrorMessage("Username is required.");
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            var model = new Login { Password = string.Empty };
            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(token => token.Password).WithErrorMessage("Password is required.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var model = new Login
            {
                Username = "testuser",
                Password = "password123"
            };
            var result = _validator.TestValidate(model);

            result.ShouldNotHaveValidationErrorFor(token => token.Username);
            result.ShouldNotHaveValidationErrorFor(token => token.Password);
        }
    }
}
