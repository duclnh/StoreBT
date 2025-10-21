using StoreBT.Models;

namespace StoreBT.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<int> AddAsync(Customer Customer);
        Task<int> UpdateAsync(Customer Customer);
        Task<int> DeleteAsync(Customer Customer);
        Task<IEnumerable<Customer>> SearchAsync(string value);
    }
}
