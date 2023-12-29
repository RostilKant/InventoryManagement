using System;
using System.Linq;
using Entities.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Entities
{
    public class TenantManagerService : ITenantManagerService
    {
        private Tenant _currTenant;
        private readonly IConfiguration _configuration;
        
        public TenantManagerService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            var context = contextAccessor.HttpContext;

            if (context != null)
            {
                if (context.User.Claims
                        .FirstOrDefault(x => x.Type == "tenant")
                        ?.Value is { } tenantId)
                    SetTenant(tenantId);
            
                //try to find tenantId in route
                if (context.Request.Query.TryGetValue("tenantId", out var tenantIdRouteValue))
                    SetTenant(tenantIdRouteValue);
            }
        }
        private void SetTenant(string tenantId)
        {
            _currTenant = _configuration.GetSection("TenantSettings").Get<TenantSettings>().Tenants.FirstOrDefault(a => a.Id == tenantId);
            if (_currTenant == null) throw new Exception("Invalid Tenant!");
            if (string.IsNullOrEmpty(_currTenant.ConnectionString)) SetDefaultConnectionStringToCurrentTenant();
        }
        private void SetDefaultConnectionStringToCurrentTenant()
            => _currTenant.ConnectionString = _configuration.GetSection("TenantSettings").Get<TenantSettings>().Defaults.ConnectionString;

        public string GetConnection()
            => _currTenant?.ConnectionString;

        public string GetDbProvider()
            => _configuration.GetSection("TenantSettings").Get<TenantSettings>().Defaults?.DbProvider;
        
        public Tenant GetTenant() => _currTenant;
    }
}