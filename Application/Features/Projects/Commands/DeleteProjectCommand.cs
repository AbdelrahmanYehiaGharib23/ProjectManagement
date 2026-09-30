using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Features.Projects.Commands
{
    public record DeleteProjectCommand(int Id) : IRequest<Unit>;
}
