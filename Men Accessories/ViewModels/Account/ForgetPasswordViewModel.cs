using System.ComponentModel.DataAnnotations;

namespace Men_Accessories.ViewModels.Account
{
    public class ForgetPasswordViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please Enter your Email ")]
        public string Email { get; set; }

    }
}
