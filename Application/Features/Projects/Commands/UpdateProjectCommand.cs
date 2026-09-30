using System;
using System.Collections.Generic;
using System.Text;
using Application.Features.Projects.DTOs;
using MediatR;

namespace Application.Features.Projects.Commands
{
    public record UpdateProjectCommand(
        int Id,
        string Name,
        string Description
    ) : IRequest<ProjectDto>;
}
