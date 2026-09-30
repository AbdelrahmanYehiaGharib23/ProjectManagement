using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public class LogoutCommandHandler
        : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public LogoutCommandHandler(
            IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Unit> Handle(
            LogoutCommand request,
            CancellationToken cancellationToken)
        {
            await _refreshTokenService.RevokeAsync(
                request.RefreshToken,
                cancellationToken);

            return Unit.Value;
        }
    }
}