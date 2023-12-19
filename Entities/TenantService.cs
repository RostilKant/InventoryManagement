using System;
using System.Linq;
using Entities.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Entities
{
    public class TenantService : ITenantService
    {
        private Tenant _currentTenant;
        private readonly IConfiguration _configuration;
        
        public TenantService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            var context = contextAccessor.HttpContext;

            if (context != null)
            {
                if (context.User.Claims.FirstOrDefault(x => x.Type == "tenant")?.Value is { } tenantId)
                    SetTenant(tenantId);
            
                //try to find tenantId in route
                if (context.Request.Query.TryGetValue("tenantId", out var tenantIdRouteValue))
                    SetTenant(tenantIdRouteValue);
            }
        }
        private void SetTenant(string tenantId)
        {
            _currentTenant = _configuration.GetSection("TenantSettings").Get<TenantSettings>().Tenants.FirstOrDefault(a => a.Id == tenantId);
            if (_currentTenant == null) throw new Exception("Invalid Tenant!");
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString)) SetDefaultConnectionStringToCurrentTenant();
        }
        private void SetDefaultConnectionStringToCurrentTenant()
            => _currentTenant.ConnectionString = _configuration.GetSection("TenantSettings").Get<TenantSettings>().Defaults.ConnectionString;

        public string GetConnectionString()
            => _currentTenant?.ConnectionString;

        public string GetDatabaseProvider()
            => _configuration.GetSection("TenantSettings").Get<TenantSettings>().Defaults?.DbProvider;
        
        public Tenant GetTenant() => _currentTenant;
    }
}