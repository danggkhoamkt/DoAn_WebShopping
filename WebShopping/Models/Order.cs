using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopping.Models
{
    public enum OrderStatus
    {
        Pending,
        Shipping,
        Completed,
        Cancelled
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(200)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(200)]
        [RegularExpression(@"^(0|\+84)(3|5|7|8|9)[0-9]{8}$",
            ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống")]
        public string Address { get; set; }

        public string? Note { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}