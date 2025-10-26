using StoreBT.Models;

namespace StoreBT.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> SearchAsync(string value);
        Task<int> AddAsync(Order product);
        Task<int> UpdateAsync(Order product);
        Task<int> DeleteAsync(Order product);
        Task<Order?> GetByIdAsync(Guid id);
    }
}
