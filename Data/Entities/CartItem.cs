using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Data.Entities
{
    public class CartItem : BaseEntity
    {

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public virtual Product Item { get; set; }

        [Required]
        public virtual User User { get; set; }

    }
}
