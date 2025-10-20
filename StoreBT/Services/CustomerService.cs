using StoreBT.Models;
using StoreBT.Repositories;
using StoreBT.Repositories.Interfaces;
using StoreBT.Services.Interfaces;

namespace StoreBT.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAll(null, CancellationToken.None);
        }

        public async Task<int> AddAsync(Customer customer)
        {
            customer.Id = Guid.NewGuid();
            customer.CreatedAt = DateTime.Now;
            await _customerRepository.AddAsync(customer);
            return await _customerRepository.SaveChangeAsync();
        }

        public async Task<int> UpdateAsync(Customer customer)
        {
            _customerRepository.Update(customer);
            return await _customerRepository.SaveChangeAsync();
        }

        public async Task<int> DeleteAsync(Customer customer)
        {
            _customerRepository.Remove(customer);
            return await _customerRepository.SaveChangeAsync();
        }
    }

}
