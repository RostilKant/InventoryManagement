namespace Multitenancy
{
    public class NewTenantMessage
    {
        public string ConnectionString { get; set; }
        public bool IsSeparate { get; set; }
        
        public bool IsMigration { get; set; }
    }
}