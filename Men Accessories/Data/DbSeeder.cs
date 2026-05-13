using Men_Accessories.Contexts;
using Men_Accessories.Models;

namespace Men_Accessories.Data
{
    public static class DbSeeder
    {
        public static void Seed(MenAccessoriesContext context)
        {
            // Check if categories exist
            if (!context.Categories.Any())
            {
                var newCategories = new List<Category>
                {
                    new Category { Name = "Watches" },
                    new Category { Name = "Belts" },
                    new Category { Name = "Wallets" },
                    new Category { Name = "Sunglasses" },
                    new Category { Name = "Bracelets" },
                    new Category { Name = "Rings" },
                    new Category { Name = "Necklaces" },
                    new Category { Name = "Ties" }
                };

                context.Categories.AddRange(newCategories);
                context.SaveChanges();
            }

            // Get categories from database (always fetch after potential creation)
            var categories = context.Categories.OrderBy(c => c.Id).ToList();

            // Check if products exist
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    // Watches
                    new Product
                    {
                        Name = "Classic Leather Watch",
                        Description = "Elegant leather strap watch with analog display. Perfect for formal occasions.",
                        ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400",
                        Price = 149.99m,
                        StockQuantity = 50,
                        Discount = 10,
                        CategoryId = categories[0].Id,
                        CreatedAt = DateTime.Now.AddDays(-30)
                    },
                    new Product
                    {
                        Name = "Sport Digital Watch",
                        Description = "Water-resistant digital watch with stopwatch and alarm features.",
                        ImageUrl = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=400",
                        Price = 89.99m,
                        StockQuantity = 75,
                        Discount = 0,
                        CategoryId = categories[0].Id,
                        CreatedAt = DateTime.Now.AddDays(-25)
                    },
                    new Product
                    {
                        Name = "Luxury Gold Watch",
                        Description = "Premium gold-plated watch with Swiss movement.",
                        ImageUrl = "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=400",
                        Price = 499.99m,
                        StockQuantity = 20,
                        Discount = 15,
                        CategoryId = categories[0].Id,
                        CreatedAt = DateTime.Now.AddDays(-20)
                    },
                    new Product
                    {
                        Name = "Minimalist Silver Watch",
                        Description = "Clean and simple design with silver mesh bracelet.",
                        ImageUrl = "https://images.unsplash.com/photo-1533139502658-0198f920d8e8?w=400",
                        Price = 199.99m,
                        StockQuantity = 40,
                        Discount = 5,
                        CategoryId = categories[0].Id,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    },

                    // Belts
                    new Product
                    {
                        Name = "Genuine Leather Belt",
                        Description = "High-quality leather belt with classic buckle.",
                        ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=400",
                        Price = 59.99m,
                        StockQuantity = 100,
                        Discount = 0,
                        CategoryId = categories[1].Id,
                        CreatedAt = DateTime.Now.AddDays(-28)
                    },
                    new Product
                    {
                        Name = "Canvas Casual Belt",
                        Description = "Durable canvas belt for casual wear.",
                        ImageUrl = "https://images.unsplash.com/photo-1624222247344-550fb60583dc?w=400",
                        Price = 34.99m,
                        StockQuantity = 80,
                        Discount = 20,
                        CategoryId = categories[1].Id,
                        CreatedAt = DateTime.Now.AddDays(-22)
                    },
                    new Product
                    {
                        Name = "Dress Belt Black",
                        Description = "Formal black leather belt with silver buckle.",
                        ImageUrl = "https://images.unsplash.com/photo-1584917865442-de89df76afd3?w=400",
                        Price = 79.99m,
                        StockQuantity = 60,
                        Discount = 0,
                        CategoryId = categories[1].Id,
                        CreatedAt = DateTime.Now.AddDays(-18)
                    },

                    // Wallets
                    new Product
                    {
                        Name = "Bifold Leather Wallet",
                        Description = "Classic bifold wallet with multiple card slots.",
                        ImageUrl = "https://images.unsplash.com/photo-1627123424574-724758594e93?w=400",
                        Price = 49.99m,
                        StockQuantity = 90,
                        Discount = 10,
                        CategoryId = categories[2].Id,
                        CreatedAt = DateTime.Now.AddDays(-26)
                    },
                    new Product
                    {
                        Name = "Slim Card Holder",
                        Description = "Ultra-slim card holder for minimalists.",
                        ImageUrl = "https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a?w=400",
                        Price = 29.99m,
                        StockQuantity = 120,
                        Discount = 0,
                        CategoryId = categories[2].Id,
                        CreatedAt = DateTime.Now.AddDays(-21)
                    },
                    new Product
                    {
                        Name = "Trifold Wallet",
                        Description = "Spacious trifold wallet with coin compartment.",
                        ImageUrl = "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=400",
                        Price = 39.99m,
                        StockQuantity = 70,
                        Discount = 15,
                        CategoryId = categories[2].Id,
                        CreatedAt = DateTime.Now.AddDays(-16)
                    },

                    // Sunglasses
                    new Product
                    {
                        Name = "Aviator Sunglasses",
                        Description = "Classic aviator style with UV protection.",
                        ImageUrl = "https://images.unsplash.com/photo-1572635196237-14b3f281503f?w=400",
                        Price = 129.99m,
                        StockQuantity = 45,
                        Discount = 0,
                        CategoryId = categories[3].Id,
                        CreatedAt = DateTime.Now.AddDays(-24)
                    },
                    new Product
                    {
                        Name = "Wayfarer Sunglasses",
                        Description = "Iconic wayfarer design with polarized lenses.",
                        ImageUrl = "https://images.unsplash.com/photo-1511499767150-a48a237f0083?w=400",
                        Price = 149.99m,
                        StockQuantity = 55,
                        Discount = 10,
                        CategoryId = categories[3].Id,
                        CreatedAt = DateTime.Now.AddDays(-19)
                    },
                    new Product
                    {
                        Name = "Sport Sunglasses",
                        Description = "Lightweight sport sunglasses with wraparound design.",
                        ImageUrl = "https://images.unsplash.com/photo-1577803645773-f96470509666?w=400",
                        Price = 99.99m,
                        StockQuantity = 65,
                        Discount = 5,
                        CategoryId = categories[3].Id,
                        CreatedAt = DateTime.Now.AddDays(-14)
                    },

                    // Bracelets
                    new Product
                    {
                        Name = "Leather Bracelet",
                        Description = "Stylish leather bracelet with metal clasp.",
                        ImageUrl = "https://images.unsplash.com/photo-1611591437281-460bfbe1220a?w=400",
                        Price = 24.99m,
                        StockQuantity = 150,
                        Discount = 0,
                        CategoryId = categories[4].Id,
                        CreatedAt = DateTime.Now.AddDays(-23)
                    },
                    new Product
                    {
                        Name = "Silver Chain Bracelet",
                        Description = "Elegant silver chain bracelet.",
                        ImageUrl = "https://images.unsplash.com/photo-1573408301185-9146fe634ad0?w=400",
                        Price = 69.99m,
                        StockQuantity = 80,
                        Discount = 20,
                        CategoryId = categories[4].Id,
                        CreatedAt = DateTime.Now.AddDays(-17)
                    },
                    new Product
                    {
                        Name = "Beaded Bracelet Set",
                        Description = "Set of 3 beaded bracelets in different colors.",
                        ImageUrl = "https://images.unsplash.com/photo-1602751584552-8ba73aad10e1?w=400",
                        Price = 19.99m,
                        StockQuantity = 200,
                        Discount = 0,
                        CategoryId = categories[4].Id,
                        CreatedAt = DateTime.Now.AddDays(-12)
                    },

                    // Rings
                    new Product
                    {
                        Name = "Signet Ring",
                        Description = "Classic signet ring with polished finish.",
                        ImageUrl = "https://images.unsplash.com/photo-1605100804763-247f67b3557e?w=400",
                        Price = 89.99m,
                        StockQuantity = 60,
                        Discount = 10,
                        CategoryId = categories[5].Id,
                        CreatedAt = DateTime.Now.AddDays(-20)
                    },
                    new Product
                    {
                        Name = "Silver Band Ring",
                        Description = "Simple silver band ring for everyday wear.",
                        ImageUrl = "https://images.unsplash.com/photo-1603561591411-07134e71a2a9?w=400",
                        Price = 49.99m,
                        StockQuantity = 100,
                        Discount = 0,
                        CategoryId = categories[5].Id,
                        CreatedAt = DateTime.Now.AddDays(-15)
                    },

                    // Necklaces
                    new Product
                    {
                        Name = "Silver Chain Necklace",
                        Description = "Elegant silver chain necklace.",
                        ImageUrl = "https://images.unsplash.com/photo-1599643478518-a784e5dc4c8f?w=400",
                        Price = 79.99m,
                        StockQuantity = 70,
                        Discount = 15,
                        CategoryId = categories[6].Id,
                        CreatedAt = DateTime.Now.AddDays(-18)
                    },
                    new Product
                    {
                        Name = "Leather Cord Necklace",
                        Description = "Casual leather cord necklace with pendant.",
                        ImageUrl = "https://images.unsplash.com/photo-1611085583191-a3b181a88401?w=400",
                        Price = 29.99m,
                        StockQuantity = 130,
                        Discount = 0,
                        CategoryId = categories[6].Id,
                        CreatedAt = DateTime.Now.AddDays(-13)
                    },

                    // Ties
                    new Product
                    {
                        Name = "Silk Tie Navy",
                        Description = "Premium silk tie in navy blue.",
                        ImageUrl = "https://images.unsplash.com/photo-1589756823695-278bc923f962?w=400",
                        Price = 59.99m,
                        StockQuantity = 85,
                        Discount = 0,
                        CategoryId = categories[7].Id,
                        CreatedAt = DateTime.Now.AddDays(-16)
                    },
                    new Product
                    {
                        Name = "Striped Tie",
                        Description = "Classic striped tie for business wear.",
                        ImageUrl = "https://images.unsplash.com/photo-1589756823695-278bc923f962?w=400",
                        Price = 49.99m,
                        StockQuantity = 95,
                        Discount = 10,
                        CategoryId = categories[7].Id,
                        CreatedAt = DateTime.Now.AddDays(-11)
                    }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
