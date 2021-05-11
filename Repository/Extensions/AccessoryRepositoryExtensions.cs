using System;
using System.Linq;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class AccessoryRepositoryExtensions
    {
        public static IQueryable<Accessory> FilterBy(this IQueryable<Accessory> queryable, 
            AccessoryParameters accessoryParameters)
        {
            var filters = accessoryParameters.GetFilters();
            
            foreach (var filter in filters)
            {
                queryable = filter switch
                {
                    "Name" => queryable.Where(x => x.Name.Equals(accessoryParameters.Name)),
                    "Category" => queryable.Where(x => x.Category.Equals(accessoryParameters.Category)),
                    "Status" => queryable.Where(x => x.Status.Equals(accessoryParameters.Status)),
                    "ModelNumber" => queryable.Where(x => x.ModelNumber.Equals(accessoryParameters.ModelNumber)),
                    "Manufacturer" => queryable.Where(x => x.Manufacturer.Equals(accessoryParameters.Manufacturer)),
                    "OfficeAddress" => queryable.Where(x => x.OfficeAddress.Equals(accessoryParameters.OfficeAddress)),
                    _ => queryable
                };
            }

            return queryable;
        }

        public static IQueryable<Accessory> Search(this IQueryable<Accessory> queryable, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return queryable;
            

            var lowerTerm = searchTerm.ToLower();
            Guid.TryParse(searchTerm, out var id);
            Enum.TryParse<AccessoryCategory>(searchTerm, out var category);
            Enum.TryParse<AssetStatus>(searchTerm, out var status);

            return queryable.Where(x => 
                x.Id.Equals(id) ||
                x.Category.Equals(category) ||
                x.Status.Equals(status) ||
                x.Manufacturer.ToLower().Contains(lowerTerm) ||
                x.Name.ToLower().Contains(lowerTerm) ||
                x.ModelNumber.ToLower().Contains(lowerTerm) ||
                x.OfficeAddress.ToLower().Contains(lowerTerm));
        }

        public static IQueryable<Accessory> Sort(this IQueryable<Accessory> queryable, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return queryable.OrderBy(d => d.PurchaseDate);
            
            var orderQueryParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Accessory).GetProperties();

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

            return string.IsNullOrWhiteSpace(orderQuery)
                ? queryable.OrderBy(d => d.PurchaseDate)
                : queryable.OrderBy(orderByQueryString);
        }
    }
}