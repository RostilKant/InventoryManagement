using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository
{
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        public DeviceRepository(ApplicationContext applicationContext)
            : base(applicationContext)
        {
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync() =>
            await FindAll()
                .Include(x => x.Employee)
                .ToListAsync();

        public async Task<Device> GetDeviceAsync(Guid id) =>
            await FindByCondition(x => x.Id == id)
                .Include(x => x.Employee)
                .SingleOrDefaultAsync();

        public void UpdateDevice(Device device) => Update(device);
        
        public void CreateDevice(Device device) => Create(device);
        public void DeleteDevice(Device device) => Delete(device);

        public async Task<IEnumerable<Device>> GetAllEmployeeDevicesAsync(Guid employeeId) =>
            await FindByCondition(x => x.Employee.Id.Equals(employeeId))
                .ToListAsync();
    }
}