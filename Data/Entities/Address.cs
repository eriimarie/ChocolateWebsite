using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Data.Entities
{
    public class Address : BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string AddressLIne1 { get; set; }

        [MaxLength(100)]
        [Required]
        public string Street { get; set; }

        [MaxLength(100)]
        [Required]
        public string City { get; set; }

        [MaxLength(100)]
        [Required]
        public string State { get; set; }

        [MaxLength(6)]
        [Required]
        public string ZipCode { get; set; }

        public AddressType AddressType { get; set; }

        // Navigation Property
        public virtual User User { get; set; }
    }
}
