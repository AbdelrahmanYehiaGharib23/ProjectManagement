using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Application.Features.Tasks.Commands
{
    public record DeleteTaskCommand  (int Id) : IRequest<Unit>;

}
