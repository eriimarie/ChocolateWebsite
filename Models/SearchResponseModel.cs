using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Models
{
    public class SearchResponseModel
    {
        public SearchCriteria Criteria { get; set; }

        List<ProductModel> Products { get; set; }

        public int TotalRecord { get { return Products.Count(); } }

    }

    public class SearchCriteria
    {
        public List<string> Tags { get; set; }
        public string SortBy { get; set; }
        public string SearchTerm { get; set; }
        public int PazeSize { get; set; }
        public int PageNumber { get; set; }
        public string PriceRange { get; set; }

    }
}
