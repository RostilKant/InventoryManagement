using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Entities;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.Extensions;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationContext applicationContext)
            : base(applicationContext)
        {
        }

        public async Task<PagedList<Employee>> GetAllEmployees(EmployeeParameters employeeParameters)
        {

            var result = await FindAll()
                .FilterBy(employeeParameters.GetFilters(), employeeParameters)
                .Include(x => x.Devices)
                .OrderBy(x => x.EmploymentDate)
                .ToListAsync();
            
            return PagedList<Employee>.ToPagedList(result, employeeParameters.PageNumber, employeeParameters.PageSize);
        }

        public async Task<Employee> GetEmployeeAsync(Guid id) =>
            await FindByCondition(x => x.Id == id)
                .Include(x => x.Devices)
                .SingleOrDefaultAsync();

        public void UpdateEmployee(Employee employee) => Update(employee);

        public void CreateEmployee(Employee employee) => Create(employee);
        public void CreateRangeEmployee(IEnumerable<Employee> employees) => CreateRange(employees);
        public void DeleteEmployee(Employee employee) => Delete(employee);
        public void DeleteRangeEmployee(IEnumerable<Employee> employees) => DeleteRange(employees);
        
    }
}