using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopping.Data;
using WebShopping.Helpers;
using WebShopping.Models;
namespace WebShopping.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        public CartController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(CartSession.Get(HttpContext.Session));
        }
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            if (quantity < 1) quantity = 1;
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();
            var cart = CartSession.Get(HttpContext.Session);
            var existing = cart.FirstOrDefault(x => x.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ImageUrl = product.ImageUrl,
                    UnitPrice = product.Price,
                    Quantity = quantity
                });
            }
            CartSession.Save(HttpContext.Session, cart);
            TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int productId, int quantity)
        {
            var cart = CartSession.Get(HttpContext.Session);
            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                if (quantity < 1)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }
                CartSession.Save(HttpContext.Session, cart);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int productId)
        {
            var cart = CartSession.Get(HttpContext.Session);
            cart.RemoveAll(x => x.ProductId == productId);
            CartSession.Save(HttpContext.Session, cart);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Clear()
        {
            CartSession.Clear(HttpContext.Session);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = CartSession.Get(HttpContext.Session);
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng đang trống.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Cart = cart;
            return View(new Order());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = CartSession.Get(HttpContext.Session);
            ViewBag.Cart = cart;
            if (!cart.Any())
            {
                ModelState.AddModelError(string.Empty, "Giỏ hàng đang trống.");
                return View(order);
            }
            ModelState.Remove(nameof(Order.OrderDetails));
            ModelState.Remove(nameof(Order.Status));
            ModelState.Remove(nameof(Order.OrderDate));
            ModelState.Remove(nameof(Order.TotalAmount));
            if (!ModelState.IsValid)
            {
                return View(order);
            }
            order.OrderDate = DateTime.Now;
            order.Status = OrderStatus.Pending;
            order.TotalAmount = cart.Sum(x => x.LineTotal);
            order.OrderDetails = cart.Select(x => new OrderDetail
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            }).ToList();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            CartSession.Clear(HttpContext.Session);
            return RedirectToAction(nameof(Success), new { id = order.OrderId });
        }
        public async Task<IActionResult> Success(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)!
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null) return NotFound();
            return View(order);
        }
    }
}