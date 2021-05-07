using System.Threading.Tasks;
using Entities;
using Repository.Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationContext _applicationContext;

        private IEmployeeRepository _employeeRepository;
        private IDeviceRepository _deviceRepository;
        private IComponentRepository _componentRepository;
        private IConsumableRepository _consumableRepository;
        private IAccessoryRepository _accessoryRepository;
        private ILicenseRepository _licenseRepository;

        public RepositoryManager(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IEmployeeRepository Employee 
            => _employeeRepository ??= new EmployeeRepository(_applicationContext);
        
        public IDeviceRepository Device 
            => _deviceRepository ??= new DeviceRepository(_applicationContext);
                        
        public IAccessoryRepository Accessory
            => _accessoryRepository ??= new AccessoryRepository(_applicationContext);

        public IComponentRepository Component
            => _componentRepository ??= new ComponentRepository(_applicationContext);

        public IConsumableRepository Consumable
            => _consumableRepository ??= new ConsumableRepository(_applicationContext);

        public ILicenseRepository License
            => _licenseRepository ??= new LicenseRepository(_applicationContext);

        public Task SaveAsync() => _applicationContext.SaveChangesAsync();
    }
}