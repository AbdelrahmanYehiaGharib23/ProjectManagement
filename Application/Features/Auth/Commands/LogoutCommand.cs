using MediatR;

namespace Application.Features.Auth.Commands
{
    public record LogoutCommand(
        string RefreshToken) : IRequest<Unit>;
}