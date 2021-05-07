using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects.Component;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IComponentService
    {
        public Task<(IEnumerable<ComponentDto>, Metadata)> GetManyAsync(ComponentParameters componentParameters);
        public Task<ComponentDto> GetOneById(Guid id);
        public Task<ComponentDto> CreateAsync(ComponentForCreationDto componentForCreation);
        public Task<bool> DeleteAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, ComponentForUpdateDto componentForUpdate);
    }
}