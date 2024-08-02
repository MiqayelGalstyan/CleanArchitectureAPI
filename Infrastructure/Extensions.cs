using System.Text;
using Domain.Mappers.Profile;
using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Infrastructure.Context;
using LayeredAPI.Domain.Mappers;
using LayeredAPI.Infrastructure.Repositories;
using LayeredAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace LayeredAPI.Infrastructure.Extensions;

public static class Extensions
{
    public static void ConfigureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureCors();
        services.AddOptions();
        services.ConfigureDbContext(configuration);
        services.AddMappers();
        services.ConfigureManagerServices();
        services.AddRepositories();
        services.AddJwtAuthentication(configuration);
    }


    private static void ConfigureManagerServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IProfileService, ProfileService>();
    }

    private static void AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
    }


    private static void AddMappers(this IServiceCollection services)
    {
        services.AddSingleton<UserMapper>();
        services.AddSingleton<RoleMapper>();
        services.AddSingleton<ProfileMapper>();
    }
    

    private static void ConfigureDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("LayeredAPIDatabase"));
        });
    }

    private static void AddJwtAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:JWTKey").Value)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.Response.StatusCode = 401;
                    context.Response.WriteAsJsonAsync(
                        "Unauthorized. You can get access token from this endpoint /Api/Login (POST)");
                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()  
                        .AllowAnyHeader(); 
                });
        });
    }
}