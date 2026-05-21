using Men_Accessories.Contexts;
using Men_Accessories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Men_Accessories.Data
{
    public static class DbSeeder
    {
        public static void Seed(MenAccessoriesContext context)
        {

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                new Category { Name = "Watches" },
                new Category { Name = "Belts" },
                new Category { Name = "Wallets" },
                new Category { Name = "Sunglasses" },
                new Category { Name = "Necklaces" },
                new Category { Name = "Ties" },
                new Category { Name = "Cufflinks" },
                new Category { Name = "Hats" },
                new Category { Name = "Gloves" },
                new Category { Name = "Keychains" }
                 };
                context.Categories.AddRange(categories);
                context.SaveChanges();



                var random = new Random();
                var products = new List<Product>();

                string[] adjectives = { "Classic", "Premium", "Luxury", "Modern", "Vintage", "Sport", "Elegant", "Rugged", "Slim", "Heavy-Duty" };
                string[] materials = { "Leather", "Stainless Steel", "Carbon Fiber", "Titanium", "Wood", "Nylon", "Cotton", "Wool", "Brass", "Silver" };

                foreach (var category in categories)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        string adj = adjectives[random.Next(adjectives.Length)];
                        string mat = materials[random.Next(materials.Length)];
                        string productName = $"{adj} {mat} {category.Name}";
                        if (category.Name == "Watches") productName = $"{adj} {mat} Watch";
                        else if (category.Name == "Hats") productName = $"{adj} {mat} Hat";
                        else if (category.Name == "Gloves") productName = $"{adj} {mat} Gloves";
                        else if (category.Name == "Keychains") productName = $"{adj} {mat} Keychain";

                        decimal price = Math.Round((decimal)(random.NextDouble() * 150 + 20), 2);
                        int stock = random.Next(5, 150);
                        int discount = random.Next(0, 100) > 80 ? random.Next(10, 40) : 0;

                        var product = new Product
                        {
                            Name = productName,
                            Description = $"High-quality {category.Name.ToLower()} for men. Made from premium {mat.ToLower()}. Perfect for everyday use or special occasions.",
                            ImageUrl = "",
                            Price = price,
                            StockQuantity = stock,
                            Discount = discount,
                            IsFeatured = random.Next(0, 100) > 90,
                            CategoryId = category.Id,
                            CreatedAt = DateTime.Now.AddDays(-random.Next(1, 90)),
                            ProductImages = new List<ProductImage>()
                        };
                        products.Add(product);
                    }
                }

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}