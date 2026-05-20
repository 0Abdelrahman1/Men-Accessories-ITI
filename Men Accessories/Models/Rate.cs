using System.ComponentModel.DataAnnotations;

namespace Men_Accessories.Models
{
    public class Rate
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required]
        public string Email { get; set; }
        [Range(1, 5, ErrorMessage = "Stars must be between 1 and 5.")]
        public int Stars { get; set; }
        public string? Comment { get; set; }
    }
}
