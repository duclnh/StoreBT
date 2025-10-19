namespace StoreBT.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();      // Mã đơn hàng (tự sinh)
        public string OrderCode { get; set; } = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
        public Guid CustomerId { get; set; }                // Mã khách hàng (khóa ngoại)
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public Customer? Customer { get; set; }              // Thông tin khách hàng

        public DateTime OrderDate { get; set; } = DateTime.Now;  // Ngày tạo đơn
        public decimal TotalAmount { get; set; }                 // Tổng tiền gốc
        public decimal Discount { get; set; }                    // Giảm giá (theo tiền)
        public decimal FinalAmount => TotalAmount - Discount;    // Thành tiền sau giảm giá
        public string? Notes { get; set; }                       // Ghi chú thêm
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public List<OrderItem>? Items { get; set; }
    }
}
