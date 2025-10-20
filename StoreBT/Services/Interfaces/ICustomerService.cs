using StoreBT.Models;

namespace StoreBT.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<int> AddAsync(Customer Customer);
        Task<int> UpdateAsync(Customer Customer);
        Task<int> DeleteAsync(Customer Customer);
    }
}
