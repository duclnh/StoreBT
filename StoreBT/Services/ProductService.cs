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
            // Dữ liệu mẫu ban đầu
            _products.Add(new Product { Name = "Áo thun trắng", Category = "Thời trang", Barcode = "8938505970012", Price = 159000, Stock = 120 });
            _products.Add(new Product { Name = "Giày sneaker", Category = "Giày dép", Barcode = "8938505970029", Price = 799000, Stock = 42 });
            _products.Add(new Product { Name = "Quần jean xanh", Category = "Thời trang", Barcode = "8938505970036", Price = 299000, Stock = 75 });
            _products.Add(new Product { Name = "Áo sơ mi caro", Category = "Thời trang", Barcode = "8938505970043", Price = 259000, Stock = 60 });
            _products.Add(new Product { Name = "Balo laptop", Category = "Phụ kiện", Barcode = "8938505970050", Price = 499000, Stock = 33 });
            _products.Add(new Product { Name = "Tai nghe Bluetooth", Category = "Điện tử", Barcode = "8938505970067", Price = 649000, Stock = 28 });
            _products.Add(new Product { Name = "Chuột không dây", Category = "Điện tử", Barcode = "8938505970074", Price = 229000, Stock = 45 });
            _products.Add(new Product { Name = "Bàn phím cơ", Category = "Điện tử", Barcode = "8938505970081", Price = 890000, Stock = 22 });
            _products.Add(new Product { Name = "Áo khoác gió", Category = "Thời trang", Barcode = "8938505970098", Price = 359000, Stock = 52 });
            _products.Add(new Product { Name = "Mũ lưỡi trai", Category = "Phụ kiện", Barcode = "8938505970104", Price = 99000, Stock = 88 });
            _products.Add(new Product { Name = "Bình giữ nhiệt", Category = "Gia dụng", Barcode = "8938505970111", Price = 189000, Stock = 64 });
            _products.Add(new Product { Name = "Ly thủy tinh", Category = "Gia dụng", Barcode = "8938505970128", Price = 59000, Stock = 140 });
            _products.Add(new Product { Name = "Túi tote vải", Category = "Phụ kiện", Barcode = "8938505970135", Price = 129000, Stock = 110 });
            _products.Add(new Product { Name = "Sạc dự phòng", Category = "Điện tử", Barcode = "8938505970142", Price = 459000, Stock = 25 });
            _products.Add(new Product { Name = "Ốp lưng điện thoại", Category = "Phụ kiện", Barcode = "8938505970159", Price = 79000, Stock = 95 });
            _products.Add(new Product { Name = "Quần short kaki", Category = "Thời trang", Barcode = "8938505970166", Price = 199000, Stock = 70 });
            _products.Add(new Product { Name = "Giày cao gót", Category = "Giày dép", Barcode = "8938505970173", Price = 699000, Stock = 34 });
            _products.Add(new Product { Name = "Dép lê nam", Category = "Giày dép", Barcode = "8938505970180", Price = 99000, Stock = 55 });
            _products.Add(new Product { Name = "Nón bucket", Category = "Phụ kiện", Barcode = "8938505970197", Price = 149000, Stock = 40 });
            _products.Add(new Product { Name = "Áo hoodie unisex", Category = "Thời trang", Barcode = "8938505970203", Price = 429000, Stock = 38 });
            _products.Add(new Product { Name = "Khăn choàng len", Category = "Phụ kiện", Barcode = "8938505970210", Price = 179000, Stock = 50 });
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
