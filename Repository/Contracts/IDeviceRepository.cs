using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface IDeviceRepository : IRepositoryBase<Device>
    {
        Task<PagedList<Device>> GetAllDevicesAsync(DeviceParameters deviceParameters);
        Task<Device> GetDeviceAsync(Guid id);
        void UpdateDevice(Device device);
        void CreateDevice(Device device);
        void DeleteDevice(Device device);

        Task<IEnumerable<Device>> GetAllEmployeeDevicesAsync(Guid employeeId);
    }
}