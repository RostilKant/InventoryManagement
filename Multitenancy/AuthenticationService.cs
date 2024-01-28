using Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Multitenancy.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace Multitenancy
{
    public class AuthenticationService(
        IConfiguration configuration,
        IPublishEndpoint publishEndpoint,
        TenantConfigurationDbContext tenantConfigurationDbContext)
    {
        public async Task<(bool isCreated, IEnumerable<IdentityError>)> RegisterOrganizationAsync(
            OrganizationForRegistrationDto organizationForRegistration)
        {
            var tenantSettings = configuration.GetSection("TenantSettings").Get<TenantSettings>();

            var newTenant = new Tenant
            {
                Name = organizationForRegistration.Name,
                Id = organizationForRegistration.Name,
                ConnectionString = organizationForRegistration.IsSeparate
                    ? $"Host=localhost;Port=5432;Database=assets-management-{organizationForRegistration.Name};Username=postgres;Password=postgres;" //taken from env variables
                    : tenantSettings.Defaults.ConnectionString
                        .Replace("counter", tenantSettings.Defaults.Counter.ToString()),
                IsSeparate = organizationForRegistration.IsSeparate
            };

            tenantConfigurationDbContext.Tenants.Add(newTenant);
            await tenantConfigurationDbContext.SaveChangesAsync();

            var tenants = await tenantConfigurationDbContext.Tenants.ToListAsync();

            if (tenants.Count != 0 && tenants.Count(x => !x.IsSeparate) % 5 == 0)
            {
                tenantSettings.Defaults.Counter++;
            }

            configuration.GetSection("TenantSettings").Value = tenantSettings.ToString();

            var json = await File.ReadAllTextAsync("appsettings.Development.json");
            var jsonObject = JObject.Parse(json);

            jsonObject["TenantSettings"] = JToken.FromObject(tenantSettings);

            var output = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            await File.WriteAllTextAsync("appsettings.Development.json", output);

            await publishEndpoint.Publish(new NewTenantMessage
            {
                ConnectionString = newTenant.ConnectionString,
                IsSeparate = newTenant.IsSeparate
            });

            return (true, null);
        }
    }
}