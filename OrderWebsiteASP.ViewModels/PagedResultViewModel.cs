using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWebsiteASP.ViewModels
{
    public class PagedResultViewModel<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        public PaginationViewModel Pagination { get; set; } = new();
    }
}
