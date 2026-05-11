using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Men_Accessories.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Please Enter First Name")]
        [MaxLength(10)]
        public string firstName { get; set; }

        [Required(ErrorMessage ="Please Enter Last Name")]
        [MaxLength(10)]
        public string lastName { get; set; }

        [MaxLength(12,ErrorMessage ="Please Enter Username not exceed 12 characters ")]
        public string userName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public bool isAgree { get; set; }














    }
}
