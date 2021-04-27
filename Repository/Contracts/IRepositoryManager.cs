using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IRepositoryManager
    {
        IEmployeeRepository Employee { get; }
        IDeviceRepository Device { get; }
        
        Task SaveAsync();
    }
}