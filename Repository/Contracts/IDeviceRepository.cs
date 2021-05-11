using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface IDeviceRepository : IRepositoryBase<Device>
    {
        Task<PagedList<Device>> GetAllDevicesAsync(Guid userId, DeviceParameters deviceParameters);
        Task<Device> GetDeviceAsync(Guid userId, Guid id, bool trackChanges = false);
        void UpdateDevice(Device device);
        void CreateDevice(Device device);
        void DeleteDevice(Device device);

        Task<IEnumerable<Device>> GetAllEmployeeDevicesAsync(Guid employeeId);
    }
}