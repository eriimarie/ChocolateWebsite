using ChocoShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Models
{
    public class ProductSearchResponseModel
    {
        public List<Product> Products { get; set; }

        public int TotalPage { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPageNumber { get; set; }

        public int NextPageNumber { get; set; }
        public int PreviousPageNumber { get; set; }

        public string SearchTerm { get; set; }
    }
}
