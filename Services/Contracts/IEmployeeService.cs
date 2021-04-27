using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Device;
using Entities.DataTransferObjects.Employee;
using Entities.Models;

namespace Services.Contracts
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<EmployeeDto>> GetManyAsync();
        public Task<EmployeeDto> GetByIdAsync(Guid id);
        public Task<EmployeeDto> CreateAsync(EmployeeForCreationDto employeeForCreation);
        public Task<bool> DeleteAsync(Guid id);
        public Task<bool> UpdateAsync(Guid id, EmployeeForUpdateDto employeeForUpdate);

        public Task<bool> AssignDeviceAsync(Guid id, Guid deviceId);
        public Task<bool> UnAssignDeviceAsync(Guid id, Guid deviceId);

    }
}