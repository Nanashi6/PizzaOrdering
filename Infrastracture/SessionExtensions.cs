using System.Text.Json;
using PizzaOrdering.LogicLayer;

namespace PizzaOrdering.Infrastracture;

public static class SessionExtensions
{
    //Запись объекта типа Dictionary<string, string> в сессию
    public static void SetCart(this ISession session, string key, Cart cart)
    {
        session.SetString(key, JsonSerializer.Serialize(cart));
    }
    //Считывание объекта типа Dictionary<string, string> из сессии
    public static Cart? GetCart(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value==null ? null : JsonSerializer.Deserialize<Cart>(value);
    }
}