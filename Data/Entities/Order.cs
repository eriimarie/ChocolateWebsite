using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChocoShop.Data.Entities
{
    public class Order : BaseEntity
    {
        [Required]
        public string ReferenceNumber { get; set; }


        [Required]
        public int Discount { get; internal set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public decimal SubTotal { get; set; }
        [Required]
        public decimal Tax { get; set; }
        [Required]
        public decimal Shipping { get; set; }

        public decimal GrandTotal { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        // Navigation Property
        public virtual User CreatedBy { get; set; }

        [Required]
        public virtual List<OrderItem> Items { get; set; }

        [Required]
        public virtual Address BillingAddress { get; set; }

        [Required]
        public virtual Address ShippingAddress { get; set; }
    }
}
