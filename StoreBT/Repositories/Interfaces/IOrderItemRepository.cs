using StoreBT.Models;

namespace StoreBT.Repositories.Interfaces
{
    public interface IOrderItemRepository : IRepositoryBase<OrderItem, Guid>
    {
        public Task<IEnumerable<OrderItem>> GetAllAsync(Guid orderId);
    }
}
