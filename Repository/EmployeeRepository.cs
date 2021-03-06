﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
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

        public async Task<PagedList<Employee>> GetAllEmployees(Guid userId, EmployeeParameters employeeParameters)
        {

            var result = 
                await FindByCondition(x => x.User.Id.Equals(userId))
                .FilterBy(employeeParameters)
                .Search(employeeParameters.SearchTerm)
                .Sort(employeeParameters.OrderBy)
                .Include(x => x.Devices)
                .Include(x => x.Licenses)
                .ToListAsync();
            
            return PagedList<Employee>.ToPagedList(result, employeeParameters.PageNumber, employeeParameters.PageSize);
        }

        public async Task<Employee> GetEmployeeAsync(Guid userId, Guid id, bool trackChanges = false) =>
            await FindByCondition(x => x.Id.Equals(id) && x.User.Id.Equals(userId), trackChanges)
                .Include(x => x.Devices)
                .Include(x => x.Licenses)
                .SingleOrDefaultAsync();

        public void UpdateEmployee(Employee employee) => Update(employee);

        public void CreateEmployee(Employee employee) => Create(employee);
        public void CreateRangeEmployee(IEnumerable<Employee> employees) => CreateRange(employees);
        public void DeleteEmployee(Employee employee) => Delete(employee);
        public void DeleteRangeEmployee(IEnumerable<Employee> employees) => DeleteRange(employees);
        
    }
}