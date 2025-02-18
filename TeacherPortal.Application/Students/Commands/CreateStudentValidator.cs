using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TeacherPortal.Application.Common.Interfaces;

namespace TeacherPortal.Application.Students.Commands
{
    public class CreateStudentValidator : AbstractValidator<CreateStudent>
    {
        private readonly ITeacherDbContext _context;
        public CreateStudentValidator(ITeacherDbContext context)
        {
            _context = context;
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(EmailNotExists).WithMessage("Email already exists.");
        }
        private async Task<bool> EmailNotExists(string email, CancellationToken cancellationToken)
        {
            return !await _context.Students.AnyAsync(s => s.Email == email, cancellationToken);
        }
    }
}
