using StoreBT.Models;

namespace StoreBT.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task AddAsync(Customer Customer);
        Task UpdateAsync(Customer Customer);
        Task DeleteAsync(Guid Id);
    }
}
