using Application.Features.Tasks.Commands;
using Application.Features.Tasks.DTOs;
using Application.Features.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ISender _sender;

        public TasksController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> Create(
            CreateTaskCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TaskDto>> Update(
            int id,
            UpdateTaskCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            var result = await _sender.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _sender.Send(
                new DeleteTaskCommand(id),
                cancellationToken);

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll(
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetTasksQuery(),
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskDto>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetTaskByIdQuery(id),
                cancellationToken);

            return Ok(result);
        }
    }
}