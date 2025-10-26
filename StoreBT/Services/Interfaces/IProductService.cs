using StoreBT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBT.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> SearchAsync(string value);
        Task<int> AddAsync(Product product);
        Task<int> UpdateAsync(Product product);
        Task<int> DeleteAsync(Product product);
        Task<Product?> GetProductByBarCode(string barcode);
    }
}
