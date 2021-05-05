using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
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

        public async Task<PagedList<Device>> GetAllDevicesAsync(DeviceParameters deviceParameters)
        {
            var queryable = FindAll();
            
            var filters = GetFilters(deviceParameters);

            foreach (var filter in filters)
            {
                switch (filter)
                {
                    case "Model":
                        queryable = queryable.Where(x => x.Model.Equals(deviceParameters.Model));
                        break;
                    case "Category":
                        queryable = queryable.Where(x => x.Category.Equals(deviceParameters.Category));
                        break;
                    case "Status":
                        queryable = queryable.Where(x => x.Status.Equals(deviceParameters.Status));
                        break;
                    case "Manufacturer":
                        queryable = queryable.Where(x => x.Manufacturer.Equals(deviceParameters.Manufacturer));
                        break;
                    case "OfficeAddress":
                        queryable = queryable.Where(x => x.OfficeAddress.Equals(deviceParameters.OfficeAddress));
                        break;
                }
                
            }
            
            var result = await queryable
                .Include(x => x.Employee)
                .ToListAsync();

            return PagedList<Device>.ToPagedList(result, deviceParameters.PageNumber, deviceParameters.PageSize);
        }

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

        private List<string> GetFilters(DeviceParameters deviceParameters)
        {
            var result = typeof(DeviceParameters)
                .GetProperties(BindingFlags.Public 
                               | BindingFlags.Instance 
                               | BindingFlags.DeclaredOnly)
                .ToList();

            var ret = new List<string>();

            foreach (var propertyInfo in result)
            {
                if (propertyInfo.GetValue(deviceParameters) != null)
                {
                    ret.Add(propertyInfo.Name);
                }
            }

            return ret;
        }
    }
}