using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Extensions
{
    public static class EmployeeRepositoryExtensions
    {
        public static IQueryable<Employee> FilterBy(this IQueryable<Employee> queryable, IEnumerable<string> filters,
            EmployeeParameters employeeParameters)
        {
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
    }
}