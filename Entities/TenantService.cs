using System;
using System.Linq;
using Entities.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Entities
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private Tenant _currentTenant;
        
        public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            _tenantSettings = tenantSettings.Value;
            
            var context = contextAccessor.HttpContext;
            
            if (context?.User.Claims.FirstOrDefault(x => x.Type == "tenant")?.Value is { } tenantId)
                SetTenant(tenantId);
        }
        private void SetTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(a => a.Id == tenantId);
            if (_currentTenant == null) throw new Exception("Invalid Tenant!");
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString)) SetDefaultConnectionStringToCurrentTenant();
        }
        private void SetDefaultConnectionStringToCurrentTenant()
            => _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;

        public string GetConnectionString()
            => _currentTenant?.ConnectionString;

        public string GetDatabaseProvider()
            => _tenantSettings.Defaults?.DbProvider;
        
        public Tenant GetTenant() => _currentTenant;
    }
}