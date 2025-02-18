using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeacherPortal.Application.Common.Models;
using TeacherPortal.Application.Teachers.Queries;


namespace TeacherPortal.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeachersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<TeacherDTO>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<IActionResult> GetTeachers([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var response = await _mediator.Send(new GetTeachers()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });
            return StatusCode(response.StatusCode, response);
        }
        [AllowAnonymous]
        [HttpPost("signup")]
        [ProducesResponseType(typeof(ApiResponse<int>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<IActionResult> CreateTeacher([FromBody] Application.Teachers.Commands.CreateTeacher command)
        {
            var response = await _mediator.Send(command);
            return StatusCode(response.StatusCode, response);
        }
    }
}
