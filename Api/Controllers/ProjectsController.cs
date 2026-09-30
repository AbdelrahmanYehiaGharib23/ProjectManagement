using Application.Features.Projects.Commands;
using Application.Features.Projects.DTOs;
using Application.Features.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(
            CreateProjectCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetProjectsQuery(),
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectDto>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProjectDto>> Update(
            int id,
            UpdateProjectCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest("Route id does not match command id.");
            }

            var result = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new DeleteProjectCommand(id),
                cancellationToken);

            return NoContent();
        }
    }
}
