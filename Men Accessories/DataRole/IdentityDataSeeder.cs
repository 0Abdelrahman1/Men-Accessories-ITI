using Men_Accessories.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Men_Accessories.DataRole
{
    public class IdentityDataSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentityDataSeeder(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            // Create Admin role 
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Get admin credentials from config
            var adminEmail = _configuration["AdminUser:Email"];
            var adminUserName = _configuration["AdminUser:UserName"];
            var adminFirstName = _configuration["AdminUser:FirstName"];
            var adminLastName = _configuration["AdminUser:LastName"];
            var adminPassword = _configuration["AdminUser:Password"];

            var user = await _userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    firstName = adminFirstName,
                    lastName = adminLastName,
                    Email = adminEmail,
                    UserName = adminUserName
                };

                await _userManager.CreateAsync(user, adminPassword);
            }

            // Assign Admin role
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
