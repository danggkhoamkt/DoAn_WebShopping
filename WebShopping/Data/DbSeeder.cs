using WebShopping.Models;
namespace WebShopping.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Username = "admin",
                    Password = "123456",
                    FullName = "Quản trị viên",
                    Role = "Admin"
                });
                context.SaveChanges();
            }
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { CategoryName = "Woman Wear", Description = "Thời trang nữ" },
                    new Category { CategoryName = "Man Wear", Description = "Thời trang nam" },
                    new Category { CategoryName = "Children", Description = "Thời trang trẻ em" },
                    new Category { CategoryName = "Bags & Purses", Description = "Túi xách" },
                    new Category { CategoryName = "Eyewear", Description = "Kính mắt" },
                    new Category { CategoryName = "Footwear", Description = "Giày dép" }
                );
                context.SaveChanges();
            }
            if (!context.Products.Any())
            {
                var cats = context.Categories.ToList();
                int Id(string name) => cats.First(c => c.CategoryName == name).CategoryId;
                context.Products.AddRange(
                    new Product { ProductName = "Yellow Cocktail Dress", Price = 890000, ImageUrl = "product-1.jpg", Description = "Đầm cocktail vàng, phù hợp dự tiệc.", CategoryId = Id("Woman Wear") },
                    new Product { ProductName = "White Summer Dress", Price = 750000, ImageUrl = "product-2.jpg", Description = "Đầm hè trắng nhẹ nhàng.", CategoryId = Id("Woman Wear") },
                    new Product { ProductName = "Classic Man Shirt", Price = 420000, ImageUrl = "product-3.jpg", Description = "Áo sơ mi nam cổ điển.", CategoryId = Id("Man Wear") },
                    new Product { ProductName = "Casual Man Jacket", Price = 980000, ImageUrl = "product-4.jpg", Description = "Áo khoác nam casual.", CategoryId = Id("Man Wear") },
                    new Product { ProductName = "Kids Color Dress", Price = 320000, ImageUrl = "product-5.jpg", Description = "Váy màu sắc cho bé.", CategoryId = Id("Children") },
                    new Product { ProductName = "Kids Sport Set", Price = 280000, ImageUrl = "product-6.jpg", Description = "Bộ thể thao trẻ em.", CategoryId = Id("Children") },
                    new Product { ProductName = "Leather Handbag", Price = 1250000, ImageUrl = "product-7.jpg", Description = "Túi xách da thời trang.", CategoryId = Id("Bags & Purses") },
                    new Product { ProductName = "Mini Crossbody Bag", Price = 540000, ImageUrl = "product-8.jpg", Description = "Túi đeo chéo nhỏ gọn.", CategoryId = Id("Bags & Purses") },
                    new Product { ProductName = "Round Sunglasses", Price = 390000, ImageUrl = "product-9.jpg", Description = "Kính râm tròn unisex.", CategoryId = Id("Eyewear") },
                    new Product { ProductName = "Classic Sneakers", Price = 690000, ImageUrl = "product-10.jpg", Description = "Giày sneaker cổ điển.", CategoryId = Id("Footwear") },
                    new Product { ProductName = "Ankle Boots", Price = 880000, ImageUrl = "product-11.jpg", Description = "Boot cổ thấp.", CategoryId = Id("Footwear") },
                    new Product { ProductName = "Evening Heels", Price = 720000, ImageUrl = "product-12.jpg", Description = "Giày cao gót dự tiệc.", CategoryId = Id("Footwear") }
                );
                context.SaveChanges();
            }
        }
    }
}