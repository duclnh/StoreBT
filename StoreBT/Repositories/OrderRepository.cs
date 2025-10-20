using StoreBT.Models;
using StoreBT.Repositories.Interfaces;

namespace StoreBT.Repositories
{
    public class OrderRepository : RepositoryBase<Order, Guid>, IOrderRepository
    {
        public OrderRepository(StoreDbContext storeDbContext) : base(storeDbContext)
        {
        }
    }

}
