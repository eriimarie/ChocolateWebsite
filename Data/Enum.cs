using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Data
{
    public enum AddressType
    {
        BillingAddress = 1,
        ShippingAddress =2
    }
    
    public enum OrderStatus
    {
        Created = 1,
        Processing =2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
        Rejected = 6
    }
}
