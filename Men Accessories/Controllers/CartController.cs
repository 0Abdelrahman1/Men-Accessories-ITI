using Microsoft.AspNetCore.Mvc;
using Men_Accessories.Services;
using Men_Accessories.ViewModels.CartRelated;

namespace Men_Accessories.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        public IActionResult Index()
        {
            int customerId = GetCustomerId();

            CartModelView? cart =
                _cartService.getCartByCustomerId(customerId);

            return View(cart);
        }


        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            int customerId = GetCustomerId();

            _cartService.addToCart(customerId, productId, quantity);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(CartModelView cart)
        {
            if (ModelState.IsValid)
            {
                _cartService.updateCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            int customerId = GetCustomerId();

            _cartService.clearCart(customerId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            int customerId = GetCustomerId();

            _cartService.Checkout(customerId);

            return RedirectToAction("Index", "Order");
        }

        private int GetCustomerId()
        {
            return int.Parse(User.FindFirst("CustomerId")!.Value);
        }
    }
}