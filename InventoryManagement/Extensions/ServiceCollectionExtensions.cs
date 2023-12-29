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
            
            var defStr = options.Defaults?.ConnectionString;
            var defDb = options.Defaults?.DbProvider;
            
            switch (defDb?.ToLower())
            {
                case "postgres":
                    services.AddDbContext<ApplicationContext>(m => m.UseNpgsql(e =>
                        e.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
                    break;
                case "mssql":
                    services.AddDbContext<ApplicationContext>(m => m.UseSqlServer(e =>
                        e.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
                    break;
                default:
                    throw new System.Exception("Invalid database provider!");
            }
            
            if (options.Tenants == null || !options.Tenants.Any()) return;
            
            foreach (var connectionString in options.Tenants.Select(tenant => string.IsNullOrEmpty(tenant.ConnectionString)
                         ? defStr
                         : tenant.ConnectionString))
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                dbContext.Database.SetConnectionString(connectionString);
                
                if (dbContext.Database.GetMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
        }

        private static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
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