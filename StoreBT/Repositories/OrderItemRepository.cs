using StoreBT.Models;
using StoreBT.Repositories.Interfaces;

namespace StoreBT.Repositories
{
    public class OrderItemRepository : RepositoryBase<OrderItem, Guid>, IOrderItemRepository
    {
        public OrderItemRepository(StoreDbContext storeDbContext) : base(storeDbContext)
        {
        }
    }

}
