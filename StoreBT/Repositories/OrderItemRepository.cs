using Microsoft.EntityFrameworkCore;
using StoreBT.Models;
using StoreBT.Repositories.Interfaces;

namespace StoreBT.Repositories
{
    public class OrderItemRepository : RepositoryBase<OrderItem, Guid>, IOrderItemRepository
    {
        private readonly StoreDbContext _dbContext;
        public OrderItemRepository(StoreDbContext storeDbContext) : base(storeDbContext)
        {
            _dbContext = storeDbContext; 
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync(Guid orderId)
        {
            return await _dbContext.OrderItems.Where(x => !x.IsDeleted && x.OrderId == orderId).AsNoTracking().ToListAsync();
        }
    }

}
