using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Extensions
{
    public static class ConsumableRepositoryExtensions
    {
         public static IQueryable<Consumable> FilterBy(this IQueryable<Consumable> queryable,
            ConsumableParameters consumableParameters)
        {
            var filters = consumableParameters.GetFilters();

            foreach (var filter in filters)
            {
                queryable = filter switch
                {
                    "Name" => queryable.Where(x => x.Name.Equals(consumableParameters.Name)),
                    "Category" => queryable.Where(x => x.Category.Equals(consumableParameters.Category)),
                    "Status" => queryable.Where(x => x.Status.Equals(consumableParameters.Status)),
                    "Manufacturer" => queryable.Where(x => x.Manufacturer.Equals(consumableParameters.Manufacturer)),
                    "OfficeAddress" => queryable.Where(x => x.OfficeAddress.Equals(consumableParameters.OfficeAddress)),
                    _ => queryable
                };
            }

            return queryable;
        }

        public static IQueryable<Consumable> Search(this IQueryable<Consumable> queryable, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return queryable;

            var lowerTerm = searchTerm.ToLower();
            Guid.TryParse(searchTerm, out var id);
            Enum.TryParse<ConsumableCategory>(searchTerm, out var category);
            Enum.TryParse<AssetStatus>(searchTerm, out var status);

            return queryable.Where(x =>
                x.Id.Equals(id) ||
                x.Category.Equals(category) ||
                x.Status.Equals(status) ||
                x.Manufacturer.ToLower().Contains(lowerTerm) ||
                x.Name.ToLower().Contains(lowerTerm) ||
                x.OfficeAddress.ToLower().Contains(lowerTerm));
        }

        public static IQueryable<Consumable> Sort(this IQueryable<Consumable> queryable, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return queryable.OrderBy(d => d.PurchaseDate);

            var orderQueryParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Consumable).GetProperties();

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