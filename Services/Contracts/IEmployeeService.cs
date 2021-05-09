using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Employee;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IEmployeeService
    {
        public Task<(IEnumerable<EmployeeDto>, Metadata)> GetManyAsync(EmployeeParameters employeeParameters);
        public Task<EmployeeDto> GetByIdAsync(Guid id);
        public Task<EmployeeDto> CreateAsync(EmployeeForCreationDto employeeForCreation);
        public Task<bool> DeleteAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, EmployeeForUpdateDto employeeForUpdate);

        public Task<bool> ManipulateDeviceAsync(Guid id, AssetForAssignDto assetForAssign);
        
        public Task<bool> ManipulateLicenseAsync(Guid id, AssetForAssignDto assetForAssign);
    }
}