using Men_Accessories.Models;

namespace Men_Accessories.ViewModels
{
    public class ProductPaginationViewModel
    {
        public List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
