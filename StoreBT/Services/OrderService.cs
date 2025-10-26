using StoreBT.Models;
using StoreBT.Repositories;
using StoreBT.Repositories.Interfaces;
using StoreBT.Services.Interfaces;

namespace StoreBT.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IEnumerable<Order>> SearchAsync(string value)
        {
            return await _orderRepository.FindAllAsync(x => !x.IsDeleted && x.OrderCode.Contains(value), x => x.OrderByDescending(x => x.CreatedAt));
        }

        public async Task<int> AddAsync(Order order)
        {
            await _orderRepository.AddAsync(order);
            return await _orderRepository.SaveChangeAsync();
        }

        public async Task<int> UpdateAsync(Order order)
        {
            _orderRepository.Update(order);
            return await _orderRepository.SaveChangeAsync();
        }

        public async Task<int> DeleteAsync(Order order)
        {
            order.IsDeleted = true;
            _orderRepository.Update(order);
            return await _orderRepository.SaveChangeAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _orderRepository.FindByIdAsync(id);
        }
    }
}
