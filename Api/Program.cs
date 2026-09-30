using Api.Extensions;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Api.Middleware;
using System.Text;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddApplication();

            // Infrastructure
            builder.Services.AddInfrastructure(
                builder.Configuration);

            // JWT Authentication
            var jwtKey = builder.Configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException(
                    "JWT SecretKey is not configured.");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer =
                                builder.Configuration["Jwt:Issuer"],

                            ValidAudience =
                                builder.Configuration["Jwt:Audience"],

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(jwtKey))
                        };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.InitializeDatabase();

            app.UseMiddleware<ApiExceptionMiddleware>();
            app.UseStatusCodePages(async statusContext =>
            {
                var response = statusContext.HttpContext.Response;
                var problem = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Status = response.StatusCode,
                    Title = response.StatusCode switch
                    {
                        401 => "Authentication required",
                        403 => "Forbidden",
                        404 => "Resource not found",
                        _ => "Request failed"
                    },
                    Instance = statusContext.HttpContext.Request.Path
                };
                response.ContentType = "application/problem+json";
                await response.WriteAsJsonAsync(problem);
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        "/openapi/v1.json",
                        "Project Management API v1");
                });
            }

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
