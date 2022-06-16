using ChocoShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Models
{
    public class ShoppingCartModel
    {
        public List<ProductQtyMap> Products { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Shipping
        {
            get
            {
                return (Subtotal * (decimal).12);
            }
        }
        public decimal Tax
        {
            get
            {
                return  (Subtotal * (decimal).05);
            }
        }
        public decimal GrandTotal
        {
            get
            {
                return Subtotal - Shipping - Tax;
            }
        }

        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public bool IsCheckout { get; set; }
        public string   OrderNumber { get; set; }
    }
}
