namespace Application.Common.Interfaces
{
    namespace Application.Common.Interfaces
    {
        public interface IIdentityService
        {
            Task<bool> UserExistsAsync(
                string email,
                CancellationToken cancellationToken = default);

            Task<string> CreateUserAsync(
                string email,
                string password,
                CancellationToken cancellationToken = default);

            Task AddToRoleAsync(
                string userId,
                string role,
                CancellationToken cancellationToken = default);

            Task<IReadOnlyCollection<string>> GetRolesAsync(
                string userId,
                CancellationToken cancellationToken = default);

            Task<(bool Succeeded, string UserId)> ValidateCredentialsAsync(
                string email,
                string password,
                CancellationToken cancellationToken = default);

            Task<string> GetEmailAsync(string userId,CancellationToken cancellationToken = default);
        }
    }
}