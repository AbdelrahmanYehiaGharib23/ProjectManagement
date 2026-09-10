using System;
using System.Collections.Generic;
using System.Text;
using Application.Features.Tasks.DTOs;
using MediatR;

namespace Application.Features.Tasks.Commands
{
    public record UpdateTaskCommand(
     int Id,
     string Title,
     string Description,
     int ProjectId,
     bool IsCompleted
 ) : IRequest<TaskDto>;
}
