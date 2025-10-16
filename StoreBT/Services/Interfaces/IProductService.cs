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
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByBarcodeAsync(string barcode);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(string barcode);
    }
}
