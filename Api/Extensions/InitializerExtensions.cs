using Application.Common.Interfaces;

namespace Api.Extensions
{
    public static class InitializerExtensions
    {
        public static void InitializeDatabase(this IApplicationBuilder app)
        {
            using var Scope = app.ApplicationServices.CreateScope();
            var Services = Scope.ServiceProvider;
            var dbInitializer = Services.GetRequiredService<IDbInitializer>(); //Ask Explicitly 
            dbInitializer.Initialize();
        }
    }
}
