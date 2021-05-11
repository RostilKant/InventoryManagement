using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Employee;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Services.Contracts;
using Services.ServiceExtensions;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUserService _userService;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        private Guid CurrentUserId =>  _contextAccessor.HttpContext?.User.GetCurrentUserId() ?? Guid.Empty;

        public EmployeeService(
            IRepositoryManager repositoryManager,
            IUserService userService,
            ILogger<EmployeeService> logger, 
            IMapper mapper, 
            IHttpContextAccessor contextAccessor)
        {
            _repositoryManager = repositoryManager;
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }


        public async Task<(IEnumerable<EmployeeDto>, Metadata)> GetManyAsync(EmployeeParameters employeeParameters)
        {
            var employees = await _repositoryManager.Employee
                .GetAllEmployees(CurrentUserId, employeeParameters);
            return (_mapper.Map<IEnumerable<EmployeeDto>>(employees), employees.Metadata);
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(CurrentUserId, id);
            
            if (employee == null)
                _logger.LogInformation("There is no employee with {Id}", id);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> CreateAsync(EmployeeForCreationDto employeeForCreation)
        {
            var employee = _mapper.Map<Employee>(employeeForCreation);
            employee = await _userService.BindAssetWithUserAsync(CurrentUserId, employee);

            _repositoryManager.Employee.CreateEmployee(employee);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(CurrentUserId, id);
            
            if (employee == null)
            {
                _logger.LogWarning("There is no employee with {Id}", id);
                return false;
            }
            
            _repositoryManager.Employee.DeleteEmployee(employee);
            await _repositoryManager.SaveAsync();
            
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, EmployeeForUpdateDto employeeForUpdate)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(CurrentUserId, id);
            
            if (employee == null)
            {
                _logger.LogWarning("There is no employee with {Id}", id);
                return false;
            }
            
            _mapper.Map(employeeForUpdate, employee);
            
            _repositoryManager.Employee.UpdateEmployee(employee);
            await _repositoryManager.SaveAsync();
            
            return true;
        }
        
        public async Task<bool> ManipulateDeviceAsync(Guid id, AssetForAssignDto assetForAssign)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(CurrentUserId, id, true);
            var device = await _repositoryManager.Device.GetDeviceAsync(CurrentUserId, assetForAssign.AssetId);
            
            if (device == null || employee == null)
                return false;
            
            var tryFindDevice = employee.Devices?.SingleOrDefault(x => x.Id.Equals(assetForAssign.AssetId));
            
            switch (assetForAssign.AssignType)
            {
                case AssetAssignType.Adding when tryFindDevice != null:
                    _logger.LogWarning("License with id {Id} is already exists", device.Id);
                    return false;
                case AssetAssignType.Removing when tryFindDevice == null:
                    _logger.LogWarning("License with id {Id} doesn't exist", device.Id);
                    return false;
                case AssetAssignType.Adding:
                    employee.Devices?.Add(device);
                    break;
                case AssetAssignType.Removing:
                    employee.Devices?.Remove(tryFindDevice);
                    break;
            }
            
            _repositoryManager.Employee.UpdateEmployee(employee);
            await _repositoryManager.SaveAsync();

            return true;
        }

        public async Task<bool> ManipulateLicenseAsync(Guid id, AssetForAssignDto assetForAssign)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(CurrentUserId, id, true);
            var license = await _repositoryManager.License.GetLicenseAsync(CurrentUserId, assetForAssign.AssetId);
            
            if (license == null || employee == null)
                return false;
            
            var tryFindLicense = employee.Licenses?.SingleOrDefault(x => x.Id.Equals(assetForAssign.AssetId));
            
            switch (assetForAssign.AssignType)
            {
                case AssetAssignType.Adding when tryFindLicense != null:
                    _logger.LogWarning("License with id {Id} is already exists", license.Id);
                    return false;
                case AssetAssignType.Removing when tryFindLicense == null:
                    _logger.LogWarning("License with id {Id} doesn't exist", license.Id);
                    return false;
                case AssetAssignType.Adding:
                    employee.Licenses?.Add(license);
                    break;
                case AssetAssignType.Removing:
                    employee.Licenses?.Remove(tryFindLicense);
                    break;
            }
            
            _repositoryManager.Employee.UpdateEmployee(employee);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}