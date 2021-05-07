using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IRepositoryManager
    {
        IEmployeeRepository Employee { get; }
        IDeviceRepository Device { get; }
        IAccessoryRepository Accessory { get; }
        IComponentRepository Component { get; }
        IConsumableRepository Consumable { get; }
        ILicenseRepository License { get; }
        
        Task SaveAsync();
    }
}