using Microsoft.AspNetCore.Identity;

namespace WebSiteBanHang.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
