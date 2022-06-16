using ChocoShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Models
{
    public class ProductQtyMap
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return Product.Price * Quantity;
            }
        }
    }
}
