using StoreBT.Models;
using StoreBT.Services.Interfaces;

namespace StoreBT.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly List<OrderItem> _items = new List<OrderItem>();

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await Task.FromResult(_items);
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
        {
            var items = _items.Where(i => i.OrderId == orderId);
            return await Task.FromResult(items);
        }

        public async Task AddAsync(OrderItem item)
        {
            _items.Add(item);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(OrderItem item)
        {
            var existing = _items.FirstOrDefault(i => i.Id == item.Id);
            if (existing != null)
            {
                existing.ProductId = item.ProductId;
                existing.Quantity = item.Quantity;
                existing.UnitPrice = item.UnitPrice;
                existing.UpdatedAt = DateTime.Now;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item != null)
                _items.Remove(item);

            await Task.CompletedTask;
        }
    }
}
