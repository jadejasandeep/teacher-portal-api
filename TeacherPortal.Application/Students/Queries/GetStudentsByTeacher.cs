using MediatR;
using Microsoft.EntityFrameworkCore;
using TeacherPortal.Application.Common.Extensions;
using TeacherPortal.Application.Common.Interfaces;
using TeacherPortal.Application.Common.Models;

namespace TeacherPortal.Application.Students.Queries
{
    public class GetStudentsByTeacherQuery : IRequest<ApiResponse<PaginatedList<StudentDTO>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int TeacherId { get; set; }
    }

    public class GetStudentsByTeacherHandler : IRequestHandler<GetStudentsByTeacherQuery, ApiResponse<PaginatedList<StudentDTO>>>
    {
        private readonly ITeacherDbContext _context;

        public GetStudentsByTeacherHandler(ITeacherDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginatedList<StudentDTO>>> Handle(GetStudentsByTeacherQuery request, CancellationToken cancellationToken)
        {
            var students = await _context.Students
                .Where(s => s.TeacherId == request.TeacherId)
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email
                }).AsNoTracking().PaginatedListAsync(request.PageNumber, request.PageSize, 10, cancellationToken);

            return ApiResponse<PaginatedList<StudentDTO>>.Success(students, "Students fetched successfully");
        }
    }
}
