using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Models
{
    public class CartModel
    {
        public string productId { get; set; }

        public int quantity { get; set; }
    }
}
