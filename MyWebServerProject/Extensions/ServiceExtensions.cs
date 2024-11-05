using Microsoft.EntityFrameworkCore;
using Repository.EFCore;

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

    }
}
