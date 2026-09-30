using MediatR;

namespace Application.Features.Comment.Commands
{
    public record DeleteCommentCommand(int Id) : IRequest<Unit>;
}
