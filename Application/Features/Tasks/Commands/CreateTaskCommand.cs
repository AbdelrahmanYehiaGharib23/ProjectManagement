using System;
using System.Collections.Generic;
using System.Text;
using Application.Features.Tasks.DTOs;
using MediatR;

namespace Application.Features.Tasks.Commands
{
    public record CreateTaskCommand(string Title,string Description, int ProjectId) : IRequest<TaskDto>;
}
