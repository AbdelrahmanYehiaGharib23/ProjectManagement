using Application.Common.Interfaces.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> UserExistsAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user is not null;
        }

        public async Task<string> CreateUserAsync(
            string email,
            string password,
            CancellationToken cancellationToken = default)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(
                user,
                password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }

            return user.Id;
        }

        public async Task AddToRoleAsync(
            string userId,
            string role,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException(
                    $"User with id {userId} was not found.");
            }

            var result = await _userManager.AddToRoleAsync(
                user,
                role);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(errors);
            }
        }

        public async Task<IReadOnlyCollection<string>> GetRolesAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException(
                    $"User with id {userId} was not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }
        public async Task<(bool Succeeded, string UserId)> ValidateCredentialsAsync( string email,string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return (false, string.Empty);

            var validPassword = await _userManager.CheckPasswordAsync(
                user,
                password);

            if (!validPassword)
                return (false, string.Empty);

            return (true, user.Id);
        }
        public async Task<string> GetEmailAsync(
    string userId,
    CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException(
                    $"User with id {userId} was not found.");
            }

            return user.Email
                ?? throw new InvalidOperationException(
                    "User email is not configured.");
        }
    }
}