using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Extensions
{
    public static class ComponentRepositoryExtensions
    {
        public static IQueryable<Component> FilterBy(this IQueryable<Component> queryable,
            ComponentParameters componentParameters)
        {
            var filters = componentParameters.GetFilters();

            foreach (var filter in filters)
            {
                queryable = filter switch
                {
                    "Name" => queryable.Where(x => x.Name.Equals(componentParameters.Name)),
                    "Category" => queryable.Where(x => x.Category.Equals(componentParameters.Category)),
                    "Status" => queryable.Where(x => x.Status.Equals(componentParameters.Status)),
                    "Serial" => queryable.Where(x => x.Serial.Equals(componentParameters.Serial)),
                    "Manufacturer" => queryable.Where(x => x.Manufacturer.Equals(componentParameters.Manufacturer)),
                    "OfficeAddress" => queryable.Where(x => x.OfficeAddress.Equals(componentParameters.OfficeAddress)),
                    _ => queryable
                };
            }

            return queryable;
        }

        public static IQueryable<Component> Search(this IQueryable<Component> queryable, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return queryable;

            var lowerTerm = searchTerm.ToLower();
            Guid.TryParse(searchTerm, out var id);
            Enum.TryParse<ComponentCategory>(searchTerm, out var category);
            Enum.TryParse<AssetStatus>(searchTerm, out var status);

            return queryable.Where(x =>
                x.Id.Equals(id) ||
                x.Category.Equals(category) ||
                x.Status.Equals(status) ||
                x.Manufacturer.ToLower().Contains(lowerTerm) ||
                x.Name.ToLower().Contains(lowerTerm) ||
                x.Serial.ToLower().Contains(lowerTerm) ||
                x.OfficeAddress.ToLower().Contains(lowerTerm));
        }

        public static IQueryable<Component> Sort(this IQueryable<Component> queryable, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return queryable.OrderBy(d => d.PurchaseDate);

            var orderQueryParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Component).GetProperties();

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