using Application.Common.Interfaces;
using Application.Common.Interfaces.Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public RegisterCommandHandler(
            IIdentityService identityService,
            IJwtTokenService jwtTokenService,
            IRefreshTokenService refreshTokenService)
        {
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<AuthResponse> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser =
                await _identityService.UserExistsAsync(
                    request.Email,
                    cancellationToken);

            if (existingUser)
            {
                throw new InvalidOperationException(
                    "A user with this email already exists.");
            }

            var userId =
                await _identityService.CreateUserAsync(
                    request.Email,
                    request.Password,
                    cancellationToken);

            await _identityService.AddToRoleAsync(
                userId,
                "User",
                cancellationToken);

            var roles =
                await _identityService.GetRolesAsync(
                    userId,
                    cancellationToken);

            var accessToken =
                _jwtTokenService.GenerateAccessToken(
                    userId,
                    request.Email,
                    roles);

            var refreshToken =
                _jwtTokenService.GenerateRefreshToken();

            var accessTokenExpiresAt =
                _jwtTokenService.GetAccessTokenExpiration();

            var refreshTokenExpiresAt =
                _jwtTokenService.GetRefreshTokenExpiration();

            await _refreshTokenService.StoreAsync(
                userId,
                refreshToken,
                refreshTokenExpiresAt,
                cancellationToken);

            return new AuthResponse(
                accessToken,
                refreshToken,
                accessTokenExpiresAt,
                refreshTokenExpiresAt);
        }
    }
}