using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Men_Accessories.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool isAgree { get; set; }
    }
}
