using MediatR;
using TeacherPortal.Application.Common.Interfaces;
using TeacherPortal.Application.Common.Models;
using TeacherPortal.Domain.Models;

namespace TeacherPortal.Application.Students.Commands
{
    public class CreateStudent : IRequest<ApiResponse<int>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int TeacherId { get; set; }
    }
    public class CreateStudentHandler : IRequestHandler<CreateStudent, ApiResponse<int>>
    {
        private readonly ITeacherDbContext _context;

        public CreateStudentHandler(ITeacherDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<int>> Handle(CreateStudent request, CancellationToken cancellationToken)
        {
            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                TeacherId = request.TeacherId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponse<int>.Success(student.Id, "Student created successfully");
        }
    }
}
