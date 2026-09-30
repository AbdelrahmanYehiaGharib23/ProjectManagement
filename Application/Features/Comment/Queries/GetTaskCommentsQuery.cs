using System;
using System.Collections.Generic;
using System.Text;
using Application.Features.Comment.DTOs;
using MediatR;

namespace Application.Features.Comment.Queries
{
    public record GetTaskCommentsQuery(
      int TaskId
  ) : IRequest<IEnumerable<CommentDto>>;
}
