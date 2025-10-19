namespace StoreBT.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();          // Mã chi tiết đơn hàng

        public Guid OrderId { get; set; }                       // Khóa ngoại liên kết Order
        public Order? Order { get; set; }                        // Đơn hàng chứa item này

        public Guid ProductId { get; set; }                     // Khóa ngoại tới sản phẩm
        public Product? Product { get; set; }                    // Thông tin sản phẩm

        public int Quantity { get; set; }                       // Số lượng
        public decimal UnitPrice { get; set; }                  // Đơn giá
        public decimal Total => UnitPrice * Quantity;           // Thành tiền của dòng sản phẩm
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }

}
