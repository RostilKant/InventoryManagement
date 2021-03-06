﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class EmployeeRepositoryExtensions
    {
        public static IQueryable<Employee> FilterBy(this IQueryable<Employee> queryable,
            EmployeeParameters employeeParameters)
        {
            var filters = employeeParameters.GetFilters();
                
            foreach (var filter in filters)
            {
                queryable = filter switch
                {
                    "Department" => queryable.Where(x => x.Department.Equals(employeeParameters.Department)),
                    "Job" => queryable.Where(x => x.Job.Equals(employeeParameters.Job)),
                    "City" => queryable.Where(x => x.City.Equals(employeeParameters.City)),
                    "State" => queryable.Where(x => x.State.Equals(employeeParameters.State)),
                    "Country" => queryable.Where(x => x.Country.Equals(employeeParameters.Country)),
                    "OfficeAddress" => queryable.Where(x => x.OfficeAddress.Equals(employeeParameters.OfficeAddress)),
                    _ => queryable
                };
            }

            return queryable;
        }
        
        public static IQueryable<Employee> Search(this IQueryable<Employee> queryable, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return queryable;

            var splitTerm = searchTerm.Split(' ');
            var lowerTerm = string.Join("", searchTerm.ToLower().Split(' '));
            Enum.TryParse<EmployeeDepartment>(string.Join('_', splitTerm), out var department);

            return queryable.Where(x => 
                x.Address.ToLower().Contains(lowerTerm) ||
                x.City.ToLower().Contains(lowerTerm) ||
                x.OfficeAddress.ToLower().Contains(lowerTerm) ||
                x.Country.ToLower().Contains(lowerTerm) ||
                x.State.ToLower().Contains(lowerTerm) ||
                x.ZipCode.ToLower().Contains(lowerTerm) ||
                x.Department.Equals(department) ||
                x.Job.ToLower().Contains(lowerTerm) ||
                x.Phone.ToLower().Contains(lowerTerm) ||
                x.FirstName.ToLower().Contains(lowerTerm) ||
                x.LastName.ToLower().Contains(lowerTerm) ||
                (x.FirstName.ToLower() + x.LastName.ToLower()).Contains(lowerTerm)
                );
        }

        public static IQueryable<Employee> Sort(this IQueryable<Employee> queryable, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return queryable.OrderBy(e => e.EmploymentDate);

            var orderParams = orderByQueryString.Trim().Split(',');

            if (orderParams[0].Contains("fullName"))
            {
                var splitOrder = orderParams[0].Split(' ');
                orderParams = new[]
                {
                    $"firstName {splitOrder[1]}",
                    $"lastName {splitOrder[1]}"
                };
            }
            
            var propertyInfos = typeof(Employee).GetProperties(BindingFlags.Public
                                                               | BindingFlags.Instance);

            var orderQuery = "";

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyName = param.Split(' ')[0];
                var objProperty = propertyInfos.FirstOrDefault(p =>
                    p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

                if (objProperty == null)
                    continue;

                var sortOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQuery += $"{objProperty.Name} {sortOrder},";
            }

            orderQuery = orderQuery.TrimEnd(',', ' ');

            return string.IsNullOrWhiteSpace(orderQuery) ? 
                queryable.OrderBy(e => e.EmploymentDate) : queryable.OrderBy(orderQuery);
        }
    }
}