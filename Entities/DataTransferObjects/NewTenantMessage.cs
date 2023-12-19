namespace Entities.DataTransferObjects
{
    public class NewTenantMessage
    {
        public string ConnectionString { get; set; }
        public bool IsSeparate { get; set; }
    }
}