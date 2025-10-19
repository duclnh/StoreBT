using StoreBT.Models;

namespace StoreBT.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);
        Task AddAsync(OrderItem item);
        Task UpdateAsync(OrderItem item);
        Task DeleteAsync(Guid id);
    }
}
