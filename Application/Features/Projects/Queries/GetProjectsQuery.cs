using System;
using System.Collections.Generic;
using System.Text;
using Application.Features.Projects.DTOs;
using MediatR;

namespace Application.Features.Projects.Queries
{
    public record GetProjectsQuery : IRequest<IEnumerable<ProjectDto>>;
}
