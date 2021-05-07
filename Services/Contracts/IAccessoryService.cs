using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects.Accessory;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IAccessoryService
    {
        public Task<(IEnumerable<AccessoryDto>, Metadata)> GetManyAsync(AccessoryParameters accessoryParameters);
        public Task<AccessoryDto> GetOneById(Guid id);
        public Task<AccessoryDto> CreateAsync(AccessoryForCreationDto accessoryForCreation);
        public Task<bool> DeleteAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, AccessoryForUpdateDto accessoryForUpdate);
    }
}