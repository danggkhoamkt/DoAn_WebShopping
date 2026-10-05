using WebShopping.Models;
namespace WebShopping.Helpers
{
    public static class OrderStatusHelper
    {
        public static string ToVietnamese(OrderStatus status) => status switch
        {
            OrderStatus.Pending => "Chờ xử lý",
            OrderStatus.Shipping => "Đang giao",
            OrderStatus.Completed => "Hoàn thành",
            OrderStatus.Cancelled => "Đã hủy",
            _ => status.ToString()
        };
        public static string CssClass(OrderStatus status) => status switch
        {
            OrderStatus.Pending => "label label-warning",
            OrderStatus.Shipping => "label label-info",
            OrderStatus.Completed => "label label-success",
            OrderStatus.Cancelled => "label label-danger",
            _ => "label label-default"
        };
    }
}