using System.ComponentModel.DataAnnotations;

namespace WebShopping.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(200)]
        public string CategoryName { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}