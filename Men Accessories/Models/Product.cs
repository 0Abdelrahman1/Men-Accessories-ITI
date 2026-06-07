using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Men_Accessories.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 150 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required")]
        [StringLength(10000, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 10000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, 10000, ErrorMessage = "Stock quantity must be between 0 and 10000")]
        public int StockQuantity { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100")]
        public decimal Discount { get; set; }

        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        // Navigation
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public int TotalRating { get; set; } = 0;
        public double AverageRating { get { return TotalRating > 0 ? (double)TotalRating / Rates.Count : 0.0; } }

        public virtual List<Rate> Rates { get; set; } = new List<Rate>();

        public virtual List<ProductImage> ProductImages { get; set; } = new();
    }
}
