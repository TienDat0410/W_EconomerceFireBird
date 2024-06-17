using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebSiteBanHang.Interfaces;
using Microsoft.AspNetCore.Identity;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class CheckoutController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CheckoutController(ICartRepository cartRepository, IOrderRepository orderRepository, UserManager<IdentityUser> userManager)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            return View(cart);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderItems = cart.CartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                }).ToList()
            };

            await _orderRepository.CreateOrderAsync(order);

            // Clear the cart after placing the order
            foreach (var item in cart.CartItems.ToList())
            {
                await _cartRepository.RemoveFromCartAsync(userId, item.ProductId);
            }
            TempData["Success"] = "Your order has been placed successfully!";
            return RedirectToAction("OrderConfirmation");
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}