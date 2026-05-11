using System.ComponentModel.DataAnnotations;

namespace Men_Accessories.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 150 characters")]
        public string Name { get; set; }

        // Navigation
        public virtual ICollection<Product>? Products { get; set; } = new List<Product>();
    }
}
