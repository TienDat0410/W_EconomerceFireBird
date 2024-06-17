using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSiteBanHang.Interfaces;

namespace WebSiteBanHang.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(ICartRepository cartRepository, UserManager<IdentityUser> userManager)
        {
            _cartRepository = cartRepository;
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
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            await _cartRepository.AddToCartAsync(userId, productId, quantity);
            TempData["Success"] = "Product added to cart successfully!";
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            await _cartRepository.RemoveFromCartAsync(userId, productId);
            TempData["Success"] = "Product removed from cart successfully!";
            return RedirectToAction("Index");
        }
    }
}
