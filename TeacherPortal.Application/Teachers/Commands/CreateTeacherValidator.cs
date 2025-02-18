using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeacherPortal.Application.Common.Interfaces;

namespace TeacherPortal.Application.Teachers.Commands
{
    public class CreateTeacherValidator : AbstractValidator<CreateTeacher>
    {
        private readonly ITeacherDbContext _context;
        public CreateTeacherValidator(ITeacherDbContext context)
        {
            _context = context;
            RuleFor(x => x.Username)
            .NotEmpty()
            .MustAsync(UsernameNotExists).WithMessage("Username already exists.");
            RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(EmailNotExists).WithMessage("Email already exists.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").MinimumLength(6);
        }
        private async Task<bool> UsernameNotExists(string username, CancellationToken cancellationToken)
        {
            return !await _context.Teachers.AnyAsync(t => t.Username == username, cancellationToken);
        }

        private async Task<bool> EmailNotExists(string email, CancellationToken cancellationToken)
        {
            return !await _context.Teachers.AnyAsync(t => t.Email == email, cancellationToken);
        }
    }
}
