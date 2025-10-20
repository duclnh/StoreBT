using StoreBT.Models;
using StoreBT.Repositories.Interfaces;

namespace StoreBT.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer, Guid>, ICustomerRepository
    {
        public CustomerRepository(StoreDbContext storeDbContext) : base(storeDbContext)
        {
        }
    }

}
