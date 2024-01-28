using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Multitenancy.Settings;

namespace Multitenancy
{
    public class TenantManagerService : ITenantManagerService
    {
        private Tenant _currTenant;
        private readonly IConfiguration _configuration;
        private readonly TenantConfigurationDbContext _tenantConfigurationDbContext;
        
        public TenantManagerService(IConfiguration configuration, IHttpContextAccessor contextAccessor, TenantConfigurationDbContext tenantConfigurationDbContext)
        {
            _configuration = configuration;
            _tenantConfigurationDbContext = tenantConfigurationDbContext;

            var context = contextAccessor.HttpContext;

            if (context != null)
            {
                if (context.User.Claims
                        .FirstOrDefault(x => x.Type == "tenant")
                        ?.Value is { } tenantId)
                    SetTenant(tenantId);
            
                //try to find tenantId in route
                else if (context.Request.Query.TryGetValue("tenantId", out var tenantIdRouteValue))
                    SetTenant(tenantIdRouteValue);
            }
        }
        private void SetTenant(string tenantId)
        {
            _currTenant = _tenantConfigurationDbContext.Tenants
                .FirstOrDefault(a => a.Id == tenantId);
            
            if (_currTenant == null) 
                throw new Exception("Invalid Tenant!");
            
            if (string.IsNullOrEmpty(_currTenant.ConnectionString))
                SetDefaultConnectionStringToCurrentTenant();
        }
        private void SetDefaultConnectionStringToCurrentTenant()
            => _currTenant.ConnectionString = _configuration.GetSection("TenantSettings")
                .Get<TenantSettings>()
                .Defaults
                .ConnectionString;

        public string GetConnection()
            => _currTenant?.ConnectionString;

        public string GetDbProvider()
            => _configuration.GetSection("TenantSettings")
                .Get<TenantSettings>()
                .Defaults
                ?.DbProvider;
        
        public Tenant GetTenant() => _currTenant;
    }
}