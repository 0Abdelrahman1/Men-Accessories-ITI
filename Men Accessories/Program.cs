using Men_Accessories.Contexts;
using Microsoft.EntityFrameworkCore;
using Men_Accessories.Models;
using Microsoft.AspNetCore.Identity;

namespace Men_Accessories
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MenAccessoriesContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CS"))
                    .UseLazyLoadingProxies());

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Optional: configure password, lockout, etc.
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
            })
           .AddEntityFrameworkStores<MenAccessoriesContext>()  // links Identity to your DbContext
           .AddDefaultTokenProviders();


            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "349658992076-g4j356se62drpi0baims7r50752vcm6j.apps.googleusercontent.com";

                options.ClientSecret = "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";
            });

            builder.Services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "1342213184632804";
                options.AppSecret = "f715c8b3460683ff044781b12494fb5f";
            });


            builder.Configuration.AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.local.json", optional: true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            

            app.UseAuthentication();
            app.UseAuthorization();
           
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
