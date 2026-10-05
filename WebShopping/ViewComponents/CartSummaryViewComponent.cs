using Microsoft.AspNetCore.Mvc;
using WebShopping.Helpers;
using WebShopping.Models;
namespace WebShopping.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = CartSession.Get(HttpContext.Session);
            return View(cart);
        }
    }
}