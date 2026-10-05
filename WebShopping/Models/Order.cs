using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(200)]
        [RegularExpression(@"^(0|\+84)(3|5|7|8|9)[0-9]{8}$",
            ErrorMessage = "Số điện thoại không đúng định dạng Việt Nam")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [ValidateNever]
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}