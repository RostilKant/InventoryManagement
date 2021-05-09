using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Device;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;
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

        public async Task<(IEnumerable<DeviceDto>, Metadata)> GetManyAsync(DeviceParameters deviceParameters)
        {
            var devices = await _repositoryManager.Device.GetAllDevicesAsync(deviceParameters);

            if (devices == null)
            {
                _logger.LogError("Devices wasn't found in db!");
            }
            
            return (_mapper.Map<IEnumerable<DeviceDto>>(devices), devices?.Metadata);
        }

        public async Task<DeviceDto> GetOneById(Guid id)
        {
            var device = await _repositoryManager.Device.GetDeviceAsync(id);
            
            if (device == null)
                _logger.LogError("Device with id {Id} doesn't exist in the db", id);

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
            var device = await _repositoryManager.Device.GetDeviceAsync(id, true);
            _mapper.Map(deviceForUpdate, device);
            
            _repositoryManager.Device.UpdateDevice(device);
            await _repositoryManager.SaveAsync();
            
            return device != null;
        }

        public async Task<bool> ManipulateAccessoryAsync(Guid id, AssetForAssignDto assetForAssign)
        {
            var device = await _repositoryManager.Device.GetDeviceAsync(id, true);
            var accessory = await _repositoryManager.Accessory.GetAccessoryAsync(assetForAssign.AssetId);
            
            if (accessory == null || device == null)
                return false;
            
            var tryFindAccessory = device.Accessories?.SingleOrDefault(x => x.Id.Equals(assetForAssign.AssetId));
            
            switch (assetForAssign.AssignType)
            {
                case AssetAssignType.Adding when tryFindAccessory != null:
                    _logger.LogWarning("Accessory with id {Id} is already exists", accessory.Id);
                    return false;
                case AssetAssignType.Removing when tryFindAccessory == null:
                    _logger.LogWarning("Accessory with id {Id} doesn't exist", accessory.Id);
                    return false;
                case AssetAssignType.Adding:
                    device.Accessories?.Add(accessory);
                    break;
                case AssetAssignType.Removing:
                    device.Accessories?.Remove(tryFindAccessory);
                    break;
            }
            
            _repositoryManager.Device.UpdateDevice(device);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> ManipulateComponentAsync(Guid id, AssetForAssignDto assetForAssign)
        {
            var device = await _repositoryManager.Device.GetDeviceAsync(id, true);
            var component = await _repositoryManager.Component.GetComponentAsync(assetForAssign.AssetId);
            
            if (component == null || device == null)
                return false;
            
            var tryFindComponent = device.Components?.SingleOrDefault(x => x.Id.Equals(assetForAssign.AssetId));
            
            switch (assetForAssign.AssignType)
            {
                case AssetAssignType.Adding when tryFindComponent != null:
                    _logger.LogWarning("Component with id {Id} is already exists", component.Id);
                    return false;
                case AssetAssignType.Removing when tryFindComponent == null:
                    _logger.LogWarning("Component with id {Id} doesn't exist", component.Id);
                    return false;
                case AssetAssignType.Adding:
                    device.Components?.Add(component);
                    break;
                case AssetAssignType.Removing:
                    device.Components?.Remove(tryFindComponent);
                    break;
            }
            
            _repositoryManager.Device.UpdateDevice(device);
            await _repositoryManager.SaveAsync();

            return true;    
        }

        public async Task<bool> ManipulateConsumableAsync(Guid id, AssetForAssignDto assetForAssign)
        {
            var device = await _repositoryManager.Device.GetDeviceAsync(id, true);
            var consumable = await _repositoryManager.Consumable.GetConsumableAsync(assetForAssign.AssetId);
            
            if (consumable == null || device == null)
                return false;
            
            var tryFindConsumable = device.Consumables?.SingleOrDefault(x => x.Id.Equals(assetForAssign.AssetId));
            
            switch (assetForAssign.AssignType)
            {
                case AssetAssignType.Adding when tryFindConsumable != null:
                    _logger.LogWarning("Consumable with id {Id} is already exists", consumable.Id);
                    return false;
                case AssetAssignType.Removing when tryFindConsumable == null:
                    _logger.LogWarning("Consumable with id {Id} doesn't exist", consumable.Id);
                    return false;
                case AssetAssignType.Adding:
                    device.Consumables?.Add(consumable);
                    break;
                case AssetAssignType.Removing:
                    device.Consumables?.Remove(tryFindConsumable);
                    break;
            }
            
            _repositoryManager.Device.UpdateDevice(device);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}