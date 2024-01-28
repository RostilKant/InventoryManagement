namespace Multitenancy
{
    public interface ITenantable
    {
        public string TenantId { get; set; }
    }
}