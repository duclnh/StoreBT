using StoreBT.Models;

namespace StoreBT.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);
        Task AddRangeAsync(List<OrderItem> items);
        Task<bool> AnyAsync(Guid productId);
    }
}
