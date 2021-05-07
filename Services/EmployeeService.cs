﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.Device;
using Entities.DataTransferObjects.Employee;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.Extensions.Logging;
using Repository.Contracts;
using Services.Contracts;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repositoryManager, ILogger<EmployeeService> logger, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<(IEnumerable<EmployeeDto>, Metadata)> GetManyAsync(EmployeeParameters employeeParameters)
        {
            var employees = await _repositoryManager.Employee.GetAllEmployees(employeeParameters);
            return (_mapper.Map<IEnumerable<EmployeeDto>>(employees), employees.Metadata);
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(id);
            
            if (employee == null)
                _logger.LogInformation("There is no employee with {Id}", id);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> CreateAsync(EmployeeForCreationDto employeeForCreation)
        {
            var employee = _mapper.Map<Employee>(employeeForCreation);
            
            _repositoryManager.Employee.CreateEmployee(employee);
            await _repositoryManager.SaveAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(id);
            
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
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(id);
            
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
        
        public async Task<bool> AssignDeviceAsync(Guid id, Guid deviceId)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(id);
            var device = await _repositoryManager.Device.GetDeviceAsync(deviceId);

            if (employee == null || device == null)
                return false;
            
            _repositoryManager.Employee.UpdateEmployee(employee);

            if (employee.Devices.Select(x => x.Id).Contains(device.Id))
            {
                _logger.LogWarning("Project with id {Id} is already exists", id);
                return false;
            }
            
            employee.Devices.Add(device);
            await _repositoryManager.SaveAsync();

            return true;
        }
        
        public async Task<bool> UnAssignDeviceAsync(Guid id, Guid deviceId)
        {
            var employee = await _repositoryManager.Employee.GetEmployeeAsync(id);
            var device = employee.Devices.SingleOrDefault(x => x.Id == deviceId);

            if (device == null)
                return false;
            
            _repositoryManager.Employee.UpdateEmployee(employee);

            if (!employee.Devices.Select(x => x.Id).Contains(device.Id))
            {
                _logger.LogWarning("Project with id {Id} doesn't exist", id);
                return false;
            }

            employee.Devices.Remove(device);
            await _repositoryManager.SaveAsync();

            return true;
        }
    }
}