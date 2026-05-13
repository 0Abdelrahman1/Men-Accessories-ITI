using Men_Accessories.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Men_Accessories.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer ID is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Creation date is required")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Total amount is required")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 999999.99, ErrorMessage = "Total amount must be between 0.01 and 999999.99")]
        [ValidOrderTotal]
        public decimal TotalAmount { get; set; }

        public string PaymentStatus { get; set; } = "Pending"; 
        public string OrderStatus { get; set; } = "Processing"; 
        public string? StripeSessionId { get; set; } 
        // Navigation
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
    }
}
