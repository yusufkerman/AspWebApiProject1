using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Repository.EFCore;
using System.Text;
using Services.Contracts;
using Services;

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
            service.AddScoped<IAuthenticationService, AuthenticationManager>();
            service.AddScoped<IServiceManager,ServiceManager>();
        }
        
    
    }
}
