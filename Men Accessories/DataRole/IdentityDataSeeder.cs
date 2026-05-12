using Men_Accessories.Models;
using Microsoft.AspNetCore.Identity;

namespace Men_Accessories.DataRole
{
    public class IdentityDataSeeder
    {
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public IdentityDataSeeder(UserManager<ApplicationUser> userManager,
                                  RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Create Admin role 
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            
            var adminEmail = "dinaalaraby9503@gmail.com";
            var user = await _userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    firstName="dina",
                    lastName="elaraby",
                    Email = adminEmail,
                    UserName = "admin"
                };

                await _userManager.CreateAsync(user, "Admin@123");
            }

            // Assign Admin role
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
