using StoreBT.Models;
using StoreBT.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBT.Repositories
{
    public class ProductRepository : RepositoryBase<Product, Guid>, IProductRepository
    {
        public ProductRepository(StoreDbContext storeDbContext) : base(storeDbContext)
        {
        }
    }
}
