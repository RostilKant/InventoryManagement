using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public Metadata Metadata { get; set; }

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Metadata = new Metadata
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = count,
                TotalPages = (int) Math.Ceiling(count / (double)pageSize)
            };
            
            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            IEnumerable<T> enumerable = source.ToList();
            
            var count = enumerable.Count();
            var items = enumerable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}