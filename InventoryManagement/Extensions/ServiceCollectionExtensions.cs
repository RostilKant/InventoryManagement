using System.Linq;
using Entities;
using Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAndMigrateTenantDatabases(this IServiceCollection services)
        {
            var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
            
            var defaultConnectionString = options.Defaults?.ConnectionString;
            var defaultDbProvider = options.Defaults?.DbProvider;
            
            switch (defaultDbProvider?.ToLower())
            {
                case "mssql":
                    services.AddDbContext<ApplicationContext>(m => m.UseSqlServer(e => e.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
                    break;
                case "postgres":
                    services.AddDbContext<ApplicationContext>(m => m.UseNpgsql(e => e.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
                    break;
                default:
                    throw new System.Exception("Invalid database provider!");
            }
            
            foreach (var tenant in options.Tenants)
            {
                var connectionString = string.IsNullOrEmpty(tenant.ConnectionString)
                    ? defaultConnectionString
                    : tenant.ConnectionString;               
                
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                dbContext.Database.SetConnectionString(connectionString);
                
                if (dbContext.Database.GetMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }
    }
}