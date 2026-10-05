using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace WebShopping.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(200)]
        [Display(Name = "Tên danh mục")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
        [ValidateNever]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}