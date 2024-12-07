using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Repository.EFCore;
using System.Text;
using Services.Contracts;
using Services;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Presentation.ActionFilters;

namespace MyWebServerProject.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureMySqlContext(this IServiceCollection service,
            IConfiguration configuration)
        {
            service.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseMySQL(
                    configuration.GetConnectionString("MyDatabase"),
                    mySqlOptions =>
                    {
                        mySqlOptions.MigrationsAssembly("MyWebServerProject");
                    }
                );
            });
        }
        public static void ConfigureJWTAuthentication(this IServiceCollection service,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = 
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

        }
        public static void RegisterIOCForManagers(this IServiceCollection service)
        {
            service.AddScoped<IApiKeyValidationService,ApiKeyValidationManager>();
            service.AddScoped<IUserService, UserManipulationManager>();
            service.AddScoped<IAuthenticationService, AuthenticationManager>();
            service.AddScoped<IServiceManager,ServiceManager>();
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 6;

                opts.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
        public static void RegisterIOCForActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ApiKeyFilter>();
        }
    }
}
