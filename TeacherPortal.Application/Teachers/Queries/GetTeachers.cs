using MediatR;
using Microsoft.EntityFrameworkCore;
using TeacherPortal.Application.Common.Extensions;
using TeacherPortal.Application.Common.Interfaces;
using TeacherPortal.Application.Common.Models;

namespace TeacherPortal.Application.Teachers.Queries
{
    public class GetTeachers : IRequest<ApiResponse<PaginatedList<TeacherDTO>>> {

        
        public int? PageNumber { get; set; }
        
        public int? PageSize { get; set; }

    }

    public class GetTeachersHandler : IRequestHandler<GetTeachers, ApiResponse<PaginatedList<TeacherDTO>>>
    {
        private readonly ITeacherDbContext _context;

        public GetTeachersHandler(ITeacherDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginatedList<TeacherDTO>>> Handle(GetTeachers request, CancellationToken cancellationToken)
        {
            var teachers = await _context.Teachers.Select(t => new TeacherDTO
            {
                Id = t.Id,
                Name = t.FirstName+" "+t.LastName,
                Email = t.Email,
                Username = t.Username
            }).AsNoTracking().PaginatedListAsync(request.PageNumber, request.PageSize,100,cancellationToken);// Currently marking default page size to 100 as frontend has no paginamtion handling currently
            return ApiResponse<PaginatedList<TeacherDTO>>.Success(teachers, "Teachers retrived successfully");
        }
    }
}
