namespace Multitenancy
{
    public interface ITenantManagerService
    {
        public string GetDbProvider();
        public string GetConnection();
        public Tenant GetTenant();
    }
}