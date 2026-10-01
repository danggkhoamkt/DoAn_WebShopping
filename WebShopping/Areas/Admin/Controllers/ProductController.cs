using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShopping.Data;
using WebShopping.Models;

namespace WebShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        // Thư mục lưu ảnh, tính từ wwwroot
        private const string UploadFolder = "client/img/product-img";

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();
            return View(products);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? ImageFile)
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Vui lòng chọn ảnh sản phẩm.");
            }

            if (ModelState.IsValid)
            {
                product.ImageUrl = await SaveImageAsync(ImageFile!);

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm sản phẩm thành công.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? ImageFile)
        {
            if (id != product.ProductId) return NotFound();

            // Ảnh không bắt buộc chọn lại khi sửa, nên bỏ qua lỗi validate mặc định của ImageUrl nếu có
            ModelState.Remove(nameof(Product.ImageUrl));

            if (ModelState.IsValid)
            {
                var existing = await _context.Products.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProductId == id);
                if (existing == null) return NotFound();

                // Nếu người dùng chọn ảnh mới thì thay thế, không thì giữ ảnh cũ
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    product.ImageUrl = await SaveImageAsync(ImageFile);
                    DeleteImageFile(existing.ImageUrl);
                }
                else
                {
                    product.ImageUrl = existing.ImageUrl;
                }

                try
                {
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật sản phẩm thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.ProductId == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            DeleteImageFile(product.ImageUrl);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        // Lưu file ảnh vào wwwroot/client/img/product-img, trả về tên file để lưu vào DB
        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var folderPath = Path.Combine(_env.WebRootPath, UploadFolder);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        private void DeleteImageFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            var filePath = Path.Combine(_env.WebRootPath, UploadFolder, fileName);
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}