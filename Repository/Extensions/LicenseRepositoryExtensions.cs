using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Entities.Enums;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repository.Extensions
{
    public static class LicenseRepositoryExtensions
    {
        public static IQueryable<License> FilterBy(this IQueryable<License> queryable,
            LicenseParameters licenseParameters)
        {
            var filters = licenseParameters.GetFilters();

            foreach (var filter in filters)
            {
                queryable = filter switch
                {
                    "Name" => queryable.Where(x => x.Name.Equals(licenseParameters.Name)),
                    "Category" => queryable.Where(x => x.Category.Equals(licenseParameters.Category)),
                    "IsReAssignable" => queryable.Where(x => x.IsReAssignable.Equals(licenseParameters.IsReAssignable)),
                    "Manufacturer" => queryable.Where(x => x.Manufacturer.Equals(licenseParameters.Manufacturer)),
                    "LicensedToEmail" => queryable.Where(x => x.LicensedToEmail.Equals(licenseParameters.LicensedToEmail)),
                    _ => queryable
                };
            }

            return queryable;
        }

        public static IQueryable<License> Search(this IQueryable<License> queryable, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return queryable;

            var lowerTerm = searchTerm.ToLower();
            Guid.TryParse(searchTerm, out var id);
            Enum.TryParse<LicenseCategory>(searchTerm, out var category);

            return queryable.Where(x =>
                x.Id.Equals(id) ||
                x.Category.Equals(category) ||
                x.Manufacturer.ToLower().Contains(lowerTerm) ||
                x.Name.ToLower().Contains(lowerTerm) ||
                x.ProductKey.ToLower().Contains(lowerTerm));
        }

        public static IQueryable<License> Sort(this IQueryable<License> queryable, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return queryable.OrderBy(d => d.PurchaseDate);

            var orderQueryParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(License).GetProperties();

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