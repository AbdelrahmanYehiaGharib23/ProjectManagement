using System;
using System.Collections.Generic;
using System.Text;
using Application.Features.Comment.DTOs;
using MediatR;

namespace Application.Features.Comment.Commands
{
    public record CreateCommentCommand(
        string Content,
        int TaskId
    ) : IRequest<CommentDto>;
}
