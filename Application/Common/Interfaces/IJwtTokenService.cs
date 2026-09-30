namespace Application.Common.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(
            string userId,
            string email,
            IEnumerable<string> roles);

        DateTime GetAccessTokenExpiration();

        string GenerateRefreshToken();

        DateTime GetRefreshTokenExpiration();
    }
}