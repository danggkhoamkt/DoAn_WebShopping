using WebShopping.Models;
namespace WebShopping.Helpers
{
    public static class CartSession
    {
        public const string Key = "CART";
        public static List<CartItem> Get(ISession session)
        {
            return session.GetJson<List<CartItem>>(Key) ?? new List<CartItem>();
        }
        public static void Save(ISession session, List<CartItem> cart)
        {
            session.SetJson(Key, cart);
        }
        public static void Clear(ISession session)
        {
            session.Remove(Key);
        }
    }
}