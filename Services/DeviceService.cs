using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Device;
using Entities.Models;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<DeviceService> _logger;
        private readonly IMapper _mapper;

        public DeviceService(IRepositoryManager repositoryManager, ILogger<DeviceService> logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DeviceDto>> GetManyAsync()
        {
            var devices = await _repositoryManager.Device.GetAllDevicesAsync();

            if (devices == null)
            {
                _logger.LogError("Devices wasn't found in db!");
            }

            return _mapper.Map<IEnumerable<DeviceDto>>(devices);
        }

        public async Task<DeviceDto> GetOneById(Guid id)
        {
            var device = await _repositoryManager.Device.GetDeviceAsync(id);
            
            if (device == null)
            {
                _logger.LogError($"Device with id {id} doesn't exist in the db");
            }

            return _mapper.Map<DeviceDto>(device);
        }

        public async Task<DeviceDto> CreateAsync(DeviceForCreationDto deviceForCreation)
        {
            var device = _mapper.Map<Device>(deviceForCreation);
            
            _repositoryManager.Device.CreateDevice(device);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<DeviceDto>(device);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var device = await _repositoryManager.Device.GetDeviceAsync(id);
            
            _repositoryManager.Device.DeleteDevice(device);
            await _repositoryManager.SaveAsync();
            
            return device != null;
        }

        public async Task<bool> UpdateAsync(Guid id, DeviceForUpdateDto deviceForUpdate)
        {
            var device = await _repositoryManager.Device.GetDeviceAsync(id);
            _mapper.Map(deviceForUpdate, device);
            
            _repositoryManager.Device.UpdateDevice(device);
            await _repositoryManager.SaveAsync();
            
            return device != null;
        }
        
        /*private async Task<bool> EmployeeExists(Guid employeeId)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(employeeId);
            if (employee == null)
            {
                _logger.LogInformation("Employee with id {EmployeeId} doesn't exist in the db", employeeId);
                return false;
            }

            return true;
        }*/
    }
}