using ChocoShop.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Models
{
    public class AddressModel
    {
        public int Id { get; set; }

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
    }
}
