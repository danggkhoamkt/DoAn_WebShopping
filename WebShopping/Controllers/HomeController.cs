using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopping.Data;
using WebShopping.Models;

namespace WebShopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy 9 sản phẩm mới nhất
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.ProductId)
                .Take(9)
                .ToListAsync();

            return View(products);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}