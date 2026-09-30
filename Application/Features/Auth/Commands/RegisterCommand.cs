using Application.Features.Auth.DTOs;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public record RegisterCommand(
       string Email,
       string Password) : IRequest<AuthResponse>;
}
