using Application.Features.Comment.Commands;
using Application.Features.Comment.DTOs;
using Application.Features.Comment.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> Create(
            CreateCommentCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("task/{taskId:int}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetTaskComments(
            int taskId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetTaskCommentsQuery(taskId),
                cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new DeleteCommentCommand(id),
                cancellationToken);

            return NoContent();
        }
    }
}
