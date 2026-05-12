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
