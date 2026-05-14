using Men_Accessories.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Men_Accessories.Contexts
{
    public class MenAccessoriesContext : IdentityDbContext<ApplicationUser>
    {
        public MenAccessoriesContext(DbContextOptions<MenAccessoriesContext> options) : base(options)
        {}

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
            base.OnModelCreating(modelBuilder);

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
                // Prevent duplicate category names
                entity.HasIndex(e => e.Name)
                    .IsUnique();
                // Prevent category deletion if products exist
                entity.HasMany(e => e.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(10000);
                entity.Property(e => e.ImageUrl)
                    .IsRequired();
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                entity.Property(e => e.StockQuantity)
                    .IsRequired();
                // Prevent product deletion if it's in active carts or orders
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
                entity.Property(e => e.Gender)
                    .IsRequired();
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(300);
                // Prevent customer deletion if they have orders (order history matters)
                entity.HasOne(e => e.Cart)
                    .WithOne(c => c.Customer)
                    .HasForeignKey<Cart>(c => c.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Orders)
                    .WithOne(o => o.Customer)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Cart configuration
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Id);
                // One cart per customer
                entity.HasIndex(e => e.CustomerId)
                    .IsUnique();
                entity.HasOne(e => e.Customer)
                    .WithOne(c => c.Cart)
                    .HasForeignKey<Cart>(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.CartItems)
                    .WithOne(ci => ci.Cart)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // CartItem configuration
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity)
                    .IsRequired();
                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                // Prevent duplicate items in same cart
                entity.HasIndex(e => new { e.CartId, e.ProductId })
                    .IsUnique();
                entity.HasOne(e => e.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(e => e.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
                // Prevent product deletion if it's in active carts
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                // Prevent customer deletion if they have order history
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity)
                    .IsRequired();
                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18, 2)")
                    .IsRequired();
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                // Prevent product deletion if it's referenced in order history
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            
        }
    }
}
