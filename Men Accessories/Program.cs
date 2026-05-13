using Men_Accessories.Contexts;
using Men_Accessories.DataRole;
using Men_Accessories.Models;
using Microsoft.AspNetCore.Identity;
using Men_Accessories.Repositories;
using Men_Accessories.Services;
using Men_Accessories.Data;
using Microsoft.EntityFrameworkCore;

namespace Men_Accessories
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
          options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
          options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
           })
           .AddFacebook(options =>
      {
          options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
          options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
          });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();


            builder.Configuration.AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.local.json", optional: true);

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var seeder = new IdentityDataSeeder(
                    services.GetRequiredService<UserManager<ApplicationUser>>(),
                    services.GetRequiredService<RoleManager<IdentityRole>>()
                );

                await seeder.SeedAsync();
            }

            // Seed database
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MenAccessoriesContext>();
                DbSeeder.Seed(context);
            }

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
