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
            customer.IsDeleted = true;
            _customerRepository.Update(customer);
            return await _customerRepository.SaveChangeAsync();
        }

        public async Task<IEnumerable<Customer>> SearchAsync(string value)
        {
            return await _customerRepository.FindAllAsync(x => !x.IsDeleted && x.Name.Contains(value) || x.Phone.Contains(value), 
                x => x.OrderByDescending(x => x.CreatedAt));
        }
    }

}
