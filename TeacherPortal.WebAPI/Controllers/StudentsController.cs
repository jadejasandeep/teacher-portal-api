using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeacherPortal.Application.Common.Models;
using TeacherPortal.Application.Students.Queries;

namespace TeacherPortal.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{teacherId}")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedList<StudentDTO>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<IActionResult> GetStudents(int teacherId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var response = await _mediator.Send(new GetStudentsByTeacherQuery() { TeacherId=teacherId, PageNumber = pageNumber, PageSize = pageSize });
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), 200)]
    [ProducesResponseType(typeof(ApiResponse<string>), 400)]
    public async Task<IActionResult> CreateStudent([FromBody] Application.Students.Commands.CreateStudent command)
    {
        var response = await _mediator.Send(command);
        return StatusCode(response.StatusCode, response);
    }
}
