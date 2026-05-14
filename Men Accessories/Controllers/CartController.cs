using Men_Accessories.Contexts;
using Men_Accessories.Models;
using Men_Accessories.Services;
using Men_Accessories.ViewModels.CartRelated;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Stripe.Checkout;
namespace Men_Accessories.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly MenAccessoriesContext _context;

        public CartController(ICartService cartService, MenAccessoriesContext context)
        {
            _cartService = cartService;
            _context = context;
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
        public IActionResult AddToCartAjax(int productId, int quantity)
        {
            try
            {
                int customerId = GetCustomerId();

                _cartService.addToCart(customerId, productId, quantity);

                return Json(new { success = true, message = "Product successfully added to your cart!" });
            }
            catch
            {
                return Json(new { success = false, message = "Something went wrong. Please try again." });
            }
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
            var cart = _cartService.getCartByCustomerId(customerId);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index");
            }


            var domain = "http://localhost:5053";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = domain + "/Order/Success?sessionId={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/Cart/Index", 
                LineItems = new List<SessionLineItemOptions>()
            };

            foreach (var item in cart.CartItems)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.UnitPrice * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Product ID #" + item.ProductId
                        },
                    },
                    Quantity = item.Quantity,
                });
            }

            var service = new SessionService();
            Session session = service.Create(options);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        private int GetCustomerId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue(ClaimTypes.Email) ?? "New Customer";

            var customer = _context.Customers.FirstOrDefault(c => c.ApplicationUserId == userId);

            if (customer == null)
            {
                customer = new Customer
                {
                    ApplicationUserId = userId,
                    Name = userName,
                    Address = "Address pending checkout", 
                    Phone = "01000000000",                
                    Gender = Men_Accessories.Enums.Gender.Male 
                };

                _context.Customers.Add(customer);
                _context.SaveChanges(); 
            }

            return customer.Id;
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            int customerId = GetCustomerId();
            _cartService.RemoveFromCart(customerId, productId);

            return RedirectToAction("Index");
        }
    }
}