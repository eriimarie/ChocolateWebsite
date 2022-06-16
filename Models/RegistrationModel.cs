using System.ComponentModel.DataAnnotations;

namespace ChocoShop.Models
{
    public class RegistrationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password and compare password should match.")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and compare password should match.")]
        public string ConfirmPassword { get; set; }
    }
}
