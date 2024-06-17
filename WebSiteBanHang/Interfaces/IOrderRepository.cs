using WebSiteBanHang.Models;

public interface IOrderRepository
{
    Task CreateOrderAsync(Order order);
}