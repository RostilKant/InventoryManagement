using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.DataTransferObjects;
using Entities.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Messaging.Consumers
{
    public class NewTenantConsumer : IConsumer<NewTenantMessage>
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _context;
        public NewTenantConsumer(IConfiguration configuration, ApplicationContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task Consume(ConsumeContext<NewTenantMessage> context)
        {
            var tenantSettings = _configuration.GetSection("TenantSettings").Get<TenantSettings>();

            if (context.Message.IsSeparate)
            {
                _context.Database.SetConnectionString(context.Message.ConnectionString);

                if (_context.Database.GetMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }
            }

            if (tenantSettings.Tenants.Count(x => !x.IsSeparate) % 10 == 0)
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