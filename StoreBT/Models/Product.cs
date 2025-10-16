using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBT.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();      // Mã định danh duy nhất
        public string Name { get; set; } = string.Empty;     // Tên sản phẩm
        public string Category { get; set; } = string.Empty; // Loại sản phẩm
        public string Barcode { get; set; } = string.Empty;  // Mã vạch
        public decimal Price { get; set; }                   // Giá bán
        public int Stock { get; set; }                       // Số lượng tồn
        public string? Description { get; set; }             // Mô tả thêm
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
