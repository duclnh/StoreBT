using StoreBT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBT.Repositories.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product, Guid>
    {
    }
}
