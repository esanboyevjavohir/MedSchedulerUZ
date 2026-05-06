using MedSchedulerUZ.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedSchedulerUZ.DataAccess
{
    public static class DataAccessDependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            return services;
        }

        private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseConfig = configuration.GetSection("Database").Get<DatabaseConfiguration>();

            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(databaseConfig.ConnectionString,
                    npgsqlOptions => npgsqlOptions.MigrationsAssembly(
                        typeof(DatabaseContext).Assembly.FullName)));
        }
    }
    public class DatabaseConfiguration
    {
        public bool UseInMemoryDatabase { get; set; }
        public string ConnectionString { get; set; }
    }
}
