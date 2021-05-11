using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.Extensions;

namespace Repository
{
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        public DeviceRepository(ApplicationContext applicationContext)
            : base(applicationContext)
        {
        }

        public async Task<PagedList<Device>> GetAllDevicesAsync(Guid userId, DeviceParameters deviceParameters)
        {
            var result = await FindByCondition(x => x.User.Id.Equals(userId))
                .FilterBy(deviceParameters)
                .Search(deviceParameters.SearchTerm)
                .Sort(deviceParameters.OrderBy)
                .Include(x => x.Employee)
                .Include(x => x.Accessories)
                .Include(x => x.Components)
                .Include(x => x.Consumables)
                .ToListAsync();

            return PagedList<Device>.ToPagedList(result, deviceParameters.PageNumber, deviceParameters.PageSize);
        }

        public async Task<Device> GetDeviceAsync(Guid userId, Guid id, bool trackChanges = false) =>
            await FindByCondition(x => x.Id.Equals(id) && x.User.Id.Equals(userId), trackChanges)
                .Include(x => x.Employee)
                .Include(x => x.Accessories)
                .Include(x => x.Components)
                .Include(x => x.Consumables)
                .SingleOrDefaultAsync();

        public void UpdateDevice(Device device) => Update(device);

        public void CreateDevice(Device device) => Create(device);
        public void DeleteDevice(Device device) => Delete(device);

        public async Task<IEnumerable<Device>> GetAllEmployeeDevicesAsync(Guid employeeId) =>
            await FindByCondition(x => x.Employee.Id.Equals(employeeId))
                .ToListAsync();
    }
}