using Application.Features.Auth.DTOs;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public record RefreshTokenCommand(
        string RefreshToken) : IRequest<AuthResponse>;
}