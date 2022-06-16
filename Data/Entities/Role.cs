using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Data.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        //Navigation Properties
        public virtual List<User> Users { get; set; }
    }
}
