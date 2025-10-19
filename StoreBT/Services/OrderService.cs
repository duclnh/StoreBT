using StoreBT.Models;
using StoreBT.Services.Interfaces;

namespace StoreBT.Services
{
    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new List<Order>();

        public OrderService()
        {
            _orders.AddRange(new List<Order>
            {
                new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    CustomerName = "Nguyễn Văn A",
                    CustomerPhone = "0901234567",
                    OrderDate = DateTime.Now.AddDays(-3),
                    TotalAmount = 2500000,
                    Discount = 100000,
                    Notes = "Giao hàng buổi sáng"
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    CustomerName = "Trần Thị B",
                    CustomerPhone = "0902345678",
                    OrderDate = DateTime.Now.AddDays(-2),
                    TotalAmount = 1200000,
                    Discount = 0,
                    Notes = "Thanh toán online"
                },
                new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    CustomerName = "Lê Văn C",
                    CustomerPhone = "0903456789",
                    OrderDate = DateTime.Now.AddDays(-1),
                    TotalAmount = 3500000,
                    Discount = 150000,
                    Notes = "Ưu tiên khách VIP"
                }
            });

        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await Task.FromResult(_orders);
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            return await Task.FromResult(order);
        }

        public async Task AddAsync(Order order)
        {
            _orders.Add(order);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Order order)
        {
            var existing = _orders.FirstOrDefault(o => o.Id == order.Id);
            if (existing != null)
            {
                existing.CustomerId = order.CustomerId;
                existing.OrderDate = order.OrderDate;
                existing.TotalAmount = order.TotalAmount;
                existing.UpdatedAt = DateTime.Now;
                existing.Notes = order.Notes;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
                _orders.Remove(order);

            await Task.CompletedTask;
        }
    }


}
