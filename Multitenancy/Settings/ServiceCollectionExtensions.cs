using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Multitenancy.Messaging;

namespace Multitenancy.Settings
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
            
            using var scope1 = services.BuildServiceProvider().CreateScope();
            var dbContext1 = scope1.ServiceProvider.GetRequiredService<TenantConfigurationDbContext>();
            
            if (dbContext1.Database.GetMigrations().Any())
            {
                dbContext1.Database.Migrate();
            }
            
            var tenants = dbContext1.Tenants.ToList();
            
            foreach (var connectionString in tenants.Select(tenant => string.IsNullOrEmpty(tenant.ConnectionString)
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
        
        public static RabbitMqMassBuilder AddMassTransitRabbitMQ(
            this IServiceCollection services,
            Action<RabbitMQConnectionSettings> config)
        {
            services.Configure(config);
            services.AddHostedService<RabbitMqBusService>();

            return new RabbitMqMassBuilder(services);
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