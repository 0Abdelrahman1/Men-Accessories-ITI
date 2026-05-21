using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Men_Accessories.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Image URL must be a valid URL")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }

        // Navigation property
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}