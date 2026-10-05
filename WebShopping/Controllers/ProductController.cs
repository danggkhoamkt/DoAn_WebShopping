using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopping.Data;

namespace WebShopping.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Product?categoryId=2
        public async Task<IActionResult> Index(int? categoryId)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = await query
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();

            // Danh sách danh mục để hiển thị bộ lọc
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            ViewBag.SelectedCategoryId = categoryId;

            return View(products);
        }

        // GET: /Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null) return NotFound();

            // Gợi ý thêm vài sản phẩm cùng danh mục
            ViewBag.RelatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != product.ProductId)
                .Take(4)
                .ToListAsync();

            return View(product);
        }
    }
}