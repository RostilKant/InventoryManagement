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
            var queryable = FindAll();
            
            var filters = GetFilters(employeeParameters);

            foreach (var filter in filters)
            {
                switch (filter)
                {
                    case "Department":
                        queryable = queryable.Where(x => x.Department.Equals(employeeParameters.Department));
                        break;
                    case "Job":
                        queryable = queryable.Where(x => x.Job.Equals(employeeParameters.Job));
                        break;
                    case "City":
                        queryable = queryable.Where(x => x.City.Equals(employeeParameters.City));
                        break;
                    case "State":
                        queryable = queryable.Where(x => x.State.Equals(employeeParameters.State));
                        break;
                    case "Country":
                        queryable = queryable.Where(x => x.Country.Equals(employeeParameters.Country));
                        break;
                    case "OfficeAddress":
                        queryable = queryable.Where(x => x.OfficeAddress.Equals(employeeParameters.OfficeAddress));
                        break;
                }
                
            }
            var result = await queryable
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
        
        private List<string> GetFilters(EmployeeParameters employeeParameters)
        {
            var result = typeof(EmployeeParameters)
                .GetProperties(BindingFlags.Public 
                               | BindingFlags.Instance 
                               | BindingFlags.DeclaredOnly)
                .ToList();

            var ret = new List<string>();

            foreach (var propertyInfo in result)
            {
                if (propertyInfo.GetValue(employeeParameters) != null)
                {
                    ret.Add(propertyInfo.Name);
                }
            }

            return ret;
        }
    }
}