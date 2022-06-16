using ChocoShop.Data;
using ChocoShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Models
{
    public class HomeModel
    {
        public IEnumerable<Product> Featured { get; set; }
        public IEnumerable<Product> BestSeller { get; set; }
    }
}
