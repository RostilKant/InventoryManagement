using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects.Device;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IDeviceService
    {
        public Task<(IEnumerable<DeviceDto>, Metadata)> GetManyAsync(DeviceParameters deviceParameters);
        public Task<DeviceDto> GetOneById(Guid id);
        public Task<DeviceDto> CreateAsync(DeviceForCreationDto deviceForCreation);
        public Task<bool> DeleteAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, DeviceForUpdateDto deviceForUpdate);
    }
}