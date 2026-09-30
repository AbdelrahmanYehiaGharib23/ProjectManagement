using Application.Common.Interfaces;
using Infrastructure.Persistence.DbInitializer;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ApplicationDbContext _dbContext;

        public RefreshTokenService(
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task StoreAsync(
            string userId,
            string refreshToken,
            DateTime expiresAt,
            CancellationToken cancellationToken = default)
        {
            var tokenHash = HashToken(refreshToken);

            var entity = new RefreshToken
            {
                UserId = userId,
                TokenHash = tokenHash,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.RefreshTokens.AddAsync(
                entity,
                cancellationToken);

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }

        public async Task<string?> GetUserIdAsync(
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            var tokenHash = HashToken(refreshToken);

            var entity = await _dbContext.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.TokenHash == tokenHash,
                    cancellationToken);

            if (entity is null)
                return null;

            if (entity.RevokedAt.HasValue)
                return null;

            if (entity.ExpiresAt <= DateTime.UtcNow)
                return null;

            return entity.UserId;
        }

        public async Task RevokeAsync(
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            var tokenHash = HashToken(refreshToken);

            var entity = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(
                    x => x.TokenHash == tokenHash,
                    cancellationToken);

            if (entity is null)
                return;

            entity.RevokedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(
                cancellationToken);
        }

        private static string HashToken(string token)
        {
            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(token));

            return Convert.ToBase64String(bytes);
        }
    }
}