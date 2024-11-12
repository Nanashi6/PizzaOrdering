using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.Models;

namespace PizzaOrdering.LogicLayer;

public class Cart
{
    public List<CartItem> cartItems { get; init; } = new List<CartItem>();

    public void AddItem(PizzaInfoViewModel pizza, int quantity)
    {
        CartItem? line = cartItems.FirstOrDefault(g => g.Pizza.Id == pizza.Id);

        if (line == null)
        {
            cartItems.Add(new CartItem
            {
                Pizza = pizza,
                Quantity = quantity
            });
        }
        else
        {
            line.Quantity += quantity;
        }
    }

    public void RemoveItem(Pizza pizza)
    {
        cartItems.RemoveAll(l => l.Pizza.Id == pizza.Id);
    }

    public void Clear()
    {
        cartItems.Clear();
    }
}

public class CartItem
{
    public PizzaInfoViewModel Pizza { get; set; }
    public int Quantity { get; set; }
}