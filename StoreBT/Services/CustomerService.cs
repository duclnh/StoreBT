using StoreBT.Models;
using StoreBT.Services.Interfaces;

namespace StoreBT.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly List<Customer> _customers = new();

        public CustomerService()
        {
            _customers.AddRange(new List<Customer>
            {
                    new Customer
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Khách hàng 1",
                        Phone = "0901000001",
                        Address = "Số 1 Đường ABC, Quận 1, TP. Hồ Chí Minh",
                        CreatedAt = DateTime.Now.AddDays(-3)
                    },
                    new Customer
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Khách hàng 2",
                        Phone = "0902000002",
                        Address = "Số 2 Đường ABC, Quận 2, TP. Hồ Chí Minh",
                        CreatedAt = DateTime.Now.AddDays(-2)
                    },
                    new Customer
                    {
                        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Name = "Khách hàng 3",
                        Phone = "0903000003",
                        Address = "Số 3 Đường ABC, Quận 3, TP. Hồ Chí Minh",
                        CreatedAt = DateTime.Now.AddDays(-1)
                    }
});
        }

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Customer>>(_customers);
        }

        public Task AddAsync(Customer customer)
        {
            customer.Id = Guid.NewGuid();
            customer.CreatedAt = DateTime.Now;
            _customers.Add(customer);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Customer customer)
        {
            var existing = _customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existing != null)
            {
                existing.Name = customer.Name;
                existing.Phone = customer.Phone;
                existing.Address = customer.Address;
                existing.UpdatedAt = DateTime.Now;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var existing = _customers.FirstOrDefault(c => c.Id == id);
            if (existing != null)
            {
                _customers.Remove(existing);
            }
            return Task.CompletedTask;
        }
    }

}
