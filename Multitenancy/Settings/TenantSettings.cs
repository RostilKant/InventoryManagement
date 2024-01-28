namespace Multitenancy.Settings
{
    public class TenantSettings
    {
        public Configuration Defaults { get; set; }
    }
    
    public class Configuration
    {
        public string DbProvider { get; set; }
        public string ConnectionString { get; set; }
        public int Counter { get; set; }
    }
}