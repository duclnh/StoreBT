using StoreBT.Models;
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
        private readonly List<Product> _products = new();

        public ProductService()
        {
            // dữ liệu mẫu ban đầu
            _products.Add(new Product
            {
                Name = "Áo thun trắng",
                Category = "Thời trang",
                Barcode = "8938505970012",
                Price = 159000,
                Stock = 120
            });

            _products.Add(new Product
            {
                Name = "Giày sneaker",
                Category = "Giày dép",
                Barcode = "8938505970029",
                Price = 799000,
                Stock = 42
            });
        }

        public Task<IEnumerable<Product>> GetAllAsync() => Task.FromResult(_products.AsEnumerable());

        public Task<Product?> GetByBarcodeAsync(string barcode)
        {
            var p = _products.FirstOrDefault(x => x.Barcode == barcode);
            return Task.FromResult(p);
        }

        public Task AddAsync(Product product)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Product product)
        {
            var index = _products.FindIndex(p => p.Barcode == product.Barcode);
            if (index >= 0)
                _products[index] = product;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string barcode)
        {
            _products.RemoveAll(p => p.Barcode == barcode);
            return Task.CompletedTask;
        }
    }
}
