using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects.Consumable;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IConsumableService
    {
        public Task<(IEnumerable<ConsumableDto>, Metadata)> GetManyAsync(ConsumableParameters consumableParameters);
        public Task<ConsumableDto> GetOneById(Guid id);
        public Task<ConsumableDto> CreateAsync(ConsumableForCreationDto consumableForCreation);
        public Task<bool> DeleteAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, ConsumableForUpdateDto consumableForUpdate);
    }
}