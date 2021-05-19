using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UniversitySystem.DataAccess.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UniversitySystemDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(UniversitySystemDbContext).Assembly.FullName)));

            services.AddScoped<IUniversitySystemDbContext>(provider => provider.GetService<UniversitySystemDbContext>());

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
