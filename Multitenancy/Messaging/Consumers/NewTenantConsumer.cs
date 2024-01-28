using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Multitenancy.Settings;

namespace Multitenancy.Messaging.Consumers
{
    public class NewTenantConsumer : IConsumer<NewTenantMessage>
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _context;
        private readonly TenantConfigurationDbContext _tenantConfigurationDbContext;
        
        public NewTenantConsumer(IConfiguration configuration, ApplicationContext context, TenantConfigurationDbContext tenantConfigurationDbContext)
        {
            _configuration = configuration;
            _context = context;
            _tenantConfigurationDbContext = tenantConfigurationDbContext;
        }

        public async Task Consume(ConsumeContext<NewTenantMessage> context)
        {
            var tenantSettings = _configuration.GetSection("TenantSettings").Get<TenantSettings>();

            if (!context.Message.IsMigration)
            {
                if (context.Message.IsSeparate)
                {
                    _context.Database.SetConnectionString(context.Message.ConnectionString);

                    if (_context.Database.GetMigrations().Any())
                    {
                        await _context.Database.MigrateAsync();
                    }
                }

                var tenants = await _tenantConfigurationDbContext.Tenants.ToListAsync();
            
                if (tenants.Any() && tenants.Count(x => !x.IsSeparate) % 5 == 0)
                {
                    _context.Database.SetConnectionString(tenantSettings.Defaults.ConnectionString
                        .Replace("counter", tenantSettings.Defaults.Counter.ToString()));

                    if (_context.Database.GetMigrations().Any())
                    {
                        await _context.Database.MigrateAsync();
                    }
                }
            }
        }
    }
}