using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWebsiteASP.ViewModels
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages =>
            PageSize <= 0
                ? 0
                : (int)Math.Ceiling((double)TotalItems / PageSize);

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
