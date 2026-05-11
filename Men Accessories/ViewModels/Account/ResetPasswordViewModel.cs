using System.ComponentModel.DataAnnotations;

namespace Men_Accessories.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
       

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
