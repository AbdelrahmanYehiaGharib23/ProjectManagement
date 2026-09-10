using Application.Features.Projects.Commands;
using Application.Features.Projects.DTOs;
using Application.Features.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ISender _sender;

        public ProjectsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(
            CreateProjectCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll(
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetProjectsQuery(),
                cancellationToken);

            return Ok(result);
        }
    }
}