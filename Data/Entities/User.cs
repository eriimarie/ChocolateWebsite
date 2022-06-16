using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Data.Entities
{
    public class User : BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        [Required]
        public string Email { get; set; }

        [MaxLength(100)]
        [Required]
        public string Password { get; set; }

        [MaxLength(100)]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime LastLogin { get; set; }

        [MaxLength(255)]
        public string ForgotPasswordKey { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int RoleId { get; set; }

        //Navigation Properties
        public virtual Role Role { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<Address> Addresses { get; set; }
        public virtual List<CartItem> CartItems { get; set; }
    }
}
