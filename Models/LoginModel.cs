using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChocoShop.Models
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings =false, ErrorMessage = "Username or Email required !")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required !")]
        public string Password { get; set; }
    }
}
