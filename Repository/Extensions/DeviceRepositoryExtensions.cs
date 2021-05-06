using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Entities.Enums;
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

        public static IQueryable<Device> Search(this IQueryable<Device> queryable, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return queryable;
            }

            var lowerTerm = searchTerm.ToLower();
            Guid.TryParse(searchTerm, out var id);
            Enum.TryParse<DeviceCategory>(searchTerm, out var category);
            Enum.TryParse<AssetStatus>(searchTerm, out var status);
            
            return queryable.Where(x => 
                x.Imei.ToLower().Contains(lowerTerm) ||
                x.Category.Equals(category) ||
                x.OfficeAddress.ToLower().Contains(lowerTerm) ||
                x.Manufacturer.ToLower().Contains(lowerTerm) ||
                x.Model.ToLower().Contains(lowerTerm) ||
                x.Notes.ToLower().Contains(lowerTerm) ||
                x.Serial.ToLower().Contains(lowerTerm) ||
                x.Status.Equals(status) ||
                x.MacAddress.ToLower().Contains(lowerTerm) ||
                x.Id.Equals(id)
            );
        }

        public static IQueryable<Device> Sort(this IQueryable<Device> queryable, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return queryable.OrderBy(d => d.PurchaseDate);

            var orderQueryParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Device).GetProperties();

            var orderQuery = "";
            
            foreach (var param in orderQueryParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var paramProperty = param.Split(' ')[0];
                var objProperty = propertyInfos.FirstOrDefault(x =>
                    x.Name.Equals(paramProperty, StringComparison.InvariantCultureIgnoreCase));

                if (objProperty == null)
                    continue;

                var sortOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQuery += $"{objProperty.Name} {sortOrder},";
            }

            orderQuery = orderQuery.TrimEnd(',', ' ');
            
            return string.IsNullOrWhiteSpace(orderQuery) ? 
                queryable.OrderBy(d => d.PurchaseDate) : queryable.OrderBy(orderByQueryString);
        }
    }
}