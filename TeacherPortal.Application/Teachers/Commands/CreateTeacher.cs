using MediatR;
using TeacherPortal.Application.Common.Interfaces;
using TeacherPortal.Application.Common.Models;
using TeacherPortal.Domain.Models;

namespace TeacherPortal.Application.Teachers.Commands
{
    public class CreateTeacher : IRequest<ApiResponse<int>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }

    public class CreateTeacherHandler : IRequestHandler<CreateTeacher, ApiResponse<int>>
    {
        private readonly ITeacherDbContext _context;

        public CreateTeacherHandler(ITeacherDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<int>> Handle(CreateTeacher request, CancellationToken cancellationToken)
        {
            var teacher = new Teacher
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<int>.Success(teacher.Id, "Teacher created successfully");
        }
    }
}
