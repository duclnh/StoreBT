using StoreBT.Models;

namespace StoreBT.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order Customer);
        Task UpdateAsync(Order Customer);
        Task DeleteAsync(Guid Id);
    }
}
