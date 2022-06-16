using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Models
{
    public class ProductSearchRequestModel
    {
        public string SearchTerm { get; set; }

        public List<string> Tags { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public string SortBy { get; set; }

        public int ItemPerPage { get; set; }

        public int PageNumber { get; set; }

    }
}
