namespace Application.Common.Interfaces
{
    public interface IRefreshTokenService
    {
        Task StoreAsync(
            string userId,
            string refreshToken,
            DateTime expiresAt,
            CancellationToken cancellationToken = default);

        Task<string?> GetUserIdAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);

        Task RevokeAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);
    }
}