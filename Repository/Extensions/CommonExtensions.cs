using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entities.RequestFeatures;

namespace Repository.Extensions
{
    public static class CommonExtensions
    {
        public static IEnumerable<string> GetFilters(this RequestParameters requestParameters)
        {
            var result = new List<string>();

            var properties = requestParameters.GetType()
                .GetProperties(BindingFlags.Public 
                               | BindingFlags.Instance 
                               | BindingFlags.DeclaredOnly)
                .ToList();
            
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetValue(requestParameters) != null)
                {
                    result.Add(propertyInfo.Name);
                }
            }

            return result;
        }
    }
}