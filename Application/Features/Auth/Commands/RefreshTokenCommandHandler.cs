using Application.Common.Interfaces;
using Application.Common.Interfaces.Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using MediatR;
using System.Security.Authentication;

namespace Application.Features.Auth.Commands
{
    public class RefreshTokenCommandHandler
        : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;

        public RefreshTokenCommandHandler(
            IRefreshTokenService refreshTokenService,
            IIdentityService identityService,
            IJwtTokenService jwtTokenService)
        {
            _refreshTokenService = refreshTokenService;
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var userId =
                await _refreshTokenService.GetUserIdAsync(
                    request.RefreshToken,
                    cancellationToken);

            if (userId is null)
            {
                throw new AuthenticationException(
                    "Invalid or expired refresh token.");
            }

            var roles =
                await _identityService.GetRolesAsync(
                    userId,
                    cancellationToken);

            var accessToken =
                _jwtTokenService.GenerateAccessToken(
                    userId,
                    await GetEmailAsync(userId, cancellationToken),
                    roles);

            var newRefreshToken =
                _jwtTokenService.GenerateRefreshToken();

            var accessTokenExpiresAt =
                _jwtTokenService.GetAccessTokenExpiration();

            var refreshTokenExpiresAt =
                _jwtTokenService.GetRefreshTokenExpiration();

            // Rotation: invalidate the old refresh token.
            await _refreshTokenService.RevokeAsync(
                request.RefreshToken,
                cancellationToken);

            await _refreshTokenService.StoreAsync(
                userId,
                newRefreshToken,
                refreshTokenExpiresAt,
                cancellationToken);

            return new AuthResponse(
                accessToken,
                newRefreshToken,
                accessTokenExpiresAt,
                refreshTokenExpiresAt);
        }

        private async Task<string> GetEmailAsync(
            string userId,
            CancellationToken cancellationToken)
        {
            return await _identityService.GetEmailAsync(
                userId,
                cancellationToken);
        }
    }
}
