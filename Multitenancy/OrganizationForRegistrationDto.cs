namespace Multitenancy
{
    public abstract class OrganizationForRegistrationDto(string name)
    {
        public string Name { get; set; } = name;

        public bool IsSeparate { get; set; }
    }
}