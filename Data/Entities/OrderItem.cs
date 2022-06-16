using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Data.Entities
{
    public class OrderItem : BaseEntity
    {
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }

        //Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
