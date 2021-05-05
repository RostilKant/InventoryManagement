using System.Collections.Generic;
using System.Linq;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Extensions
{
    public static class DeviceRepositoryExtensions
    {
        public static IQueryable<Device> FilterBy(this IQueryable<Device> queryable, IEnumerable<string> filters,
            DeviceParameters deviceParameters)
        {
            foreach (var filter in filters)
            {
                queryable = filter switch
                {
                    "Model" => queryable.Where(x => x.Model.Equals(deviceParameters.Model)),
                    "Category" => queryable.Where(x => x.Category.Equals(deviceParameters.Category)),
                    "Status" => queryable.Where(x => x.Status.Equals(deviceParameters.Status)),
                    "Manufacturer" => queryable.Where(x => x.Manufacturer.Equals(deviceParameters.Manufacturer)),
                    "OfficeAddress" => queryable.Where(x => x.OfficeAddress.Equals(deviceParameters.OfficeAddress)),
                    _ => queryable
                };
            }

            return queryable;
        }
    }
}