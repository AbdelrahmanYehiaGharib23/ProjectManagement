using System;
using System.Collections.Generic;
using System.Text;
using Application.Features.Auth.DTOs;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<AuthResponse>;
}
