using Men_Accessories.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Men_Accessories.ViewModels.CartRelated
{
    public class CartModelView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer ID is required")]
        public int CustomerId { get; set; }

        // Navigation
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();


        public Cart ToCart()
        {
            return new Cart
            {
                Id = this.Id,
                CustomerId = this.CustomerId,
                Customer = this.Customer,
                CartItems = this.CartItems
            };
        }
    }
}
