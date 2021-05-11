using System;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Contracts
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task<PagedList<Employee>> GetAllEmployees(Guid userId, EmployeeParameters employeeParameters);
        Task<Employee> GetEmployeeAsync(Guid userId, Guid id, bool trackChanges = false);
        void UpdateEmployee(Employee employee);
        void CreateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
    }
}