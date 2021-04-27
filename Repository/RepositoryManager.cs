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

        public RepositoryManager(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IEmployeeRepository Employee 
            => _employeeRepository ??= new EmployeeRepository(_applicationContext);
        
        public IDeviceRepository Device 
            => _deviceRepository ??= new DeviceRepository(_applicationContext);

        public Task SaveAsync() => _applicationContext.SaveChangesAsync();
    }
}