using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task<PagedList<Employee>> GetAllEmployees(EmployeeParameters employeeParameters);
        Task<Employee> GetEmployeeAsync(Guid id);
        void UpdateEmployee(Employee employee);
        void CreateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
    }
}