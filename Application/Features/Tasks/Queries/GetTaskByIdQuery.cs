using System;
using System.Collections.Generic;
using System.Text;
using Application.Features.Tasks.DTOs;
using MediatR;

namespace Application.Features.Tasks.Queries
{
    public record GetTaskByIdQuery(int Id) : IRequest<TaskDto>;
}
