using StoreBT.Models;
using StoreBT.Repositories.Interfaces;
using StoreBT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBT.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> SearchAsync(string value)
        {
            return await _productRepository.FindAllAsync(x => !x.IsDeleted && (x.Barcode.Contains(value) || x.Name.Contains(value)
                                                        || x.Category.Contains(value)), x => x.OrderByDescending(x => x.CreatedAt));
        }
        public async Task<int> AddAsync(Product product)
        {
            await _productRepository.AddAsync(product);

            return await _productRepository.SaveChangeAsync();
        }

        public async Task<int> UpdateAsync(Product product)
        {
            _productRepository.Update(product);

            return await _productRepository.SaveChangeAsync();
        }

        public async Task<int> DeleteAsync(Product product)
        {
            product.IsDeleted = true;
            _productRepository.Update(product);

            return await _productRepository.SaveChangeAsync();
        }

        public async Task<Product?> GetProductByBarCode(string barcode)
        {
            return await _productRepository.FindAsync(x => !x.IsDeleted && x.Barcode == barcode);
        }
    }
}
