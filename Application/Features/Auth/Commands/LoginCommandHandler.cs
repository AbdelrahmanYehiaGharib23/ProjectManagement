using Application.Common.Interfaces;
using Application.Common.Interfaces.Application.Common.Interfaces;
using Application.Features.Auth.DTOs;
using MediatR;
using System.Security.Authentication;

namespace Application.Features.Auth.Commands
{
    public class LoginCommandHandler
        : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginCommandHandler(
            IIdentityService identityService,
            IJwtTokenService jwtTokenService,
            IRefreshTokenService refreshTokenService)
        {
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<AuthResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var result =
                await _identityService.ValidateCredentialsAsync(
                    request.Email,
                    request.Password,
                    cancellationToken);

            if (!result.Succeeded)
            {
                throw new AuthenticationException(
                    "Invalid email or password.");
            }

            var roles =
                await _identityService.GetRolesAsync(
                    result.UserId,
                    cancellationToken);

            var accessToken =
                _jwtTokenService.GenerateAccessToken(
                    result.UserId,
                    request.Email,
                    roles);

            var refreshToken =
                _jwtTokenService.GenerateRefreshToken();

            var accessTokenExpiresAt =
                _jwtTokenService.GetAccessTokenExpiration();

            var refreshTokenExpiresAt =
                _jwtTokenService.GetRefreshTokenExpiration();

            await _refreshTokenService.StoreAsync(
                result.UserId,
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
