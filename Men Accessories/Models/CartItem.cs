using Men_Accessories.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Men_Accessories.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Cart ID is required")]
        public int CartId { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, 10000, ErrorMessage = "Quantity must be at least 1")]
        [MaxStockQuantity]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 999999.99, ErrorMessage = "Unit price must be between 0.01 and 999999.99")]
        [ValidCartItemPrice]
        public decimal UnitPrice { get; set; }

        // Navigation
        [ForeignKey("CartId")]
        public virtual Cart? Cart { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
