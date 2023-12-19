using System.Collections.Generic;

namespace Entities.Settings
{
    public class TenantSettings
    {
        public Configuration Defaults { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
    
    public class Tenant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public bool IsSeparate { get; set; }
    }
    
    public class Configuration
    {
        public string DbProvider { get; set; }
        public string ConnectionString { get; set; }
        public int Counter { get; set; }
    }
}