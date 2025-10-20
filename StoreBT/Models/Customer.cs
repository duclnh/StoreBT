using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreBT.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();         // Mã khách hàng (tự tăng)
        public string Name { get; set; }           // Tên khách hàng
        public string Phone { get; set; }          // Số điện thoại
        public string Address { get; set; }        // Địa chỉ
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }

}
