using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                _dbContext.Database.Migrate();
            }

            SeedRoles();
        }

        private void SeedRoles()
        {
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role)
                    .GetAwaiter()
                    .GetResult())
                {
                    _roleManager
                        .CreateAsync(new IdentityRole(role))
                        .GetAwaiter()
                        .GetResult();
                }
            }
        }
    }
}