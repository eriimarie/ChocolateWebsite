using ChocoShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Data.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Tags { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Discount { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        // Navigation Properties
        public virtual List<Order> Orders { get; set; }
        public virtual List<CartItem> CartItems { get; set; }
    }
}
